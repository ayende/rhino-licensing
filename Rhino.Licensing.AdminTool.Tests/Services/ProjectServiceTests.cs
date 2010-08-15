using System;
using System.IO;
using Rhino.Licensing.AdminTool.Model;
using Rhino.Licensing.AdminTool.Services;
using Xunit;

namespace Rhino.Licensing.AdminTool.Tests.Services
{
    public class ProjectServiceTests
    {
        [Fact]
        public void Can_Save_Project_Graph()
        {
            var p = CreateNewProject();
            var service = new ProjectService() as IProjectService;
            var fileInfo = new FileInfo(Path.GetTempFileName());

            service.Save(p, fileInfo);

            var reader = fileInfo.OpenText();
            var content = reader.ReadToEnd();

            Assert.NotNull(content);
            Assert.NotEmpty(content);
        }

        [Fact(Skip = "Won't run due to reported bug in Caliburn")]
        public void Can_Load_Project_Graph()
        {
            var p = CreateNewProject();
            var service = new ProjectService() as IProjectService;
            var fileInfo = new FileInfo(Path.GetTempFileName());

            service.Save(p, fileInfo);

            var project = service.Load(fileInfo);
            
            Assert.NotNull(project);
            Assert.NotNull(project.Product);
            Assert.Equal("New Project", project.Name);
            Assert.Equal("Rhino Mocks", project.Product.Name);
            Assert.Equal("Private Key", project.Product.PrivateKey);
            Assert.Equal("Public Key", project.Product.PublicKey);
        }

        private Project CreateNewProject()
        {
            return new Project
            {
                Name = "New Project",
                Product = new Product
                {
                    Name = "Rhino Mocks",
                    PrivateKey = "Private Key",
                    PublicKey = "Public Key"
                }
            };
        }
    }
}