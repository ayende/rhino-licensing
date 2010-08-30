using System.Windows.Forms;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Mocks;
using Xunit;
using System.Linq;
using Xunit.Extensions;
using FileDialog = Rhino.Licensing.AdminTool.Dialogs.FileDialog;
using DialogForm = System.Windows.Forms.FileDialog;

namespace Rhino.Licensing.AdminTool.Tests.Dialogs
{
    public class FileDialogTests
    {
        private DialogForm _dialogForm;

        public FileDialogTests()
        {
            _dialogForm = MockRepository.GenerateMock<DialogForm>();
        }

        [Fact]
        public void ShowDialog_Sets_Selected_Files()
        {
            var model = new OpenFileDialogViewModel();
            var dialog = new TestFileDialog(_dialogForm, model);

            _dialogForm.Expect(x => x.ShowDialog()).Return(DialogResult.OK);
            _dialogForm.Expect(x => x.FileName).Return("License.lic");
            _dialogForm.Expect(x => x.FileNames).Return(new[] {"License.lic", "License2.lic"});

            dialog.ShowDialog();

            Assert.Equal("License.lic", model.FileName);
            Assert.Contains("License.lic", model.FileNames);
            Assert.Contains("License2.lic", model.FileNames);
            Assert.Equal(2, model.FileNames.Count());
        }

        [Fact]
        public void Dialog_Disposes_Upon_Destruction()
        {
            var model = new OpenFileDialogViewModel();
            var dialog = new TestFileDialog(_dialogForm, model);

            dialog.ShowDialog();
            dialog.Dispose();

            _dialogForm.AssertWasCalled(x => x.Dispose(), x => x.Repeat.Once());
        }

        [Theory]
        [InlineData(DialogResult.OK, true)]
        [InlineData(DialogResult.Cancel, false)]
        [InlineData(DialogResult.Abort, null)]
        public void DialogResult_Maps_To_ViewModel_Result(DialogResult dialogResult, bool? mappedResult)
        {
            var model = new OpenFileDialogViewModel();
            var dialog = new TestFileDialog(_dialogForm, model);

            _dialogForm.Expect(x => x.ShowDialog()).Return(dialogResult);

            dialog.ShowDialog();
            
            Assert.Equal(mappedResult, model.Result);
        }

        public class TestFileDialog : FileDialog
        {
            private readonly System.Windows.Forms.FileDialog _dialog;
            private readonly IFileDialogViewModel _viewModel;

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