using System.Windows.Forms;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Mocks;
using Xunit;
using System.Linq;
using FileDialog = Rhino.Licensing.AdminTool.Dialogs.FileDialog;

namespace Rhino.Licensing.AdminTool.Tests.Dialogs
{
    public class FileDialogTests
    {
        [Fact]
        public void ShowDialog_Sets_Selected_Files()
        {
            var dialogForm = MockRepository.GenerateMock<System.Windows.Forms.FileDialog>();

            var model = new OpenFileDialogViewModel();
            var dialog = new TestFileDialog(dialogForm, model);

            dialogForm.Expect(x => x.ShowDialog()).Return(DialogResult.OK);
            dialogForm.Expect(x => x.FileName).Return("License.lic");
            dialogForm.Expect(x => x.FileNames).Return(new[] {"License.lic", "License2.lic"});

            dialog.ShowDialog();

            Assert.Equal("License.lic", model.FileName);
            Assert.Contains("License.lic", model.FileNames);
            Assert.Contains("License2.lic", model.FileNames);
            Assert.Equal(2, model.FileNames.Count());
        }

        [Fact]
        public void Dialog_Disposes_Upon_Destruction()
        {
            var dialogForm = MockRepository.GenerateMock<System.Windows.Forms.FileDialog>();

            var model = new OpenFileDialogViewModel();
            var dialog = new TestFileDialog(dialogForm, model);

            dialog.ShowDialog();
            dialog.Dispose();

            dialogForm.AssertWasCalled(x => x.Dispose(), x => x.Repeat.Once());
        }

        public class TestFileDialog : FileDialog
        {
            private System.Windows.Forms.FileDialog _dialog;
            private IFileDialogViewModel _viewModel;

            public TestFileDialog(System.Windows.Forms.FileDialog dialog, IFileDialogViewModel viewModel)
            {
                _dialog = dialog;
                _viewModel = viewModel;
            }

            protected override System.Windows.Forms.FileDialog Dialog
            {
                get { return _dialog; }
            }

            public override IFileDialogViewModel ViewModel
            {
                get { return _viewModel; }
            }
        }
    }
}