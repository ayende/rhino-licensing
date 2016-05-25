using System.IO;
using Rhino.Licensing.AdminTool.Model;
using System;
using System.Linq;

namespace Rhino.Licensing.AdminTool.Services
{
    public class ExportService : IExportService
    {
        public void Export(Product product, License license, FileInfo path)
        {
            var generator = new LicenseGenerator(product.PrivateKey);
            var expiration = license.ExpirationDate.GetValueOrDefault(DateTime.MaxValue);
            var key = generator.Generate(license.OwnerName, license.ID, expiration, license.Data.ToDictionary(userData => userData.Key, userData => userData.Value), license.LicenseType);

            File.WriteAllText(path.FullName, key);
        }
    }
}