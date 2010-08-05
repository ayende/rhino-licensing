using Caliburn.PresentationFramework.Screens;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class ShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        public ShellViewModel()
        {
            DisplayName = "Rhino.Licensing.AdminTool";
        }
    }
}