using System.Windows;
using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Screens;
using Caliburn.PresentationFramework.ViewModels;
using Caliburn.PresentationFramework.Views;

namespace Rhino.Licensing.AdminTool.Infrastructure
{
    public class WindowManager : DefaultWindowManager
    {
        public WindowManager(IViewLocator viewLocator, IViewModelBinder viewModelBinder) 
            : base(viewLocator, viewModelBinder)
        {
        }

        public override bool? ShowDialog(object rootModel, object context)
        {
            var window = base.CreateWindow(rootModel, true, context);
            
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.ResizeMode = ResizeMode.NoResize;
            window.Title = ((IScreen)rootModel).DisplayName;

            return window.ShowDialog();
        }
    }
}