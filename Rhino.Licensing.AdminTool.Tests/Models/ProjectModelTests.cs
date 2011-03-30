using Rhino.Licensing.AdminTool.Model;
using Xunit;
using Caliburn.Testability.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.Models
{
    public class ProjectModelTests
    {
        [Fact]
        public void Project_Fires_Property_Changed()
        {
            var project = new Project();

            project.AssertThatAllProperties()
                   .RaiseChangeNotification();
        }
    }
}