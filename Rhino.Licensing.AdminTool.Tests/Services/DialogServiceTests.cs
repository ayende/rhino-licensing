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
        [Fact(Skip = "Not Completed")]
        public void Can_Show_OpenFileDialog()
        {
            var model = CreateOpenFileDialogModel();
            var factory = MockRepository.GenerateMock<IDialogFactory>();
            var dialog = MockRepository.GenerateMock<OpenFileDialog>(model);

            factory.Expect(x => x.Create<OpenFileDialog>(Arg.Is(model))).Return(dialog);
            dialog.Expect(x => x.ShowDialog()).Return(DialogResult.OK);
            dialog.Expect(x => x.ViewModel).Return(model);

            new DialogService(factory).ShowOpenFileDialog(model);

            dialog.AssertWasCalled(x => x.ShowDialog(), x => x.Repeat.Once());
        }

        [Fact(Skip = "Not Completed")]
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