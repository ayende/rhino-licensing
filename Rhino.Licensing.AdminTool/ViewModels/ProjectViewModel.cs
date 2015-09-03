using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Filters;
using Caliburn.PresentationFramework.Screens;
using Rhino.Licensing.AdminTool.Extensions;
using Rhino.Licensing.AdminTool.Factories;
using Rhino.Licensing.AdminTool.Model;
using Rhino.Licensing.AdminTool.Services;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class ProjectViewModel : Screen
    {
        private readonly IProjectService _projectService;
        private readonly IDialogService _dialogService;
        private readonly IStatusService _statusService;
        private readonly IExportService _exportService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IWindowManager _windowManager;
        private Project _project;
        private License _selectedLicense;
        private string _filePath;

        public const string ProjectFileFilter = "Rhino Project|*.rlic";
        public const string LicenseFileFilter = "Rhino License|*.xml";

        public ProjectViewModel(
            IProjectService projectService,
            IDialogService dialogService,
            IStatusService statusService,
            IExportService exportService,
            IViewModelFactory viewModelFactory,
            IWindowManager windowManager)
        {
            _projectService = projectService;
            _dialogService = dialogService;
            _statusService = statusService;
            _exportService = exportService;
            _viewModelFactory = viewModelFactory;
            _windowManager = windowManager;
        }

        public virtual Project CurrentProject
        {
            get { return _project; }
            set
            {
                _project = value;
                NotifyOfPropertyChange(() => CurrentProject);
                OnCurrentProjectChanged();
            }
        }

        public virtual License SelectedLicense
        {
            get { return _selectedLicense; }
            set
            {
                _selectedLicense = value;
                NotifyOfPropertyChange(() => SelectedLicense);
            }
        }

        protected virtual void OnCurrentProjectChanged()
        {
            if(_project != null)
            {
                _statusService.Update("Loaded {0} license(s).", _project.Product.IssuedLicenses.Count);
            }
        }

        public virtual bool CanSave()
        {
            return CurrentProject != null &&
                   CurrentProject.Product != null &&
                   CurrentProject.Product.Name.IsNotEmpty();
        }

        [AutoCheckAvailability]
        public virtual void Save()
        {
            var model = _viewModelFactory.Create<ISaveFileDialogViewModel>();
            model.Filter = ProjectFileFilter;

            if(_filePath == null)
            {
                _dialogService.ShowSaveFileDialog(model);
                if (model.Result.GetValueOrDefault(false) && model.FileName.IsNotEmpty())
                {
                    _filePath = model.FileName;
                }
            }
            
            if(_filePath != null)
            {
                _projectService.Save(CurrentProject, new FileInfo(_filePath));
            }
        }

        [AutoCheckAvailability]
        public virtual bool Open()
        {
            var model = _viewModelFactory.Create<IOpenFileDialogViewModel>();
            model.Filter = ProjectFileFilter;

            _dialogService.ShowOpenFileDialog(model);

            if(model.Result.GetValueOrDefault(false) && model.FileName.IsNotEmpty())
            {
                _filePath = model.FileName;
                CurrentProject = _projectService.Open(new FileInfo(_filePath));

                return true;
            }

            return false;
        }

        [AutoCheckAvailability]
        public virtual void AddLicense()
        {
            var vm = _viewModelFactory.Create<IssueLicenseViewModel>();
            var dialogResult = _windowManager.ShowDialog(vm);

            if (dialogResult.GetValueOrDefault(false))
            {
                CurrentProject.Product.IssuedLicenses.Add(vm.CurrentLicense);
            }
        }

        public virtual bool CanAddLicense()
        {
            return CurrentProject != null &&
                   CurrentProject.Product.PublicKey.IsNotEmpty() &&
                   CurrentProject.Product.PrivateKey.IsNotEmpty();
        }

        public virtual bool CanExportLicense()
        {
            return SelectedLicense != null;
        }

        [AutoCheckAvailability]
        public virtual void ExportLicense()
        {
            var license = SelectedLicense;
            var product = CurrentProject.Product;
            var model = _viewModelFactory.Create<ISaveFileDialogViewModel>();
            model.Filter = LicenseFileFilter;

            _dialogService.ShowSaveFileDialog(model);

            if(model.Result.GetValueOrDefault(false))
            {
                _exportService.Export(product, license, new FileInfo(model.FileName));
            }
        }

        public virtual void CopyToClipboard(string text)
        {
            try
            {
                Clipboard.SetText(text, TextDataFormat.UnicodeText);
            }
            catch(COMException)
            {
                //May thorw COM exception
            }
        }
    }
}