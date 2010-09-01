using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Screens;
using Rhino.Licensing.AdminTool.Factories;
using Rhino.Licensing.AdminTool.Services;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public interface IShellViewModel : IConductor
    {
        /// <summary>
        /// Shows about dialog
        /// </summary>
        void ShowAboutDialog();

        /// <summary>
        /// Shows Project view model and 
        /// create a new project
        /// </summary>
        void CreateNewProject();
    }

    public class ShellViewModel : Conductor<Screen>, IShellViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IProjectService _projectService;
        private readonly IWindowManager _windowManager;

        public ShellViewModel(
            IWindowManager windowManager,
            IViewModelFactory viewModelFactory, 
            IProjectService projectService)
        {
            DisplayName = "Rhino.Licensing.AdminTool";

            _windowManager = windowManager;
            _viewModelFactory = viewModelFactory;
            _projectService = projectService;
        }

        protected override void  ChangeActiveItem(Screen newItem, bool closePrevious)
        {
            var oldViewModel = ActiveItem;
            if(oldViewModel != null)
            {
                _viewModelFactory.Release(oldViewModel);
            }

            base.ChangeActiveItem(newItem, closePrevious);
        }

        public virtual void ShowAboutDialog()
        {
            _windowManager.ShowDialog(_viewModelFactory.Create<AboutViewModel>());
        }

        public virtual void CreateNewProject()
        {
            var vm = _viewModelFactory.Create<ProjectViewModel>();
            var project = _projectService.Create();

            vm.CurrentProject = project;

            ActiveItem = vm;
        }

        public virtual void OpenProject()
        {
            var vm = _viewModelFactory.Create<ProjectViewModel>();

            vm.Open();

            ActiveItem = vm;
        }
    }
}