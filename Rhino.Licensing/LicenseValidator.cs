using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using log4net;

namespace Rhino.Licensing
{
	using System.Globalization;
	using System.ServiceModel;
	using System.Threading;

	public class LicenseValidator
	{
		private readonly ILog log = LogManager.GetLogger(typeof(LicenseValidator));

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
			{
				log.WarnFormat("Could not find license file: {0}", licensePath);
				throw new LicenseFileNotFoundException();
			}

			if (HasExistingLicense())
				return;
			log.WarnFormat("Could not find an existing license in {0}", licensePath);
			throw new LicenseNotFoundException();
		}

		private bool HasExistingLicense()
		{
			try
			{
				if (File.Exists(licensePath) == false)
				{
					log.WarnFormat("Could not find license file: {0}", licensePath);
					return false;
				}

				if (TryValidate() == false)
				{
					log.WarnFormat("Failed validating license file: {0}", licensePath);
					return false;
				}
				log.InfoFormat("License {0} expiration date is {1}", licensePath, ExpirationDate);
				
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
				{
					log.WarnFormat("Could not validate xml signature of {0}", licensePath);
					return false;
				}

				if (doc.FirstChild == null)
				{
					log.WarnFormat("Could not find first child of {0}", licensePath);
					return false;
				}

				if (doc.SelectSingleNode("/floating-license") != null)
				{
					var node = doc.SelectSingleNode("/floating-license/license-server-public-key/text()");
					if (node == null)
					{
						log.WarnFormat("Invalid license file {0}, floating license without license server public key", licensePath);
						throw new InvalidOperationException(
							"Invalid license file format, floating license without license server public key");
					}
					return ValidateFloatingLicense(node.InnerText);
				}

				return ValidateXmlDocumentLicense(doc);
			}
			catch (Exception e)
			{
				log.Error("Could not validate license", e);
				return false;
			}
		}

		private bool ValidateFloatingLicense(string publicKeyOfFloatingLicense)
		{
			if (licenseServerUrl == null)
			{
				log.Warn("Could not find license server url");
				throw new InvalidOperationException("Floating license encountered, but licenseServerUrl was not set");
			}

			var success = false;
			var licensingService = ChannelFactory<ILicensingService>.CreateChannel(new WSHttpBinding(), new EndpointAddress(licenseServerUrl));
			try
			{
				var leasedLicense = licensingService.LeaseLicense(
					Environment.MachineName,
					Environment.UserName,
					clientId);
				((ICommunicationObject)licensingService).Close();
				success = true;
				if (leasedLicense == null)
					return false;

				var doc = new XmlDocument();
				doc.LoadXml(leasedLicense);

				if (TryGetValidDocument(publicKeyOfFloatingLicense, doc) == false)
				{
					log.WarnFormat("Could not get valid license from floating license server{0}", licensePath);
					return false;
				}

				var validLicense = ValidateXmlDocumentLicense(doc);
				if (validLicense)
				{
					//setup next lease
					var time = (ExpirationDate.AddMinutes(-5) - DateTime.Now);
					log.DebugFormat("Will lease license again at {0}", time);
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
			{
				log.WarnFormat("Could not find id attribute in {0}", licensePath);
				return false;
			}

			UserId = new Guid(id.Value);

			XmlNode date = doc.SelectSingleNode("/license/@expiration");
			if (date == null)
			{
				log.WarnFormat("Could not find expiration in {0}", licensePath);
				return false;
			}
			
			ExpirationDate = DateTime.ParseExact(date.Value, "yyyy-MM-ddTHH:mm:ss.fffffff", CultureInfo.InvariantCulture);

			XmlNode licenseType = doc.SelectSingleNode("/license/@type");
			if (licenseType == null)
			{
				log.WarnFormat("Could not find license type in {0}", licenseType);
				return false;
			}

			LicenseType = (LicenseType)Enum.Parse(typeof(LicenseType), licenseType.Value);

			XmlNode name = doc.SelectSingleNode("/license/name/text()");
			if (name == null)
			{
				log.WarnFormat("Could not find licensee's name in {0}", licensePath);
				return false;
			}

			Name = name.Value;

			return true;
		}

		private bool TryGetValidDocument(string licensePublicKey, XmlDocument doc)
		{
			var rsa = new RSACryptoServiceProvider();
			rsa.FromXmlString(licensePublicKey);

			var nsMgr = new XmlNamespaceManager(doc.NameTable);
			nsMgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");

			var signedXml = new SignedXml(doc);
			var sig = (XmlElement)doc.SelectSingleNode("//sig:Signature", nsMgr);
			if(sig == null)
			{
				log.WarnFormat("Could not find this signature node on {0}", licensePath);
				return false;
			}
			signedXml.LoadXml(sig);

			return signedXml.CheckSignature(rsa);
		}
	}
}
