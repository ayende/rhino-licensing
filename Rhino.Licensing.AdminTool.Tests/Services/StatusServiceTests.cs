using System;
using Caliburn.PresentationFramework.ApplicationModel;
using Rhino.Licensing.AdminTool.Factories;
using Rhino.Licensing.AdminTool.Services;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Mocks;
using Xunit;

namespace Rhino.Licensing.AdminTool.Tests.Services
{
    public class StatusServiceTests
    {
        private readonly IStatusService _statusService;

        public StatusServiceTests()
        {
            var windowManager = MockRepository.GenerateMock<IWindowManager>();
            var viewModelFactory = MockRepository.GenerateMock<IViewModelFactory>();
            var projectService = MockRepository.GenerateMock<IProjectService>();

            _statusService = new ShellViewModel(windowManager, viewModelFactory, projectService);
        }

        [Fact]
        public void Can_Update_Status_Message()
        {
            _statusService.Update("Issued Licenses: {0}", 10);

            var vm = _statusService as IShellViewModel;

            Assert.Equal("Issued Licenses: 10", vm.StatusMessage);
        }
    }
}