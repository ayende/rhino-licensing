using System;
using System.Collections.Generic;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Xunit;
using Rhino.Licensing.AdminTool.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.Startup
{
    public class ContainerTests
    {
        [Fact]
        public void Registering_With_FirstInterfaceOnClass()
        {
            var container = new WindsorContainer();

            container.Register(AllTypes.Pick(TypesToRegister)
                                       .Where(t => t != null /*Any Predicate*/)
                                       .WithService.FirstInterfaceOnClass());

            var component = container.Resolve<IFirst>();
            
            Assert.NotNull(component);
            Assert.Throws<ComponentNotFoundException>(() => container.Resolve<ISecond>());
        }

        private IEnumerable<Type> TypesToRegister
        {
            get { return new[] {typeof (ComponentToRegister)}; }
        }

        private interface IFirst
        {
        }

        private interface ISecond
        {
        }

        private class ComponentToRegister : IFirst, ISecond
        {
        }
    }
}