using System.IO;
using System.Runtime.Serialization;
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
        Project Load(FileInfo path);
    }

    public class ProjectService : IProjectService
    {
        public void Save(Project project, FileInfo path)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(Project));
                serializer.WriteObject(stream, project);

                FileStream writer = null;

                try
                {
                    writer = path.OpenWrite();
                    writer.Write(stream.GetBuffer(), 0, (int) stream.Length);
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Flush();
                        writer.Close();
                        writer.Dispose();
                    }
                }
            }
        }

        public Project Create()
        {
            return new Project();
        }

        public Project Load(FileInfo path)
        {
            var reader = path.OpenRead();
            var serializer = new DataContractSerializer(typeof(Project));

            return serializer.ReadObject(reader) as Project;
        }
    }
}