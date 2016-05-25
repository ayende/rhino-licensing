using System;
using System.Windows.Forms;
using Rhino.Licensing.AdminTool.ViewModels;

namespace Rhino.Licensing.AdminTool.Dialogs
{
    public abstract class FileDialog<T> : IDisposable 
        where T : IFileDialogViewModel
    {
        private T _viewModel;

        public virtual void ShowDialog()
        {
            var result = Dialog.ShowDialog();

            ViewModel.FileName = Dialog.FileName;
            ViewModel.FileNames = Dialog.FileNames;
            ViewModel.Result = MappedResult(result);
        }

        protected abstract FileDialog Dialog
        {
            get;
        }

        public virtual T ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                BindDialogToViewModel();
            }
        }

        protected virtual void BindDialogToViewModel()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FileDialog()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Dialog != null)
                {
                    Dialog.Dispose();
                }
            }
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