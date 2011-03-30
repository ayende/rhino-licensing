using System.IO;
using System.Windows;
using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.Testability;
using Rhino.Licensing.AdminTool.Factories;
using Rhino.Licensing.AdminTool.Model;
using Rhino.Licensing.AdminTool.Services;
using Rhino.Licensing.AdminTool.Tests.Base;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Licensing.AdminTool.Views;
using Rhino.Mocks;
using Xunit;
using Caliburn.Testability.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.ViewModels
{
    public class ProjectViewModelTests : TestBase
    {
        private readonly IDialogService _dialogService;
        private readonly IProjectService _projectService;
        private readonly IStatusService _statusService;
        private readonly IWindowManager _windowManager;
        private readonly IViewModelFactory _viewModelFactory;

        public ProjectViewModelTests()
        {
            _dialogService = MockRepository.GenerateMock<IDialogService>();
            _projectService = MockRepository.GenerateMock<IProjectService>();
            _statusService = MockRepository.GenerateMock<IStatusService>();
            _windowManager = MockRepository.GenerateMock<IWindowManager>();
            _viewModelFactory = MockRepository.GenerateMock<IViewModelFactory>();
        }

        [Fact]
        public void Creating_New_ProductViewModel_Will_Have_Empty_Product()
        {
            var vm = CreateViewModel();
            
            Assert.Null(vm.CurrentProject);
        }

        [Fact]
        public void Fires_PropertyChange_Notification()
        {
            var vm = CreateViewModel();

            vm.AssertThatProperty(x => x.CurrentProject).RaisesChangeNotification();
        }

        [Fact]
        public void CurrentProject_Properties_Are_Bound()
        {
            var validator = Validator.For<ProjectView, ProjectViewModel>()
                              .Validate();

            validator.AssertWasBound(x => x.CurrentProject.Product.Name);
            validator.AssertWasBound(x => x.CurrentProject.Product.PrivateKey);
            validator.AssertWasBound(x => x.CurrentProject.Product.PublicKey);
            validator.AssertWasBound(x => x.CurrentProject.Product.IssuedLicenses);
        }

        [Fact]
        public void Can_Not_Save_If_Name_Is_Not_Provided()
        {
            var vm = CreateViewModel();

            vm.CurrentProject = new Project
            {
                Product = new Product
                {
                    Name = null
                }
            };

            Assert.False(vm.CanSave());
        }

        [Fact]
        public void Can_Save_If_Name_Is_Provided()
        {
            var vm = CreateViewModel();
            
            vm.CurrentProject = new Project
            {
                Product = new Product
                {
                    Name = "New Product"
                }
            };

            Assert.True(vm.CanSave());
        }

        [Fact]
        public void Save_Action_Will_Open_SaveDialog()
        {
            var dialogModel = new SaveFileDialogViewModel {Result = true};
            _dialogService.Expect(x => x.ShowSaveFileDialog(dialogModel));

            var vm = CreateViewModel(dialogModel);
            vm.Save();

            _dialogService.AssertWasCalled(x => x.ShowSaveFileDialog(Arg.Is(dialogModel)), x => x.Repeat.Once());
        }

        [Fact]
        public void Save_Action_Will_Call_ProjectService_If_Proper_Result_Is_Set()
        {
            var existingFile = Path.GetTempFileName();
            var choosenFile = new FileInfo(existingFile);
            var model = new SaveFileDialogViewModel {Result = true, FileName = existingFile};

            _dialogService.Expect(x => x.ShowSaveFileDialog(model));

            var vm = CreateViewModel(model);
            vm.Save();

            _projectService.Expect(x => x.Save(Arg<Project>.Is.Anything, Arg.Is(choosenFile))).Repeat.Once();
        }

        [Fact]
        public void Will_Not_Proceed_To_Save_When_No_File_Is_Selected()
        {
            var dialogModel = new SaveFileDialogViewModel {Result = true, FileName = null};
            _dialogService.Expect(x => x.ShowSaveFileDialog(dialogModel));

            var vm = CreateViewModel(dialogModel);
            vm.Save();
            
            _projectService.AssertWasNotCalled(x => x.Save(Arg<Project>.Is.Anything, Arg<FileInfo>.Is.Anything));
        }

        [Fact]
        public void Key_Pair_Is_Generated_When_A_Product_Is_Created()
        {
            var vm = CreateViewModel();

            vm.CurrentProject = new Project {Product = new Product()};

            Assert.NotNull(vm.CurrentProject.Product.PublicKey);
            Assert.NotNull(vm.CurrentProject.Product.PrivateKey);
            Assert.Contains("<P>", vm.CurrentProject.Product.PrivateKey); //Makes sure it is only private
            Assert.Contains("<Modulus>", vm.CurrentProject.Product.PublicKey); //Makes sure it is public
        }

        [Fact]
        public void Default_Project_Save_Dialog()
        {
            var vm = CreateProjectViewModel();
            var dialogModel = vm.CreateSaveDialogModel();

            Assert.Equal("Rhino License|*.rlic", dialogModel.Filter);
            Assert.True(dialogModel.OverwritePrompt);
        }

        [Fact]
        public void Default_Project_Open_Dialog()
        {
            var vm = CreateProjectViewModel();
            var dialogModel = vm.CreateOpenDialogModel();

            Assert.Equal("Rhino License|*.rlic", dialogModel.Filter);
        }

        [Fact]
        public void Can_Copy_Keys_To_Clipboard()
        {
            var keyContent = "Key Content";
            var vm = CreateViewModel();

            vm.CopyToClipboard(keyContent);

            var readback = Clipboard.GetText(TextDataFormat.UnicodeText);

            Assert.Equal(keyContent, readback);
        }

        [Fact]
        public void Calling_Save_For_Second_Time_Wont_Show_SaveDialog()
        {
            var dialogModel = new SaveFileDialogViewModel {Result = true, FileName = "C:\\"};
            var vm = CreateViewModel(dialogModel);
            
            vm.Save(); //For the first time, opens the dialog

            vm.Save(); //For the second time saves on the same file

            _dialogService.AssertWasCalled(x => x.ShowSaveFileDialog(Arg.Is(dialogModel)), options => options.Repeat.Once());
        }

        [Fact]
        public void Open_Shows_OpenDialog()
        {
            var dialogModel = new OpenFileDialogViewModel { Result = true, FileName = "C:\\" };
            var vm = CreateViewModel(dialogModel);
            
            vm.Open();

            _dialogService.AssertWasCalled(x => x.ShowOpenFileDialog(Arg.Is(dialogModel)), options => options.Repeat.Once());
        }

        [Fact]
        public void Open_Returns_False_When_User_Cancels_The_Open_Dialog()
        {
            var dialogModel = new OpenFileDialogViewModel { Result = false, FileName = "C:\\" };
            var vm = CreateViewModel(dialogModel);

            var opened = vm.Open();

            Assert.False(opened);
        }

        [Fact]
        public void Open_Loads_Project()
        {
            var p = new Project();
            _projectService.Expect(x => x.Open(Arg<FileInfo>.Is.Anything)).Return(p);

            var dialogModel = new OpenFileDialogViewModel { Result = true, FileName = "C:\\" };
            var vm = CreateViewModel(dialogModel);

            vm.Open();

            Assert.NotNull(vm.CurrentProject);
            Assert.Same(p, vm.CurrentProject);
        }

        [Fact]
        public void Can_Issue_New_License_When_Keys_Are_Generated()
        {
            var vm = CreateViewModel();

            vm.CurrentProject = new Project();
            var canAdd = vm.CanAddLicense();

            Assert.True(canAdd);
        }

        [Fact]
        public void Can_Issues_New_License()
        {
            var vm = CreateViewModel();
            var issueVm = new IssueLicenseViewModel();

            _viewModelFactory.Expect(f => f.Create<IssueLicenseViewModel>()).Return(issueVm);
            _windowManager.Expect(w => w.ShowDialog(Arg.Is(issueVm), Arg<object>.Is.Null)).Return(true);

            vm.CurrentProject = new Project { Product = new Product() };
            vm.AddLicense();

            Assert.Equal(1, vm.CurrentProject.Product.IssuedLicenses.Count);
        }

        private ProjectViewModel CreateViewModel(ISaveFileDialogViewModel model)
        {
            return new TestProjectViewModel(_projectService, _dialogService, model, null, _statusService, _viewModelFactory, _windowManager);
        }

        private ProjectViewModel CreateViewModel(IOpenFileDialogViewModel model)
        {
            return new TestProjectViewModel(_projectService, _dialogService, null, model, _statusService, _viewModelFactory, _windowManager);
        }

        private ProjectViewModel CreateProjectViewModel()
        {
            return new ProjectViewModel(_projectService, _dialogService, _statusService, _viewModelFactory, _windowManager);
        }

        private ProjectViewModel CreateViewModel()
        {
            return new TestProjectViewModel(_projectService, _dialogService, null, null, _statusService, _viewModelFactory, _windowManager);
        }

        public class TestProjectViewModel : ProjectViewModel
        {
            private readonly ISaveFileDialogViewModel _saveDialogModel;
            private readonly IOpenFileDialogViewModel _openDialogModel;

            public TestProjectViewModel(
                IProjectService projectService, 
                IDialogService dialogService, 
                ISaveFileDialogViewModel saveModel, 
                IOpenFileDialogViewModel openModel,
                IStatusService statusService,
                IViewModelFactory viewModelFactory, 
                IWindowManager windowManager) 
                : base(projectService, dialogService, statusService, viewModelFactory, windowManager)
            {
                _saveDialogModel = saveModel;
                _openDialogModel = openModel;
            }

            public override ISaveFileDialogViewModel CreateSaveDialogModel()
            {
                return _saveDialogModel; 
            }

            public override IOpenFileDialogViewModel CreateOpenDialogModel()
            {
                return _openDialogModel;
            }
        }
    }
}