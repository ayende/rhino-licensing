using System;
using System.Collections.Generic;
using System.IO;
using Rhino.Licensing.AdminTool.Model;
using Rhino.Licensing.AdminTool.Services;
using Xunit;

namespace Rhino.Licensing.AdminTool.Tests.Services
{
    public class ExportServiceTests
    {
        [Fact]
        public void Exports_LicenseInfo_To_File()
        {
            var service = new ExportService() as IExportService;
            var product = new Product { Id = Guid.NewGuid() };
            var license = new License { ID = Guid.NewGuid(), LicenseType = LicenseType.Standard, OwnerName = "License Owner", ExpirationDate = new DateTime(2020, 1, 1) };
            var path = Path.GetTempFileName();
            var file = new FileInfo(path);

            service.Export(product, license, file);

            var reader = file.OpenText();
            var content = reader.ReadToEnd();

            Assert.NotNull(content);
            Assert.Contains("<name>License Owner</name>", content);
            Assert.Contains("type=\"Standard\"", content);
            Assert.Contains("expiration=\"2020-01-01T00:00:00.0000000\"", content);
        }

        [Fact]
        public void Exports_With_MaxDate_If_No_Expiration_Date_Is_Set()
        {
            var service = new ExportService() as IExportService;
            var product = new Product { Id = Guid.NewGuid(), Name = "My Product", };
            var license = new License { ID = Guid.NewGuid(), LicenseType = LicenseType.Standard, OwnerName = "License Owner", ExpirationDate = null };
            var path = Path.GetTempFileName();
            var file = new FileInfo(path);

            service.Export(product, license, file);

            var reader = file.OpenText();
            var content = reader.ReadToEnd();

            Assert.NotNull(content);
            Assert.Contains("expiration=\"9999-12-31T23:59:59.9999999\"", content);
        }

        [Fact]
        public void Exports_With_User_Defined_KeyValues_When_Available()
        {
            var service = new ExportService() as IExportService;
            var product = new Product { Id = Guid.NewGuid(), Name = "My Product", };
            var license = new License
            {
                LicenseType = LicenseType.Standard, 
                OwnerName = "License Owner", 
                ExpirationDate = null,
            };
            
            license.Data.Add(new UserData { Key = "KeyOne", Value = "ValueOne"});
            license.Data.Add(new UserData { Key = "KeyTwo", Value = "ValueTwo"});

            var path = Path.GetTempFileName();
            var file = new FileInfo(path);

            service.Export(product, license, file);

            var reader = file.OpenText();
            var content = reader.ReadToEnd();

            Assert.NotNull(content);
            Assert.Contains("KeyOne=\"ValueOne\"", content);
            Assert.Contains("KeyTwo=\"ValueTwo\"", content);
        }
    }
}