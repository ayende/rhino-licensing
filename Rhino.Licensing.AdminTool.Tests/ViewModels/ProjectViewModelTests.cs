using System.IO;
using Caliburn.Testability;
using Rhino.Licensing.AdminTool.Model;
using Rhino.Licensing.AdminTool.Services;
using Rhino.Licensing.AdminTool.ViewModels;
using Rhino.Licensing.AdminTool.Views;
using Rhino.Mocks;
using Xunit;
using Caliburn.Testability.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.ViewModels
{
    public class ProjectViewModelTests
    {
        private readonly IDialogService _dialogService;
        private readonly IProjectService _projectService;

        public ProjectViewModelTests()
        {
            _dialogService = MockRepository.GenerateMock<IDialogService>();
            _projectService = MockRepository.GenerateMock<IProjectService>();
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
        public void Properties_Are_Bound()
        {
            var validator = Validator.For<ProjectView, ProjectViewModel>()
                              .Validate();

            validator.AssertWasBound(x => x.CurrentProject.Product.Name);
            validator.AssertWasBound(x => x.CurrentProject.Product.PrivateKey);
            validator.AssertWasBound(x => x.CurrentProject.Product.PublicKey);
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
            var vm = CreateViewModel();

            vm.Save();

            _dialogService.AssertWasCalled(x => x.ShowSaveFileDialog(Arg<ISaveFileDialogViewModel>.Is.Anything), x => x.Repeat.Once());
        }

        [Fact]
        public void Will_Proceed_To_Save_When_Dialog_Successfully_Closes()
        {
            var vm = CreateViewModel();
            var project = new Project();
            var temp = Path.GetTempFileName();

            _dialogService.Expect(x => x.ShowSaveFileDialog(Arg<ISaveFileDialogViewModel>.Is.Anything))
                          .WhenCalled(m =>
                          {
                              var model = (SaveFileDialogViewModel) m.Arguments[0];
                              model.FileName = temp;
                          })
                          .Return(true);

            vm.CurrentProject = project;
            vm.Save();
            
            _projectService.AssertWasCalled(x => x.Save(Arg.Is(project), Arg<FileInfo>.Is.Anything));
        }

        [Fact]
        public void Can_Generate_Key_Pair()
        {
            var vm = CreateViewModel();

            vm.CurrentProject = new Project {Product = new Product()};
            vm.GenerateKey();

            Assert.NotNull(vm.CurrentProject.Product.PublicKey);
            Assert.NotNull(vm.CurrentProject.Product.PrivateKey);
            Assert.Contains("<P>", vm.CurrentProject.Product.PrivateKey); //Makes sure it is only private
            Assert.Contains("<Modulus>", vm.CurrentProject.Product.PublicKey); //Makes sure it is public
        }

        private ProjectViewModel CreateViewModel()
        {
            return new ProjectViewModel(_projectService, _dialogService);
        }
    }
}