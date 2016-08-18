using System;
using System.IO;
using Xunit;

namespace Rhino.Licensing.Tests
{
    public class License_From_File_And_String_Validate_Correctly : BaseLicenseTest
    {
        [Fact]
        public void Will_tell_that_we_are_in_invalid_state()
        {

            var guid = Guid.NewGuid();
            var generator = new LicenseGenerator(public_and_private);
            var expiration = DateTime.Now.AddDays(-30);
            var key = generator.Generate("Oren Eini", guid, expiration, LicenseType.Trial);
            var path = Path.GetTempFileName();
            File.WriteAllText(path, key);
            var validator = new LicenseValidator(public_only, Path.GetTempFileName());
            Assert.Throws<LicenseNotFoundException>(() => new LicenseValidator(public_only, Path.GetTempFileName()).AssertValidLicense());
            Assert.Throws<LicenseExpiredException>(() => new LicenseValidator(public_only, null, key).AssertValidLicense());
        }

        [Fact]
        public void Will_tell_that_license_can_load_from_file()
        {
            var guid = Guid.NewGuid();
            var generator = new LicenseGenerator(public_and_private);
            var expiration = DateTime.Now.AddDays(30);
            var key = generator.Generate("Oren Eini", guid, expiration, LicenseType.Trial);
            var path = Path.GetTempFileName();
            File.WriteAllText(path, key);
            new LicenseValidator(public_only, Path.GetTempFileName()).AssertValidLicense();
        }

        [Fact]
        public void Will_tell_that_license_can_load_from_string()
        {
            var guid = Guid.NewGuid();
            var generator = new LicenseGenerator(public_and_private);
            var expiration = DateTime.Now.AddDays(30);
            var key = generator.Generate("Oren Eini", guid, expiration, LicenseType.Trial);
            new LicenseValidator(public_only, null, key).AssertValidLicense();
        }
    }
}
