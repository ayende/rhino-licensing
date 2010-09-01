using Rhino.Licensing.AdminTool.Dialogs;
using Rhino.Licensing.AdminTool.ViewModels;

namespace Rhino.Licensing.AdminTool.Factories
{
    public interface IDialogFactory
    {
        /// <summary>
        /// Creates a new FileDialog
        /// </summary>
        /// <typeparam name="T">dialog Type</typeparam>
        /// <typeparam name="TViewModel">viewModel</typeparam>
        /// <returns></returns>
        T Create<T, TViewModel>(TViewModel viewModel)
            where T : FileDialog<TViewModel>
            where TViewModel : IFileDialogViewModel;

        /// <summary>
        /// Releases file dialog for garbage collection
        /// </summary>
        /// <param name="dialog"></param>
        void Release(object dialog);
    }
}