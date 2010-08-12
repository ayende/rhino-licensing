using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.Testability;
using Rhino.Licensing.AdminTool.Factories;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Licensing.AdminTool.Views;
using Rhino.Mocks;
using Xunit;
using IViewModelFactory = Caliburn.PresentationFramework.ViewModels.IViewModelFactory;

namespace Rhino.Licensing.AdminTool.Tests.ViewModels
{
    public class ShellViewModelTests
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IProjectFactory _projectFactory;

        public ShellViewModelTests()
        {
            _viewModelFactory = MockRepository.GenerateMock<IViewModelFactory>();
            _windowManager = MockRepository.GenerateMock<IWindowManager>();
            _projectFactory = MockRepository.GenerateMock<IProjectFactory>();
        }

        [Fact]
        public void Shell_Has_Correct_Binding()
        {
            var validator = Validator.For<ShellView, ShellViewModel>()
                                     .Validate();

            validator.AssertNoErrors();
            validator.AssertWasBound(x => x.DisplayName);
        }

        [Fact]
        public void ShowAbout_Action_Displays_Dialog()
        {
            var shell = CreateShellViewModel();
            var aboutVm = new AboutViewModel();

            _viewModelFactory.Expect(x => x.Create<AboutViewModel>()).Return(aboutVm);

            shell.ShowAboutDialog();

            _windowManager.AssertWasCalled(x => x.ShowDialog(Arg.Is(aboutVm), Arg.Is<object>(null)), c => c.Repeat.Once());
        }

        [Fact]
        public void CreateNewProject_Opens_ProjectViewModel()
        {

            var shell = CreateShellViewModel();
            var vm = new ProjectViewModel();

            _viewModelFactory.Expect(x => x.Create<ProjectViewModel>()).Return(vm);

            shell.CreateNewProject();

            Assert.NotNull(shell.ActiveItem);
            Assert.Equal(vm, shell.ActiveItem);
        }

        private ShellViewModel CreateShellViewModel()
        {
            return new ShellViewModel(_windowManager, _viewModelFactory, _projectFactory);
        }
    }
}