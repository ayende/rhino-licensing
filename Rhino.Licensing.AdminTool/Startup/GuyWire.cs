using System.Collections.Generic;
using Caliburn.PresentationFramework.ApplicationModel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Rhino.Licensing.AdminTool.ViewModels;

namespace Rhino.Licensing.AdminTool.Startup
{
    public class GuyWire
    {
        public GuyWire(IWindsorContainer container)
        {
            Container = container;
        }

        public GuyWire() : this(CreateDefaultContainer())
        {
        }

        private static IWindsorContainer CreateDefaultContainer()
        {
            return new WindsorContainer();
        }

        public virtual IWindsorContainer Container
        {
            get; private set;
        }

        /// <summary>
        /// Component information to be registered 
        /// in the container
        /// </summary>
        private IEnumerable<IRegistration> ComponentsInfo
        {
            get
            {
                yield return new ViewRegistration();
                yield return new ViewModelRegistration();
            }
        }

        /// <summary>
        /// Wires the components to the container
        /// </summary>
        public virtual void Wire()
        {
            foreach (var reg in ComponentsInfo)
            {
                reg.Register(Container.Kernel);
            }
        }

        /// <summary>
        /// Dewires the components and disposes the container.
        /// </summary>
        public virtual void Dewire()
        {
            if(Container != null)
            {
                Container.Dispose();
                Container = null;
            }
        }

        /// <summary>
        /// Shows the root view model
        /// </summary>
        public void ShowRootModel()
        {
            var root = Container.Resolve<ShellViewModel>();
            var windowManager = Container.Resolve<IWindowManager>();

            windowManager.Show(root, null);
        }
    }
}