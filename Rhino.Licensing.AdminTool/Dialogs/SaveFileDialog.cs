using Rhino.Licensing.AdminTool.ViewModels;
using SaveDialog = System.Windows.Forms.SaveFileDialog;


namespace Rhino.Licensing.AdminTool.Dialogs
{
    public class SaveFileDialog : FileDialog
    {
        private readonly ISaveFileDialogViewModel _viewModel;
        private readonly SaveDialog _dialog;

        public SaveFileDialog(ISaveFileDialogViewModel viewModel)
        {
            _viewModel = viewModel;
            _dialog = new SaveDialog
            {
                AddExtension = viewModel.AddExtension,
                CheckFileExists = viewModel.CheckFileExists,
                CheckPathExists = viewModel.CheckPathExists,
                DefaultExt = viewModel.DefaultExtension,
                FileName = viewModel.FileName,
                Filter = viewModel.Filter,
                InitialDirectory = viewModel.InitialDirectory,
                Title = viewModel.Title,
                AutoUpgradeEnabled = true,
                OverwritePrompt = viewModel.OverwritePrompt,
                SupportMultiDottedExtensions = viewModel.SupportMultiDottedExtensions,
            };
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