using System;
using System.IO;
using Rhino.Licensing.AdminTool.Model;

namespace Rhino.Licensing.AdminTool.Services
{
    public interface IProjectService
    {
        void Save(Project project);

        Project Create();

        Project Load(FileInfo file);
    }

    public class ProjectService : IProjectService
    {
        public void Save(Project project)
        {
            
        }

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