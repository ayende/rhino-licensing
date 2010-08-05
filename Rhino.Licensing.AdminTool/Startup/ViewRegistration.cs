using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Rhino.Licensing.AdminTool.Extensions;
using Rhino.Licensing.AdminTool.Views;

namespace Rhino.Licensing.AdminTool.Startup
{
    public class ViewRegistration : IRegistration
    {
        public void Register(IKernel kernel)
        {
            kernel.Register(AllTypes.FromAssemblyContaining<ViewRegistration>()
                                    .Where(t => t.Namespace.EndsWith("Rhino.Licensing.AdminTool.Views"))
                                    .WithService.FirstInterfaceOnClass()
                                    .Configure(c => c.LifeStyle.Transient)
                                    .ConfigureFor<ShellView>(c => c.LifeStyle.Singleton));
        }
    }
}