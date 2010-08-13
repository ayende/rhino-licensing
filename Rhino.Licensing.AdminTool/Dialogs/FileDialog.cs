using System;
using System.Windows.Forms;
using Rhino.Licensing.AdminTool.ViewModels;

namespace Rhino.Licensing.AdminTool.Dialogs
{
    public abstract class FileDialog : IDisposable
    {
        public DialogResult ShowDialog()
        {
            var result = Dialog.ShowDialog();

            ViewModel.FileName = Dialog.FileName;
            ViewModel.FileNames = Dialog.FileNames;

            return result;
        }

        protected abstract System.Windows.Forms.FileDialog Dialog
        {
            get;
        }

        public abstract IFileDialogViewModel ViewModel
        {
            get; 
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
    }
}