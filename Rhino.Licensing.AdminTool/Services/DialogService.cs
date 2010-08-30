using Rhino.Licensing.AdminTool.Factories;
using Rhino.Licensing.AdminTool.ViewModels;
using OpenFileDialog = Rhino.Licensing.AdminTool.Dialogs.OpenFileDialog;
using SaveFileDialog = Rhino.Licensing.AdminTool.Dialogs.SaveFileDialog;

namespace Rhino.Licensing.AdminTool.Services
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows a OpenFileDialog
        /// </summary>
        /// <returns></returns>
        IOpenFileDialogViewModel ShowOpenFileDialog();

        /// <summary>
        /// Shows a SaveFileDialog
        /// </summary>
        /// <returns></returns>
        ISaveFileDialogViewModel ShowSaveFileDialog();
    }

    public class DialogService : IDialogService
    {
        private readonly IDialogFactory _dialogFactory;

        public DialogService(IDialogFactory dialogFactory)
        {
            _dialogFactory = dialogFactory;
        }

        public virtual IOpenFileDialogViewModel ShowOpenFileDialog()
        {
            var dialog = _dialogFactory.Create<OpenFileDialog>();
            
            dialog.ShowDialog();

            _dialogFactory.Release(dialog);

            return dialog.ViewModel as IOpenFileDialogViewModel;
        }

        public virtual ISaveFileDialogViewModel ShowSaveFileDialog()
        {
            var dialog = _dialogFactory.Create<SaveFileDialog>();
            
            dialog.ShowDialog();

            _dialogFactory.Release(dialog);

            return dialog.ViewModel as ISaveFileDialogViewModel;
        }

    }
}