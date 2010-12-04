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
        private Project _project;

        private readonly IProjectService _projectService;
        private readonly IDialogService _dialogService;
        private readonly IStatusService _statusService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IWindowManager _windowManager;
        private string _filePath;

        public ProjectViewModel(
            IProjectService projectService,
            IDialogService dialogService,
            IStatusService statusService,
            IViewModelFactory viewModelFactory,
            IWindowManager windowManager)
        {
            _projectService = projectService;
            _dialogService = dialogService;
            _statusService = statusService;
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
            var model = CreateSaveDialogModel();

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
            var model = CreateOpenDialogModel();

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

        public bool CanAddLicense()
        {
            return CurrentProject != null &&
                   CurrentProject.Product.PublicKey.IsNotEmpty() &&
                   CurrentProject.Product.PrivateKey.IsNotEmpty();
        }

        public virtual IOpenFileDialogViewModel CreateOpenDialogModel()
        {
            return new OpenFileDialogViewModel
            {
                Filter = "Rhino License|*.rlic",
            };
        }

        public virtual ISaveFileDialogViewModel CreateSaveDialogModel()
        {
            return new SaveFileDialogViewModel
            {
                Filter = "Rhino License|*.rlic",
                OverwritePrompt = true,
            };
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