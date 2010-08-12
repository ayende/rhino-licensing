using Rhino.Licensing.AdminTool.Model;
using Rhino.Licensing.AdminTool.ViewModels;
using Xunit;
using Caliburn.Testability.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.ViewModels
{
    public class ProjectViewModelTests
    {
        [Fact]
        public void Creating_New_ProductViewModel_Will_Have_Empty_Product()
        {
            var vm = new ProjectViewModel();
            
            Assert.Null(vm.CurrentProject);
        }

        [Fact]
        public void Fires_PropertyChange_Notification()
        {
            var vm = new ProjectViewModel();

            vm.AssertThatProperty(x => x.CurrentProject).RaisesChangeNotification();
        }
    }
}