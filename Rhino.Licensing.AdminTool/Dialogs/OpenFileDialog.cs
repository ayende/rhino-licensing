using Rhino.Licensing.AdminTool.ViewModels;
using OpenDialog = System.Windows.Forms.OpenFileDialog;

namespace Rhino.Licensing.AdminTool.Dialogs
{
    public class OpenFileDialog : FileDialog
    {
        private readonly OpenDialog _openFileDialog;
        private readonly IOpenFileDialogViewModel _viewModel;

        public OpenFileDialog(IOpenFileDialogViewModel viewModel)
        {
            _viewModel = viewModel;
            _openFileDialog = new OpenDialog
            {
                AddExtension = viewModel.AddExtension,
                CheckFileExists = viewModel.CheckFileExists,
                CheckPathExists = viewModel.CheckPathExists,
                DefaultExt = viewModel.DefaultExtension,
                FileName = viewModel.FileName,
                Filter = viewModel.Filter,
                InitialDirectory = viewModel.InitialDirectory,
                Multiselect = viewModel.MultiSelect,
                Title = viewModel.Title
            };
        }

        protected override System.Windows.Forms.FileDialog Dialog
        {
            get { return _openFileDialog; }
        }

        public override IFileDialogViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}