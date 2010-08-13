namespace Rhino.Licensing.AdminTool.ViewModels
{
    public interface ISaveFileDialogViewModel : IFileDialogViewModel
    {
        bool OverwritePrompt { get; set; }
        bool SupportMultiDottedExtensions { get; set; }
    }

    public class SaveFileDialogViewModel : FileDialogViewModel, ISaveFileDialogViewModel
    {
        private bool _overwritePrompt;
        private bool _supportMultiDottedExtensions;

        public bool OverwritePrompt
        {
            get { return _overwritePrompt; }
            set
            {
                _overwritePrompt = value;
                NotifyOfPropertyChange(() => OverwritePrompt);
            }
        }

        public bool SupportMultiDottedExtensions
        {
            get { return _supportMultiDottedExtensions; }
            set
            {
                _supportMultiDottedExtensions = value;
                NotifyOfPropertyChange(() => SupportMultiDottedExtensions);
            }
        }
    }
}