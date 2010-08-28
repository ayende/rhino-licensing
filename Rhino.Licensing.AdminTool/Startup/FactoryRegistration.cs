using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Rhino.Licensing.AdminTool.Extensions;
using Rhino.Licensing.AdminTool.Factories;
using Castle.Facilities.TypedFactory;

namespace Rhino.Licensing.AdminTool.Startup
{
    public class FactoryRegistration : IRegistration
    {
        public virtual void Register(IKernel kernel)
        {
            kernel.AddFacility<TypedFactoryFacility>();

            kernel.Register(Component.For<IViewModelFactory>().AsFactory());

            kernel.Register(Component.For<IDialogFactory>().AsFactory());

            kernel.Register(AllTypes.FromAssemblyContaining<ViewModelRegistration>()
                                    .Where(t => t.Namespace == "Rhino.Licensing.AdminTool.Factories")
                                    .WithService.FirstInterfaceOnClass());
        }
    }
}