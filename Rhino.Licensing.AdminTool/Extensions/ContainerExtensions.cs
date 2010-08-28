using System.Linq;
using Castle.MicroKernel.Registration;

namespace Rhino.Licensing.AdminTool.Extensions
{
    public static class ContainerExtensions
    {
        public static BasedOnDescriptor FirstInterfaceOnClass(this ServiceDescriptor serviceDescriptor)
        {
            return serviceDescriptor.Select((t, bt) =>
            {
                var baseInterfaces = t.BaseType.GetInterfaces();
                var interfaces = t.GetInterfaces().Except(baseInterfaces);

                return interfaces.Count() != 0 ? new[] { interfaces.First() } : null;
            });
        }
    }
}