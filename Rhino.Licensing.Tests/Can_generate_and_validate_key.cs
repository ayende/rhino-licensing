using System;
using System.IO;
using Xunit;

namespace Rhino.Licensing.Tests
{
    public class Can_generate_and_validate_key : BaseLicenseTest
    {
        [Fact]
        public void Gen_and_validate()
        {
            var guid = Guid.NewGuid();
            var generator = new LicenseGenerator(public_and_private);
            var expiration = DateTime.Now.AddDays(30);
            var key = generator.Generate("Oren Eini", guid, expiration, LicenseType.Trial);

            var path = Path.GetTempFileName();
            File.WriteAllText(path, key);

            var validator = new LicenseValidator(public_only, path);
            validator.AssertValidLicense();
            
            Assert.Equal(guid, validator.UserId);
            Assert.Equal(expiration, validator.ExpirationDate);
            Assert.Equal("Oren Eini", validator.Name);
            Assert.Equal(LicenseType.Trial, validator.LicenseType);
        }

        [Fact]
        public void Cannot_validate_hacked_license()
        {
            const string hackedLicense =
                @"<?xml version=""1.0"" encoding=""utf-16""?>
<license id=""2bc65446-0c78-453f-9da3-badb9f191163"" expiration=""2009-04-04T12:16:45.4328736"" type=""Trial"">
  <name>Oren Eini</name>
  <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
    <SignedInfo>
      <CanonicalizationMethod Algorithm=""http://www.w3.org/TR/2001/REC-xml-c14n-20010315"" />
      <SignatureMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#rsa-sha1"" />
      <Reference URI="""">
        <Transforms>
          <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
        </Transforms>
        <DigestMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#sha1"" />
        <DigestValue>KxG48zaXJ0gEOpuCc4NPYTh7U7c=</DigestValue>
      </Reference>
    </SignedInfo>
    <SignatureValue>KfNKJRDwPeJ2Olw+sbe1RLLrzQzMAf/Kwcs34LkRMR/bRYTvm+RX1KMr0/+bNxtg0NCqMjMgPqrZfx3V416GMhlwMIFzFSRzF8Z/khKHXM2Hbur3ibeyMoj5GGTx5sUKJy0v5eVaLvE9pDc5pafVaUynk/5NDgCQR6wBbKtzLJamaX+zigS8uXGvDxcMAGSeY97wtATdXBawrWbfQgeJ72h0FHshaepqS5roNXgr/5oV4ma/KWTrwTZVBo76ThkgB4HzsZGqjAo9vZa5eUHQwrJfNEEwoHnu4Ld8PfKErwTbr6Q8GK8CSxldP5HJ0BALuj1bETlDum6/vcZZXGEtsQ==</SignatureValue>
  </Signature>
</license>";

            var path = Path.GetTempFileName();
            File.WriteAllText(path, hackedLicense);

            var validator = new LicenseValidator(public_only, path);
            Assert.Throws<LicenseNotFoundException>(validator.AssertValidLicense);
        }
    }
}