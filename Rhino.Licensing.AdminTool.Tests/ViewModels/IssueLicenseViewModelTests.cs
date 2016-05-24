using Rhino.Licensing.AdminTool.ViewModels;
using Xunit;

namespace Rhino.Licensing.AdminTool.Tests.ViewModels
{
    public class IssueLicenseViewModelTests
    {
        [Fact]
        public void Notifies_All_Screen_When_Selected_License_Changes()
        {
            var userData = new UserDataViewModel() as IUserDataViewModel;
            var licenseInfo = new LicenseInfoViewModel() as ILicenseInfoViewModel;

            var vm = new IssueLicenseViewModel(userData, licenseInfo);

            Assert.Same(vm.CurrentLicense, userData.CurrentLicense);
            Assert.Same(vm.CurrentLicense, licenseInfo.CurrentLicense);
        }

        [Fact]
        public void Closing_ViewModel_Calls_TryClose_With_True_DialogResult()
        {
            var userData = new UserDataViewModel() as IUserDataViewModel;
            var licenseInfo = new LicenseInfoViewModel() as ILicenseInfoViewModel;

            var vm = new TestableIssueLicenseViewModel(userData, licenseInfo);
            vm.Accept();

            Assert.NotNull(vm.SelectedDialogResult);
            Assert.True(vm.SelectedDialogResult.Value);
        }

        [Fact]
        public void Can_Accept_New_License_When_OwnerName_Is_Filled()
        {
            var userData = new UserDataViewModel() as IUserDataViewModel;
            var licenseInfo = new LicenseInfoViewModel() as ILicenseInfoViewModel;

            var vm = new TestableIssueLicenseViewModel(userData, licenseInfo);

            var defaultCanAccept = vm.CanAccept;
            vm.CurrentLicense.OwnerName = "John Doe";
            var canAccept = vm.CanAccept;

            Assert.False(defaultCanAccept);
            Assert.True(canAccept);
        }

        private class TestableIssueLicenseViewModel : IssueLicenseViewModel
        {
            public TestableIssueLicenseViewModel(IUserDataViewModel userDataViewModel, ILicenseInfoViewModel licenseInfoViewModel) 
                : base(userDataViewModel, licenseInfoViewModel)
            {
            }

            public bool? SelectedDialogResult { get; set; }

            public override void TryClose(bool? dialogResult)
            {
                SelectedDialogResult = dialogResult;
            }
        }
    }
}