using System;
using System.IO;
using Microsoft.Win32;
using Xunit;

namespace Rhino.Licensing.Tests
{
    public class Can_check_trial_date : BaseLicenseTest
    {
        [Fact]
        public void Will_tell_that_we_are_in_invalid_state()
        {
            var validator = new LicenseValidator(public_only, Path.GetTempFileName());
            Assert.Throws<LicenseNotFoundException>(validator.AssertValidLicense);
        }


        [Fact]
        public void Will_fail_if_file_is_not_there()
        {
            var validator = new LicenseValidator(public_only, "not_there");
            Assert.Throws<LicenseFileNotFoundException>(validator.AssertValidLicense);
        }
    }
}