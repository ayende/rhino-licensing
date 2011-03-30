using System;
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
        Project Open(FileInfo path);
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
                    writer = path.Create();
                    writer.Write(stream.GetBuffer(), 0, (int) stream.Length);
                    writer.Flush();
                }
                finally
                {
                    if (writer != null)
                    {
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

        public Project Open(FileInfo path)
        {
            Project project = null;
            FileStream reader = null;

            try
            {
                reader = new FileStream(path.FullName, FileMode.Open, FileAccess.Read);

                var serializer = new DataContractSerializer(typeof (Project));

                project = serializer.ReadObject(reader) as Project;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }

            return project;
        }
    }
}