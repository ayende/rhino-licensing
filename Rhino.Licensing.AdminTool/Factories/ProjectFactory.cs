using System;
using System.IO;
using Rhino.Licensing.AdminTool.Model;

namespace Rhino.Licensing.AdminTool.Factories
{
    public interface IProjectFactory
    {
        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <returns></returns>
        Project Create();

        /// <summary>
        /// Loads a project object from
        /// specified file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Project Load(FileInfo file);
    }

    public class ProjectFactory : IProjectFactory
    {
        public Project Create()
        {
            return new Project()
            {
                Name = string.Empty,
                Product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = string.Empty
                }
            };
        }

        public Project Load(FileInfo file)
        {
            return null;
        }
    }
}