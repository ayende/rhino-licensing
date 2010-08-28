using Rhino.Licensing.AdminTool.Dialogs;
using Rhino.Licensing.AdminTool.ViewModels;

namespace Rhino.Licensing.AdminTool.Factories
{
    public interface IDialogFactory
    {
        /// <summary>
        /// Creates a new FileDialog
        /// </summary>
        /// <param name="dialogModel"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T>(IFileDialogViewModel dialogModel) where T : FileDialog;

        /// <summary>
        /// Releases file dialog for garbage collection
        /// </summary>
        /// <param name="dialog"></param>
        void Release(FileDialog dialog);
    }
}