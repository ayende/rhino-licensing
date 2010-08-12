using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Rhino.Licensing.AdminTool.Extensions;
using Rhino.Licensing.AdminTool.Factories;
using Rhino.Licensing.AdminTool.ViewModels;

namespace Rhino.Licensing.AdminTool.Startup
{
    public class ServiceRegistration : IRegistration
    {
        public void Register(IKernel kernel)
        {
            kernel.Register(AllTypes.FromAssemblyContaining<ServiceRegistration>()
                                    .Where(t => t.Namespace == "Rhino.Licensing.AdminTool.Services")
                                    .WithService.FirstInterfaceOnClass()
                                    .Configure(c => c.LifeStyle.Transient));
        }
    }
}