using System.Reflection;
using Caliburn.PresentationFramework.Screens;
using Rhino.Licensing.AdminTool.Extensions;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class AboutViewModel : Screen
    {
        public virtual string Version
        {
            get { return typeof (AboutViewModel).Assembly.GetName().Version.ToString(); }
        }

        public virtual string Copyright
        {
            get { return typeof (AboutViewModel).Assembly.GetAttribute<AssemblyCopyrightAttribute>().Copyright; }
        }
    }
}