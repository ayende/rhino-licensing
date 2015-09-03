using System.Reflection;
using System.Windows;
using Caliburn.Core;
using Caliburn.Core.Configuration;
using Caliburn.PresentationFramework;
using Caliburn.Windsor;
using Rhino.Licensing.AdminTool.Infrastructure;
using Rhino.Licensing.AdminTool.Startup;

namespace Rhino.Licensing.AdminTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private GuyWire guyWire;
        private WindsorAdapter locatorAdapter;

        public App()
        {
            CreateGuyWire();
            CreateContainer();
            ConfigureFramework();
        }

        protected void CreateContainer()
        {
            locatorAdapter = new WindsorAdapter(guyWire.Container);
        }

        protected void ConfigureFramework()
        {
            CaliburnFramework.Configure(locatorAdapter)
                .With.Core()
                .With.Assemblies(SelectAssemblies())
                .With.PresentationFramework()
                .Using(x => x.WindowManager<WindowManager>())
                .Start();
        }

        protected void CreateGuyWire()
        {
            guyWire = new GuyWire();
            guyWire.Wire();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            guyWire.ShowRootModel();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            guyWire.Dewire();

            base.OnExit(e);
        }

        protected virtual Assembly[] SelectAssemblies()
        {
            return new[] { Assembly.GetEntryAssembly() };
        }
    }
}
