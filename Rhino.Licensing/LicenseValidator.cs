using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Rhino.Licensing
{
	using System.ServiceModel;
	using System.Threading;

	public class LicenseValidator
	{
		private readonly string licensePath;
		private readonly string licenseServerUrl;
		private readonly Guid clientId;
		private readonly string publicKey;

		public event Action LicenseInvalidated;

		public DateTime ExpirationDate { get; private set; }
		public LicenseType LicenseType { get; private set; }
		public Guid UserId { get; private set; }
		public string Name { get; private set; }

		private readonly Timer nextLeaseTimer;

		private void LeaseLicenseAgain(object state)
		{
			if (TryValidate())
				return;
			if (LicenseInvalidated == null)
				throw new InvalidOperationException("License was invalidated, but there is no one subscribe to the LicenseInvalidated event");
			LicenseInvalidated();
		}

		public LicenseValidator(string publicKey, string licensePath)
		{
			nextLeaseTimer = new Timer(LeaseLicenseAgain);
			this.publicKey = publicKey;
			this.licensePath = licensePath;
		}

		public LicenseValidator(string publicKey, string licensePath, string licenseServerUrl, Guid clientId)
		{
			nextLeaseTimer = new Timer(LeaseLicenseAgain);
			this.publicKey = publicKey;
			this.licensePath = licensePath;
			this.licenseServerUrl = licenseServerUrl;
			this.clientId = clientId;
		}


		public void AssertValidLicense()
		{
			if (File.Exists(licensePath) == false)
				throw new LicenseFileNotFoundException();

			if (HasExistingLicense())
				return;

			throw new LicenseNotFoundException();
		}

		private bool HasExistingLicense()
		{
			try
			{
				if (File.Exists(licensePath) == false)
					return false;

				if (TryValidate() == false)
					return false;

				return DateTime.Now < ExpirationDate;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void RemoveExistingLicense()
		{
			File.Delete(licensePath);
		}

		private bool TryValidate()
		{
			try
			{
				var doc = new XmlDocument();
				doc.Load(licensePath);

				if (TryGetValidDocument(publicKey, doc) == false)
					return false;

				if (doc.FirstChild == null)
					return false;

				if (doc.SelectSingleNode("/floating-license") != null)
				{
					var node = doc.SelectSingleNode("/floating-license/license-server-public-key/text()");
					if (node == null)
						throw new InvalidOperationException(
							"Invalid license file format, floating license without license server public key");
					return ValidateFloatingLicense(node.InnerText);
				}

				return ValidateXmlDocumentLicense(doc);
			}
			catch (Exception)
			{
				return false;
			}
		}

		private bool ValidateFloatingLicense(string publicKeyOfFloatingLicense)
		{
			if (licenseServerUrl == null)
				throw new InvalidOperationException("Floating license encountered, but licenseServerUrl was not set");

			var success = false;
			var licensingService = ChannelFactory<ILicensingService>.CreateChannel(new WSHttpBinding(), new EndpointAddress(licenseServerUrl));
			try
			{
				var leasedLicense = licensingService.LeaseLicense(clientId);
				((ICommunicationObject)licensingService).Close();
				success = true;
				if (leasedLicense == null)
					return false;

				var doc = new XmlDocument();
				doc.LoadXml(leasedLicense);

				if (TryGetValidDocument(publicKeyOfFloatingLicense, doc) == false)
					return false;

				var validLicense = ValidateXmlDocumentLicense(doc);
				if (validLicense)
				{
					//setup next lease
					var time = (ExpirationDate.AddMinutes(-5) - DateTime.Now);
					nextLeaseTimer.Change(time, time);
				}
				return validLicense;
			}
			finally
			{
				if (success == false)
					((ICommunicationObject)licensingService).Abort();
			}
		}

		internal bool ValidateXmlDocumentLicense(XmlDocument doc)
		{
			XmlNode id = doc.SelectSingleNode("/license/@id");
			if (id == null)
				return false;

			UserId = new Guid(id.Value);

			XmlNode date = doc.SelectSingleNode("/license/@expiration");
			if (date == null)
				return false;

			ExpirationDate = XmlConvert.ToDateTime(date.Value, XmlDateTimeSerializationMode.Utc);

			XmlNode licenseType = doc.SelectSingleNode("/license/@type");
			if (licenseType == null)
				return false;

			LicenseType = (LicenseType)Enum.Parse(typeof(LicenseType), licenseType.Value);

			XmlNode name = doc.SelectSingleNode("/license/name/text()");
			if (name == null)
				return false;

			Name = name.Value;

			return true;
		}

		private bool TryGetValidDocument(string licensePrivateKey, XmlDocument doc)
		{
			var rsa = new RSACryptoServiceProvider();
			rsa.FromXmlString(licensePrivateKey);

			var nsMgr = new XmlNamespaceManager(doc.NameTable);
			nsMgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");

			var signedXml = new SignedXml(doc);
			var sig = (XmlElement)doc.SelectSingleNode("//sig:Signature", nsMgr);
			signedXml.LoadXml(sig);

			return signedXml.CheckSignature(rsa);
		}
	}
}
