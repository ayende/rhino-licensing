using System.Collections.Generic;
using Caliburn.PresentationFramework.Screens;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public interface IFileDialogViewModel
    {
        bool AddExtension { get; set; }
        bool CheckFileExists { get; set; }
        bool CheckPathExists { get; set; }
        string DefaultExtension { get; set; }
        string FileName { get; set; }
        string Filter { get; set; }
        string InitialDirectory { get; set; }
        string Title { get; set; }
        string DisplayName { get; set; }
        IEnumerable<string> FileNames { get; set; }
    }

    public abstract class FileDialogViewModel : Screen, IFileDialogViewModel
    {
        private bool _addExtension;
        private bool _checkFileExists;
        private bool _checkPathExists;
        private string _defaultExtension;
        private string _fileName;
        private IEnumerable<string > _fileNames;
        private string _filter;
        private string _initialDirectory;
        private string _title;

        public virtual bool AddExtension
        {
            get { return _addExtension; }
            set
            {
                _addExtension = value;
                NotifyOfPropertyChange(() => AddExtension);
            }
        }

        public virtual bool CheckFileExists
        {
            get { return _checkFileExists; }
            set
            {
                _checkFileExists = value;
                NotifyOfPropertyChange(() => CheckFileExists);
            }
        }

        public virtual bool CheckPathExists
        {
            get { return _checkPathExists; }
            set
            {
                _checkPathExists = value;
                NotifyOfPropertyChange(() => CheckPathExists);
            }
        }

        public virtual string DefaultExtension
        {
            get { return _defaultExtension; }
            set
            {
                _defaultExtension = value;
                NotifyOfPropertyChange(() => DefaultExtension);
            }
        }

        public virtual string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyOfPropertyChange(() => FileName);
            }
        }

        public virtual IEnumerable<string> FileNames
        {
            get { return _fileNames; }
            set
            {
                _fileNames = value;
                NotifyOfPropertyChange(() => FileNames);
            }
        }

        public virtual string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                NotifyOfPropertyChange(() => Filter);
            }
        }

        public virtual string InitialDirectory
        {
            get { return _initialDirectory; }
            set
            {
                _initialDirectory = value;
                NotifyOfPropertyChange(() => InitialDirectory);
            }
        }

        public virtual string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }
    }
}