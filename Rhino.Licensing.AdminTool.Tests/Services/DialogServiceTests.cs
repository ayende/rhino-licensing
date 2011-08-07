using Rhino.Licensing.AdminTool.Factories;
using Rhino.Licensing.AdminTool.Services;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Mocks;
using Xunit;
using OpenFileDialog = Rhino.Licensing.AdminTool.Dialogs.OpenFileDialog;
using SaveFileDialog = Rhino.Licensing.AdminTool.Dialogs.SaveFileDialog;

namespace Rhino.Licensing.AdminTool.Tests.Services
{
    public class DialogServiceTests
    {
        [Fact]
        public void Can_Show_OpenFileDialog()
        {
            var model = CreateOpenFileDialogModel(true);
            var factory = MockRepository.GenerateMock<IDialogFactory>();
            var dialog = MockRepository.GenerateMock<OpenFileDialog>();

            factory.Expect(x => x.Create<OpenFileDialog, IOpenFileDialogViewModel>(Arg.Is(model))).Return(dialog);
            dialog.Expect(x => x.ViewModel).Return(model);

            new DialogService(factory).ShowOpenFileDialog(model);

            dialog.AssertWasCalled(x => x.ShowDialog(), x => x.Repeat.Once());
        }

        [Fact]
        public void Can_Show_SaveFileDialog()
        {
            var model = CreateSaveFileDialogModel(true);
            var factory = MockRepository.GenerateMock<IDialogFactory>();
            var dialog = MockRepository.GenerateMock<SaveFileDialog>();

            var service = new DialogService(factory) as IDialogService;

            factory.Expect(x => x.Create<SaveFileDialog, ISaveFileDialogViewModel>(Arg.Is(model))).Return(dialog);

            service.ShowSaveFileDialog(model);

            dialog.AssertWasCalled(x => x.ShowDialog(), x => x.Repeat.Once());
        }

        private IOpenFileDialogViewModel CreateOpenFileDialogModel(bool? result)
        {
            return new OpenFileDialogViewModel
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExtension = "rlic",
                Filter = "Rhino License Project|*.rlic",
                InitialDirectory = "C:\\",
                MultiSelect = false,
                Title = "Open File Dialog",
                Result = result
            };
        }

        private ISaveFileDialogViewModel CreateSaveFileDialogModel(bool? result)
        {
            return new SaveFileDialogViewModel
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExtension = "rlic",
                Filter = "Rhino License Project|*.rlic",
                InitialDirectory = "C:\\",
                Title = "Open File Dialog",
                Result = result
            };
        }

    }
}