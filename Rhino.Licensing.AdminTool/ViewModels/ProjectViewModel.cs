using Caliburn.PresentationFramework.Screens;
using Rhino.Licensing.AdminTool.Model;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class ProjectViewModel : Screen
    {
        private Project _project;

        public virtual Project CurrentProject
        {
            get { return _project; }
            set
            {
                _project = value;
                NotifyOfPropertyChange(() => CurrentProject);
            }
        }
    }
}