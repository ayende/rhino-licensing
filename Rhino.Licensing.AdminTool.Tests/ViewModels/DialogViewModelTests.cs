using Rhino.Licensing.AdminTool.ViewModels;
using Xunit;
using Caliburn.Testability.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.ViewModels
{
    public class DialogViewModelTests
    {
        [Fact]
        public void FileDialogModel_Fires_PropertyChange()
        {
            var vm = new OpenFileDialogViewModel();

            vm.AssertThatAllProperties()
              .RaiseChangeNotification();
        }

        [Fact]
        public void SaveDialogModel_Fires_PropertyChange()
        {
            var vm = new SaveFileDialogViewModel();

            vm.AssertThatAllProperties()
              .RaiseChangeNotification();
        }


    }
}