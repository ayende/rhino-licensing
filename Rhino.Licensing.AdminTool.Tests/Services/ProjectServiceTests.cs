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
        public void Can_Save_Project()
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

        [Fact]
        public void Can_Load_Project()
        {
            var p = CreateNewProject();
            var service = new ProjectService() as IProjectService;
            var fileInfo = new FileInfo(Path.GetTempFileName());

            service.Save(p, fileInfo);

            var project = service.Open(fileInfo);
            
            Assert.NotNull(project);
            Assert.NotNull(project.Product);
            Assert.Equal("Rhino Mocks", project.Product.Name);
            Assert.Equal("Private Key", project.Product.PrivateKey);
            Assert.Equal("Public Key", project.Product.PublicKey);
        }

        [Fact]
        public void Can_Load_Project_With_Licenses()
        {
            var p = CreateNewProject();
            var service = new ProjectService() as IProjectService;
            var fileInfo = new FileInfo(Path.GetTempFileName());
            var expirationDate = new DateTime(2010, 5, 5);

            p.Product.IssuedLicenses.Add(new License { ExpirationDate = expirationDate, LicenseType = LicenseType.Trial, OwnerName = "John Doe" });
            service.Save(p, fileInfo);
            var project = service.Open(fileInfo);

            Assert.NotNull(project);
            Assert.Equal(1, project.Product.IssuedLicenses.Count);
            Assert.Equal("John Doe", project.Product.IssuedLicenses[0].OwnerName);
            Assert.Equal(expirationDate, project.Product.IssuedLicenses[0].ExpirationDate);
            Assert.Equal(LicenseType.Trial, project.Product.IssuedLicenses[0].LicenseType);
        }

        [Fact]
        public void Can_Create_New_Project()
        {
            var service = new ProjectService() as IProjectService;

            var project = service.Create();

            Assert.NotNull(project);
        }

        private Project CreateNewProject()
        {
            return new Project
            {
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