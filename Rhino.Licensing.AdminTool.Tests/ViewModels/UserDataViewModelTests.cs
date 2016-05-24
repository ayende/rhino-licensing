using Rhino.Licensing.AdminTool.Model;
using Rhino.Licensing.AdminTool.ViewModels;
using Xunit;
using Caliburn.Testability.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.ViewModels
{
    public class UserDataViewModelTests
    {
        [Fact]
        public void Fires_PropertyChange_Notification()
        {
            var viewModel = new UserDataViewModel();

            viewModel.AssertThatProperty(x => x.SelectedKeyValue).RaisesChangeNotification();
            viewModel.AssertThatProperty(x => x.CurrentLicense).RaisesChangeNotification();
        }

        [Fact]
        public void Can_Remove_When_There_Is_A_Valid_Selection()
        {
            var viewModel = new UserDataViewModel();
            var defaultCanRemove = viewModel.CanRemoveSelected;
            
            viewModel.CurrentLicense = new License();
            viewModel.SelectedKeyValue = new UserData();

            var canRemove = viewModel.CanRemoveSelected;

            Assert.False(defaultCanRemove);
            Assert.True(canRemove);
        }

        [Fact]
        public void RemoveSelected_Removes_UserData_From_License()
        {
            var viewModel = new UserDataViewModel();
            var license = new License();
            var data = new UserData();

            license.Data.Add(data);
            viewModel.CurrentLicense = license;
            viewModel.SelectedKeyValue = data;

            viewModel.RemoveSelected();

            Assert.DoesNotContain(data, viewModel.CurrentLicense.Data);
        }

        [Fact]
        public void Can_AddKey_When_There_Is_A_Valid_Selection()
        {
            var viewModel = new UserDataViewModel();
            var defaultCanAdd = viewModel.CanAddKey;

            viewModel.CurrentLicense = new License();
            viewModel.CurrentKey = "UserKey";
            viewModel.CurrentValue = "UserValue";

            var canAdd = viewModel.CanAddKey;

            Assert.False(defaultCanAdd);
            Assert.True(canAdd);
        }

        [Fact]
        public void AddKey_Adds_To_Licese_UserData()
        {
            var viewModel = new UserDataViewModel();
            var license = new License();

            viewModel.CurrentLicense = license;
            viewModel.CurrentKey = "UserKey";
            viewModel.CurrentValue = "UserValue";
            
            viewModel.AddKey();

            var data = viewModel.CurrentLicense.Data[0];

            Assert.Equal("UserKey", data.Key);
            Assert.Equal("UserValue", data.Value);
        }
    }
}