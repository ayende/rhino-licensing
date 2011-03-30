using Rhino.Licensing.AdminTool.Model;
using Xunit;
using Caliburn.Testability.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.Models
{
    public class LicenseModelTests
    {
        [Fact]
        public void License_Fires_Property_Changed()
        {
            var license = new License();

            license.AssertThatAllProperties()
                   .RaiseChangeNotification();
        }
    }
}