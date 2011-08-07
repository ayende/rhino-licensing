using System.IO;
using Rhino.Licensing.AdminTool.Model;

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
}