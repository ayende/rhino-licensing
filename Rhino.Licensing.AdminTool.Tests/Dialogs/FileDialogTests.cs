using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Mocks;
using Xunit;
using System.Linq;
using Xunit.Extensions;
using DialogForm = System.Windows.Forms.FileDialog;
using OpenFileDialog = Rhino.Licensing.AdminTool.Dialogs.OpenFileDialog;

namespace Rhino.Licensing.AdminTool.Tests.Dialogs
{
    public class FileDialogTests
    {
        private readonly DialogForm _dialogForm;

        public FileDialogTests()
        {
            _dialogForm = MockRepository.GenerateMock<DialogForm>();
        }

        [Fact]
        public void ShowDialog_Sets_Selected_Files()
        {
            var model = new OpenFileDialogViewModel();
            var dialog = new TestOpenFileDialog(_dialogForm) { ViewModel = model };

            _dialogForm.Expect(x => x.ShowDialog()).Return(System.Windows.Forms.DialogResult.OK);
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
            var dialog = new TestOpenFileDialog(_dialogForm) { ViewModel = model };

            dialog.ShowDialog();
            dialog.Dispose();

            _dialogForm.AssertWasCalled(x => x.Dispose(), x => x.Repeat.Once());
        }

        [Theory]
        [InlineData(System.Windows.Forms.DialogResult.OK, true)]
        [InlineData(System.Windows.Forms.DialogResult.Cancel, false)]
        [InlineData(System.Windows.Forms.DialogResult.Abort, null)]
        public void DialogResult_Maps_To_ViewModel_Result(System.Windows.Forms.DialogResult dialogResult, bool? mappedResult)
        {
            var model = new OpenFileDialogViewModel();
            var dialog = new TestOpenFileDialog(_dialogForm) { ViewModel = model };

            _dialogForm.Expect(x => x.ShowDialog()).Return(dialogResult);

            dialog.ShowDialog();
            
            Assert.Equal(mappedResult, model.Result);
        }

        public class TestOpenFileDialog : OpenFileDialog
        {
            private readonly DialogForm _form;

            public TestOpenFileDialog(DialogForm form)
            {
                _form = form;
            }

            protected override DialogForm Dialog
            {
                get { return _form; }
            }
        }
    }
}