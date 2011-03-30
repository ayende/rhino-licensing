using System.Windows.Forms;
using Rhino.Licensing.AdminTool.ViewModels;
using OpenDialog = System.Windows.Forms.OpenFileDialog;

namespace Rhino.Licensing.AdminTool.Dialogs
{
    public class OpenFileDialog : FileDialog<IOpenFileDialogViewModel>
    {
        private readonly OpenDialog _openFileDialog;

        public OpenFileDialog()
        {
            _openFileDialog = new OpenDialog();
        }

        protected override FileDialog Dialog
        {
            get { return _openFileDialog; }
        }

        protected override void BindDialogToViewModel()
        {
            base.BindDialogToViewModel();

            _openFileDialog.AddExtension = ViewModel.AddExtension;
            _openFileDialog.CheckFileExists = ViewModel.CheckFileExists;
            _openFileDialog.CheckPathExists = ViewModel.CheckPathExists;
            _openFileDialog.DefaultExt = ViewModel.DefaultExtension;
            _openFileDialog.FileName = ViewModel.FileName;
            _openFileDialog.Filter = ViewModel.Filter;
            _openFileDialog.InitialDirectory = ViewModel.InitialDirectory;
            _openFileDialog.Multiselect = ViewModel.MultiSelect;
            _openFileDialog.Title = ViewModel.Title;
        }
    }
}