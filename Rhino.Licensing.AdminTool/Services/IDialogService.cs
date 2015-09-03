using Rhino.Licensing.AdminTool.ViewModels;

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
}