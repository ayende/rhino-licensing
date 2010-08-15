using System.IO;
using System.Security.Cryptography;
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

        public ProjectViewModel(
            IProjectService projectService,
            IDialogService dialogService)
        {
            _projectService = projectService;
            _dialogService = dialogService;
        }

        public virtual Project CurrentProject
        {
            get { return _project; }
            set
            {
                _project = value;
                NotifyOfPropertyChange(() => CurrentProject);
            }
        }

        public virtual void GenerateKey()
        {
            var key = RSA.Create();

            CurrentProject.Product.PublicKey = key.ToXmlString(false);
            CurrentProject.Product.PrivateKey = key.ToXmlString(true);
        }

        public virtual bool CanSave()
        {
            return CurrentProject.Product != null &&
                   CurrentProject.Product.Name.IsNotEmpty();
        }

        [AutoCheckAvailability]
        public virtual void Save()
        {
            var dialog = new SaveFileDialogViewModel();
            var result = _dialogService.ShowSaveFileDialog(dialog);

            if (result.GetValueOrDefault(false))
            {
                _projectService.Save(CurrentProject, new FileInfo(dialog.FileName));
            }
        }

        public override void TryClose(bool? dialogResult)
        {
            base.TryClose(dialogResult);
        }
    }
}