using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Screens;
using Caliburn.Testability;
using Rhino.Licensing.AdminTool.Factories;
using Rhino.Licensing.AdminTool.Services;
using Rhino.Licensing.AdminTool.Tests.Base;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Licensing.AdminTool.Views;
using Rhino.Mocks;
using Xunit;

namespace Rhino.Licensing.AdminTool.Tests.ViewModels
{
    public class ShellViewModelTests : TestBase
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IProjectService _projectService;
        private readonly IDialogService _dialogService;

        public ShellViewModelTests()
        {
            _viewModelFactory = MockRepository.GenerateMock<IViewModelFactory>();
            _windowManager = MockRepository.GenerateMock<IWindowManager>();
            _projectService = MockRepository.GenerateMock<IProjectService>();
            _dialogService = MockRepository.GenerateMock<IDialogService>();
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
            var vm = CreateProjectViewModel();

            _viewModelFactory.Expect(x => x.Create<ProjectViewModel>()).Return(vm);

            shell.CreateNewProject();

            Assert.NotNull(shell.ActiveItem);
            Assert.Same(vm, shell.ActiveItem);
        }

        [Fact]
        public void OpenProject_Opens_ProjectViewModel()
        {
            var shell = CreateShellViewModel();
            var projectViewModel = MockRepository.GenerateMock<ProjectViewModel>(_projectService, _dialogService);
            
            _viewModelFactory.Expect(x => x.Create<ProjectViewModel>()).Return(projectViewModel);

            shell.OpenProject();

            Assert.NotNull(shell.ActiveItem);
            Assert.Same(projectViewModel, shell.ActiveItem);
        }

        [Fact]
        public void OpenProject_Calls_Open_On_ProjectViewModel()
        {
            var shell = CreateShellViewModel();
            var projectViewModel = MockRepository.GenerateMock<ProjectViewModel>(_projectService, _dialogService);

            _viewModelFactory.Expect(x => x.Create<ProjectViewModel>()).Return(projectViewModel);

            shell.OpenProject();

            projectViewModel.AssertWasCalled(x => x.Open(), options => options.Repeat.Once());
        }

        [Fact]
        public void Opening_New_Screen_Will_Destroy_Previous()
        {
            var shell = CreateShellViewModel();
            var vm = CreateProjectViewModel();

            shell.ActiveItem = vm; //Set initial view

            shell.ActiveItem = new Screen(); //Change to new view

            _viewModelFactory.AssertWasCalled(x => x.Release(Arg.Is(vm)));
        }

        private ProjectViewModel CreateProjectViewModel()
        {
            return new ProjectViewModel(_projectService, _dialogService);
        }

        private ShellViewModel CreateShellViewModel()
        {
            return new ShellViewModel(_windowManager, _viewModelFactory, _projectService);
        }
    }
}