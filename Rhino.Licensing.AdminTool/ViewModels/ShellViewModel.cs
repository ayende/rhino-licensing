using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Screens;
using Rhino.Licensing.AdminTool.Factories;
using IViewModelFactory = Caliburn.PresentationFramework.ViewModels.IViewModelFactory;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class ShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IProjectFactory _projectFactory;
        private readonly IWindowManager _windowManager;

        public ShellViewModel(
            IWindowManager windowManager,
            IViewModelFactory viewModelFactory, 
            IProjectFactory projectFactory)
        {
            DisplayName = "Rhino.Licensing.AdminTool";

            _windowManager = windowManager;
            _viewModelFactory = viewModelFactory;
            _projectFactory = projectFactory;
        }

        protected override void  ChangeActiveItem(Screen newItem, bool closePrevious)
        {
            base.ChangeActiveItem(newItem, closePrevious);
        }

        public virtual void ShowAboutDialog()
        {
            _windowManager.ShowDialog(_viewModelFactory.Create<AboutViewModel>());
        }

        public virtual void CreateNewProject()
        {
            var vm = _viewModelFactory.Create<ProjectViewModel>();
            var project = _projectFactory.Create();

            vm.CurrentProject = project;
            ActiveItem = vm;
        }
    }
}