using Rhino.Licensing.AdminTool.Dialogs;
using Rhino.Licensing.AdminTool.ViewModels;
using Xunit;

namespace Rhino.Licensing.AdminTool.Tests.Dialogs
{
    public class OpenFileDialogTests
    {
        [Fact]
        public void Dialog_Wires_Model_Properties()
        {
            var model = CreateOpenFileDialogModel();
            var dialog = new OpenFileDialogStub(model);
            var dialogForm = dialog.GetCoreDialog();

            Assert.True(dialogForm.AddExtension);
            Assert.True(dialogForm.CheckFileExists);
            Assert.True(dialogForm.CheckPathExists);
            Assert.False(dialogForm.Multiselect);
            Assert.Equal("rlic", dialogForm.DefaultExt);
            Assert.Equal("Rhino License Project|*.rlic", dialogForm.Filter);
            Assert.Equal("C:\\", dialogForm.InitialDirectory);
            Assert.Equal("Open File Dialog", dialogForm.Title);
        }

        [Fact]
        public void OpenFileDialog_ViewModel_Property()
        {
            var model = CreateOpenFileDialogModel();
            var dialog = new OpenFileDialogStub(model);

            Assert.NotNull(dialog.ViewModel);
            Assert.Same(model, dialog.ViewModel);
        }

        private IOpenFileDialogViewModel CreateOpenFileDialogModel()
        {
            var model = new OpenFileDialogViewModel
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

            return model;
        }

        public class OpenFileDialogStub : OpenFileDialog
        {
            public OpenFileDialogStub(IOpenFileDialogViewModel viewModel) 
                : base(viewModel)
            {
            }

            public System.Windows.Forms.OpenFileDialog GetCoreDialog()
            {
                return base.Dialog as System.Windows.Forms.OpenFileDialog;
            }
        }
    }
}