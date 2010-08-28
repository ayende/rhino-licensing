using System.Windows.Forms;
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
            var model = CreateOpenFileDialogModel();
            var factory = MockRepository.GenerateMock<IDialogFactory>();
            var dialog = MockRepository.GenerateMock<OpenFileDialog>(model);

            factory.Expect(x => x.Create<OpenFileDialog>(Arg.Is(model))).Return(dialog);
            dialog.Expect(x => x.ViewModel).Return(model);
            dialog.Expect(x => x.ShowDialog()).Return(DialogResult.OK);

            new DialogService(factory).ShowOpenFileDialog(model);

            dialog.AssertWasCalled(x => x.ShowDialog(), x => x.Repeat.Once());
        }

        [Fact]
        public void Can_Show_SaveFileDialog()
        {
            var model = CreateSaveFileDialogModel();
            var factory = MockRepository.GenerateMock<IDialogFactory>();
            var dialog = MockRepository.GenerateMock<SaveFileDialog>(model);

            var service = new DialogService(factory) as IDialogService;

            factory.Expect(x => x.Create<SaveFileDialog>(Arg.Is(model))).Return(dialog);

            service.ShowSaveFileDialog(model);

            dialog.AssertWasCalled(x => x.ShowDialog(), x => x.Repeat.Once());
        }

        [Fact]
        public void Returning_OK_As_DialogResult_Translates_To_True()
        {
            var model = CreateOpenFileDialogModel();
            var factory = MockRepository.GenerateMock<IDialogFactory>();
            var dialog = MockRepository.GenerateMock<OpenFileDialog>(model);

            factory.Expect(x => x.Create<OpenFileDialog>(Arg.Is(model))).Return(dialog);
            dialog.Expect(x => x.ViewModel).Return(model);
            dialog.Expect(x => x.ShowDialog()).Return(DialogResult.OK);

            var result = new DialogService(factory).ShowOpenFileDialog(model);

            Assert.NotNull(result);
            Assert.True(result.Value);
        }

        [Fact]
        public void Returning_Cancel_As_DialogResult_Translates_To_False()
        {
            var model = CreateOpenFileDialogModel();
            var factory = MockRepository.GenerateMock<IDialogFactory>();
            var dialog = MockRepository.GenerateMock<OpenFileDialog>(model);

            factory.Expect(x => x.Create<OpenFileDialog>(Arg.Is(model))).Return(dialog);
            dialog.Expect(x => x.ViewModel).Return(model);
            dialog.Expect(x => x.ShowDialog()).Return(DialogResult.Cancel);

            var result = new DialogService(factory).ShowOpenFileDialog(model);

            Assert.NotNull(result);
            Assert.False(result.Value);
        }

        [Fact]
        public void Returning_Anything_Else_From_DialogResult_Translates_To_Null()
        {
            var model = CreateOpenFileDialogModel();
            var factory = MockRepository.GenerateMock<IDialogFactory>();
            var dialog = MockRepository.GenerateMock<OpenFileDialog>(model);

            factory.Expect(x => x.Create<OpenFileDialog>(Arg.Is(model))).Return(dialog);
            dialog.Expect(x => x.ViewModel).Return(model);
            dialog.Expect(x => x.ShowDialog()).Return(DialogResult.Abort);

            var result = new DialogService(factory).ShowOpenFileDialog(model);

            Assert.Null(result);
        }

        private IOpenFileDialogViewModel CreateOpenFileDialogModel()
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
                Title = "Open File Dialog"
            };
        }

        private ISaveFileDialogViewModel CreateSaveFileDialogModel()
        {
            return new SaveFileDialogViewModel
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExtension = "rlic",
                Filter = "Rhino License Project|*.rlic",
                InitialDirectory = "C:\\",
                Title = "Open File Dialog"
            };
        }

    }
}