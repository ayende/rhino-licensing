using Rhino.Licensing.AdminTool.Dialogs;
using Rhino.Licensing.AdminTool.ViewModels;
using Xunit;

namespace Rhino.Licensing.AdminTool.Tests.Dialogs
{
    public class SaveFileDialogTests
    {
        [Fact]
        public void Dialog_Wires_Model_Properties()
        {
            var model = CreateSaveFileDialogModel();
            var dialog = new SaveFileDialogStub(model);
            var dialogForm = dialog.GetCoreDialog();

            Assert.True(dialogForm.AddExtension);
            Assert.True(dialogForm.CheckFileExists);
            Assert.True(dialogForm.CheckPathExists);
            Assert.True(dialogForm.OverwritePrompt);
            Assert.True(dialogForm.SupportMultiDottedExtensions);
            Assert.Equal("rlic", dialogForm.DefaultExt);
            Assert.Equal("Rhino License Project|*.rlic", dialogForm.Filter);
            Assert.Equal("C:\\", dialogForm.InitialDirectory);
            Assert.Equal("Save File Dialog", dialogForm.Title);
        }

        [Fact]
        public void OpenFileDialog_ViewModel_Property()
        {
            var model = CreateSaveFileDialogModel();
            var dialog = new SaveFileDialogStub(model);

            Assert.NotNull(dialog.ViewModel);
            Assert.Same(model, dialog.ViewModel);
        }

        private ISaveFileDialogViewModel CreateSaveFileDialogModel()
        {
            var model = new SaveFileDialogViewModel
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                OverwritePrompt = true,
                SupportMultiDottedExtensions = true,
                DefaultExtension = "rlic",
                Filter = "Rhino License Project|*.rlic",
                InitialDirectory = "C:\\",
                Title = "Save File Dialog",
            };

            return model;
        }

        public class SaveFileDialogStub : SaveFileDialog
        {
            public SaveFileDialogStub(ISaveFileDialogViewModel viewModel)
                : base(viewModel)
            {
            }

            public System.Windows.Forms.SaveFileDialog GetCoreDialog()
            {
                return base.Dialog as System.Windows.Forms.SaveFileDialog;
            }
        }
    }
}