namespace Rhino.Licensing.Tests
{
	using System;
	using System.IO;
	using System.ServiceModel;
	using Xunit;

	public class Can_use_floating_licenses : BaseLicenseTest
	{
		[Fact]
		public void Can_generate_floating_license()
		{
			const string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<floating-license>
  <license-server-public-key>&lt;RSAKeyValue&gt;&lt;Modulus&gt;tOAa81fDkKAIcmBx5SybBQM34OG12Qsbm0V8H10Q5iL3bFIco1S6BFyKRK84LKitSPczY3z62imwNkanDVfXhnhl2UFTS0MTkhXM+yG9xFRGc3QwIcNE1j7UFAENo7RS1eguVQaYm26uaqgYXWHJn352CzddV7Lv4M3lAe6oh2M=&lt;/Modulus&gt;&lt;Exponent&gt;AQAB&lt;/Exponent&gt;&lt;/RSAKeyValue&gt;</license-server-public-key>
  <name>ayende</name>
  <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
    <SignedInfo>
      <CanonicalizationMethod Algorithm=""http://www.w3.org/TR/2001/REC-xml-c14n-20010315"" />
      <SignatureMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#rsa-sha1"" />
      <Reference URI="""">
        <Transforms>
          <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
        </Transforms>
        <DigestMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#sha1"" />
        <DigestValue>t7E+c2Q3ZU6/H6su57vpC7zTxC0=</DigestValue>
      </Reference>
    </SignedInfo>
    <SignatureValue>CoC8WI7WyfPwJGY3jHC4/OJZ2QrJD38YHC+IsivD2p0LwiOmMv+BuwwznPk+MEKQGQxWKzVFJwFXYhmbdDnYoX1ad0xb9q93kKLVu1HTts2682SOpvUhBC9JDXUPdYKPuA+eZNNAaLmfjwsYNzMDlwZlFLOV4S8bPAnnnk1cy01pRPT9nWZS89S86fpN0ws+XPuaS6yj9luv5DCNMYKa18loDnuKuD6hAyby3HWVDcRdyjd8yDCCHH090hubjUubSIFFRSR2CLiK0aQ5fqDJEEdxiI9F7s/r+qz2Ou5aAo2jOEAua+jLdzX/bUzFlUadw8daTEYf82hnDCmFO/BbnQ==</SignatureValue>
  </Signature>
</floating-license>";

			var generator = new LicenseGenerator(public_and_private);
			var license = generator.GenerateFloatingLicense("ayende", floating_public);
			Assert.Equal(expected, license);
		}

		[Fact]
		public void Can_validate_floating_license()
		{
			string fileName = WriteFloatingLicenseFile();

			GenerateLicenseFileInLicensesDirectory();

			LicensingService.SoftwarePublicKey = public_only;
			LicensingService.LicenseServerPrivateKey = floating_private;

			var host = new ServiceHost(typeof(LicensingService));
			const string address = "http://localhost:9292/license";
			host.AddServiceEndpoint(typeof(ILicensingService), new WSHttpBinding(), address);

			host.Open();

			var validator = new LicenseValidator(public_only, fileName, address, Guid.NewGuid());
			validator.AssertValidLicense();

			host.Abort();
		}

		[Fact]
		public void Can_only_get_license_per_allocated_licenses()
		{
			string fileName = WriteFloatingLicenseFile();

			GenerateLicenseFileInLicensesDirectory();

			LicensingService.SoftwarePublicKey = public_only;
			LicensingService.LicenseServerPrivateKey = floating_private;

			var host = new ServiceHost(typeof(LicensingService));
			var address = "http://localhost:9292/license";
			host.AddServiceEndpoint(typeof(ILicensingService), new WSHttpBinding(), address);

			host.Open();

			var validator = new LicenseValidator(public_only, fileName, address, Guid.NewGuid());
			validator.AssertValidLicense();

			var validator2 = new LicenseValidator(public_only, fileName, address, Guid.NewGuid());
			Assert.Throws<LicenseNotFoundException>(validator2.AssertValidLicense);

			host.Abort();
		}

		private void GenerateLicenseFileInLicensesDirectory()
		{
			var generator = new LicenseGenerator(public_and_private);
			var generate = generator.Generate("ayende", Guid.NewGuid(), DateTime.MaxValue, LicenseType.Standard);
			var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Licenses");
			if (Directory.Exists(dir) == false)
				Directory.CreateDirectory(dir);
			File.WriteAllText(Path.Combine(dir, "ayende.xml"), generate);
		}

		private string WriteFloatingLicenseFile()
		{
			var generator = new LicenseGenerator(public_and_private);
			var license = generator.GenerateFloatingLicense("ayende", floating_public);
			var fileName = Path.GetTempFileName();
			File.WriteAllText(fileName, license);
			return fileName;
		}
	}
}