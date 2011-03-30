using System;
using System.Collections.Generic;
using Caliburn.PresentationFramework.ApplicationModel;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Rhino.Licensing.AdminTool.Startup;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;
using System.Linq;

namespace Rhino.Licensing.AdminTool.Tests.Startup
{
    public class GuyWireTests
    {
        [Fact]
        public void GuyWire_Has_Default_Container()
        {
            var guyWire = new GuyWire();

            Assert.NotNull(guyWire.Container);
        }

        [Fact]
        public void GuyWire_Registers_Correct_Components()
        {
            var guyWire = new GuyWire();
            var count = guyWire.ComponentsInfo.Count();

            Assert.Equal(4, count);
        }

        [Fact]
        public void GuyWire_Registers_ComponentInfo()
        {
            var guyWire = new GuyWireStub();

            guyWire.Wire();

            var components = guyWire.GetComponents();

            Assert.True(components.OfType<StubRegistration>().All(c => c.IsRegistered));
        }


        [Fact]
        public void GuyWire_Disposes_Container_Upon_Dewire()
        {
            var container = new WindsorContainer();
            var guyWire = new GuyWireStub(container);

            guyWire.Wire();
            guyWire.Dewire();

            Assert.Null(guyWire.Container);
        }

        [Fact]
        public void Can_Show_Root_ViewModel()
        {
            var winManager = MockRepository.GenerateMock<IWindowManager>();
            var shell = MockRepository.GenerateMock<IShellViewModel>();
            var container = new WindsorContainer();
            var guyWire = new GuyWire(container);

            container.Kernel.AddComponentInstance<IWindowManager>(winManager);
            container.Kernel.AddComponentInstance<IShellViewModel>(shell);

            guyWire.ShowRootModel();

            winManager.AssertWasCalled(x => x.Show(Arg.Is(shell), Arg.Is<object>(null)));
        }

        public class GuyWireStub : GuyWire
        {
            private IEnumerable<IRegistration> _components;

            public GuyWireStub() : this(new WindsorContainer())
            {
            }

            public GuyWireStub(IWindsorContainer container) : base(container)
            {
                _components = new[] { new StubRegistration(), new StubRegistration() };
            }

            public override IEnumerable<IRegistration> ComponentsInfo
            {
                get { return _components; }
            }

            public IEnumerable<IRegistration> GetComponents()
            {
                return _components;
            }
        }

        public class StubRegistration : IRegistration
        {
            public bool IsRegistered;

            public void Register(IKernel kernel)
            {
                IsRegistered = true;
            }
        }
    }
}