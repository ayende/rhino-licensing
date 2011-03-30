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
        void ShowOpenFileDialog(IOpenFileDialogViewModel model);

        /// <summary>
        /// Shows a SaveFileDialog
        /// </summary>
        /// <returns></returns>
        void ShowSaveFileDialog(ISaveFileDialogViewModel model);
    }

    public class DialogService : IDialogService
    {
        private readonly IDialogFactory _dialogFactory;

        public DialogService(IDialogFactory dialogFactory)
        {
            _dialogFactory = dialogFactory;
        }

        public virtual void ShowOpenFileDialog(IOpenFileDialogViewModel model)
        {
            var dialog = _dialogFactory.Create<OpenFileDialog, IOpenFileDialogViewModel>(model);

            dialog.ViewModel = model;
            dialog.ShowDialog();

            _dialogFactory.Release(dialog);
        }

        public virtual void ShowSaveFileDialog(ISaveFileDialogViewModel model)
        {
            var dialog = _dialogFactory.Create<SaveFileDialog, ISaveFileDialogViewModel>(model);

            dialog.ViewModel = model;
            dialog.ShowDialog();

            _dialogFactory.Release(dialog);
        }

    }
}