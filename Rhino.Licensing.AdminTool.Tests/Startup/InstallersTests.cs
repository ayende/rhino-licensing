using Castle.Windsor;
using Rhino.Licensing.AdminTool.Startup;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace Rhino.Licensing.AdminTool.Tests.Startup
{
    public class InstallersTests
    {
        private readonly IWindsorContainer _container;
        private readonly GuyWire _guyWire;

        public InstallersTests()
        {
            _container = new WindsorContainer();

            _guyWire = MockRepository.GenerateMock<GuyWire>(_container);
            _guyWire.Expect(g => g.Container).Return(_container);
            _guyWire.Expect(g => g.Wire()).CallOriginalMethod(OriginalCallOptions.CreateExpectation);
        }

        [Fact]
        public void GuyWire_Delegates_To_FactoryInstaller()
        {
            var installer = new FactoryRegistration();

            _guyWire.Expect(g => g.ComponentsInfo).Return(new[] { installer });
            
            Assert.DoesNotThrow(() => _guyWire.Wire());
        }

        [Fact]
        public void GuyWire_Delegates_To_ServiceInstaller()
        {
            var installer = new ServiceRegistration();

            _guyWire.Expect(g => g.ComponentsInfo).Return(new[] { installer });

            Assert.DoesNotThrow(() => _guyWire.Wire());
        }

        [Fact]
        public void GuyWire_Delegates_To_ViewModelInstaller()
        {
            var installer = new ViewModelRegistration();

            _guyWire.Expect(g => g.ComponentsInfo).Return(new[] { installer });

            Assert.DoesNotThrow(() => _guyWire.Wire());
        }

        [Fact]
        public void GuyWire_Delegates_To_ViewInstaller()
        {
            var installer = new ViewRegistration();

            _guyWire.Expect(g => g.ComponentsInfo).Return(new[] { installer });

            Assert.DoesNotThrow(() => _guyWire.Wire());
        }
    }
}