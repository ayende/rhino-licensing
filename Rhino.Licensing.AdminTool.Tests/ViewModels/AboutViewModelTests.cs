using System.Reflection;
using Caliburn.Testability;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Licensing.AdminTool.Views;
using Xunit;
using Rhino.Licensing.AdminTool.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.ViewModels
{
    public class AboutViewModelTests
    {
        [Fact]
        public void Version_Is_Bound()
        {
            var validator = Validator.For<AboutView, AboutViewModel>()
                                     .Validate();

            validator.AssertWasBound(x => x.Version);
            validator.AssertWasBound(x => x.Copyright);
        }

        [Fact]
        public void Version_Property_Returns_Value_From_AssemblyInfo()
        {
            var vm = new AboutViewModel();
            var versionInfo = typeof (AboutViewModel).Assembly.GetName();

            Assert.Equal(versionInfo.Version.ToString(), vm.Version);
        }

        [Fact]
        public void Copyright_Property_Returns_Value_From_AssemblyInfo()
        {
            var vm = new AboutViewModel();
            var copyrightAttrib = typeof (AboutViewModel).Assembly.GetAttribute<AssemblyCopyrightAttribute>();

            Assert.NotNull(vm.Copyright);
            Assert.NotEmpty(vm.Copyright);
            Assert.Equal(copyrightAttrib.Copyright, vm.Copyright);
        }
    }
}