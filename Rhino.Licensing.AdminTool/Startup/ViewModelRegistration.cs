using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Rhino.Licensing.AdminTool.Extensions;
using Rhino.Licensing.AdminTool.ViewModels;

namespace Rhino.Licensing.AdminTool.Startup
{
    public class ViewModelRegistration : IRegistration
    {
        public virtual void Register(IKernel kernel)
        {
            kernel.Register(AllTypes.FromAssemblyContaining<ViewModelRegistration>()
                                    .Where(t => t.Namespace == "Rhino.Licensing.AdminTool.ViewModels")
                                    .WithService.FirstInterfaceOnClass()
                                    .Configure(c => c.LifeStyle.Transient)
                                    .ConfigureFor<IShellViewModel>(c => c.LifeStyle.Singleton));
        }
    }
}