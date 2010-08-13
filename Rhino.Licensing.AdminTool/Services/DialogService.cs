using System.Windows.Forms;
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
        /// <param name="dialogModel"></param>
        /// <returns></returns>
        bool? ShowOpenFileDialog(IOpenFileDialogViewModel dialogModel);

        /// <summary>
        /// Shows a SaveFileDialog
        /// </summary>
        /// <param name="dialogModel"></param>
        /// <returns></returns>
        bool? ShowSaveFileDialog(ISaveFileDialogViewModel dialogModel);
    }

    public class DialogService : IDialogService
    {
        private readonly IDialogFactory _dialogFactory;

        public DialogService(IDialogFactory dialogFactory)
        {
            _dialogFactory = dialogFactory;
        }

        public bool? ShowOpenFileDialog(IOpenFileDialogViewModel dialogModel)
        {
            var dialog = _dialogFactory.Create<OpenFileDialog>(dialogModel);
            var result = dialog.ShowDialog();

            _dialogFactory.Release(dialog);

            return MappedResult(result);
        }

        public bool? ShowSaveFileDialog(ISaveFileDialogViewModel dialogModel)
        {
            var dialog = _dialogFactory.Create<SaveFileDialog>(dialogModel);
            var result = dialog.ShowDialog();

            _dialogFactory.Release(dialog);
            
            return MappedResult(result);
        }

        private static bool? MappedResult(DialogResult result)
        {
            if (result == DialogResult.OK)
            {
                return true;
            }

            if (result == DialogResult.Cancel)
            {
                return false;
            }

            return null;
        }
    }
}