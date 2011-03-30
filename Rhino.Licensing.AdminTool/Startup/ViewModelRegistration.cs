using System;
using System.Collections.Generic;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Rhino.Licensing.AdminTool.Extensions;
using Rhino.Licensing.AdminTool.Services;
using Rhino.Licensing.AdminTool.ViewModels;
using System.Linq;

namespace Rhino.Licensing.AdminTool.Startup
{
    public class ViewModelRegistration : IRegistration
    {
        public virtual void Register(IKernel kernel)
        {
            kernel.Register(AllTypes.FromAssemblyContaining<ViewModelRegistration>()
                                    .Where(t => ViewModelNamespaces.Contains(t.Namespace) &&
                                                !SkipAutoRegistration.Contains(t))
                                    .WithService.FirstInterfaceOnClass()
                                    .Configure(c => c.LifeStyle.Transient));

            kernel.Register(Component.For<IShellViewModel>()
                                     .ImplementedBy<ShellViewModel>()
                                     .Forward<IStatusService>()
                                     .LifeStyle.Singleton);
        }

        private static IEnumerable<string> ViewModelNamespaces
        {
            get
            {
                yield return "Rhino.Licensing.AdminTool.ViewModels";
                yield return "Rhino.Licensing.AdminTool.Dialogs";
            }
        }

        private static IEnumerable<Type> SkipAutoRegistration
        {
            get
            {
                yield return typeof (ShellViewModel);
            }
        }
    }
}