using System.Windows.Forms;
using Rhino.Licensing.AdminTool.ViewModels;
using SaveDialog = System.Windows.Forms.SaveFileDialog;

namespace Rhino.Licensing.AdminTool.Dialogs
{
    public class SaveFileDialog : FileDialog<ISaveFileDialogViewModel>
    {
        private readonly SaveDialog _dialog;

        public SaveFileDialog()
        {
            _dialog = new SaveDialog();
        }

        protected override FileDialog Dialog
        {
            get { return _dialog; }
        }

        protected override void BindDialogToViewModel()
        {
            base.BindDialogToViewModel();

            _dialog.AddExtension = ViewModel.AddExtension;
            _dialog.CheckFileExists = ViewModel.CheckFileExists;
            _dialog.CheckPathExists = ViewModel.CheckPathExists;
            _dialog.DefaultExt = ViewModel.DefaultExtension;
            _dialog.FileName = ViewModel.FileName;
            _dialog.Filter = ViewModel.Filter;
            _dialog.InitialDirectory = ViewModel.InitialDirectory;
            _dialog.Title = ViewModel.Title;
            _dialog.AutoUpgradeEnabled = true;
            _dialog.OverwritePrompt = ViewModel.OverwritePrompt;
            _dialog.SupportMultiDottedExtensions = ViewModel.SupportMultiDottedExtensions;
        }
    }
}