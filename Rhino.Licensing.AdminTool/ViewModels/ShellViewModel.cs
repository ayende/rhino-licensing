using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Screens;
using Caliburn.PresentationFramework.ViewModels;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class ShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        public ShellViewModel()
        {
            DisplayName = "Rhino.Licensing.AdminTool";
        }

        public virtual IWindowManager WindowManager
        {
            get; set;
        }

        public virtual IViewModelFactory Factory
        { 
            get; set;
        }

        protected override void  ChangeActiveItem(Screen newItem, bool closePrevious)
        {
            base.ChangeActiveItem(newItem, closePrevious);
        }

        public virtual void ShowAboutDialog()
        {
            var vm = Factory.Create<AboutViewModel>();
            WindowManager.ShowDialog(vm);
        }

        public virtual void CreateNewProject()
        {
            var vm = Factory.Create<ProjectViewModel>();
            ActiveItem = vm;
        }
    }
}