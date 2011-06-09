using System.IO;
using Rhino.Licensing.AdminTool.Model;
using System;

namespace Rhino.Licensing.AdminTool.Services
{
    public interface IExportService
    {
        /// <summary>
        /// Exports a license as xml format
        /// </summary>
        /// <param name="product"></param>
        /// <param name="license"></param>
        /// <param name="path"></param>
        void Export(Product product, License license, FileInfo path);
    }

    public class ExportService : IExportService
    {
        public void Export(Product product, License license, FileInfo path)
        {
            var generator = new LicenseGenerator(product.PrivateKey);
            var expiration = license.ExpirationDate.GetValueOrDefault(DateTime.MaxValue);
            var key = generator.Generate(license.OwnerName, license.ID, expiration, LicenseType.Trial);

            File.WriteAllText(path.FullName, key);
        }
    }
}