using System.IO;
using Rhino.Licensing.AdminTool.Model;

namespace Rhino.Licensing.AdminTool.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// Saves the project to disk
        /// </summary>
        /// <param name="project">project to save</param>
        /// <param name="path">path to project file</param>
        void Save(Project project, FileInfo path);

        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <returns></returns>
        Project Create();

        /// <summary>
        /// Loads a project
        /// </summary>
        /// <param name="path">path to load the project from</param>
        /// <returns></returns>
        Project Open(FileInfo path);
    }
}