#region Using Namespace

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Idealde.Framework.Projects;

#endregion

namespace Idealde.Framework.ProjectExplorer.Models
{
    public class ProjectType
    {
        public ProjectType(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }

        public string Name { get; }
        public string Extension { get; }
    }

    public interface IProjectManager
    {
        IEnumerable<ProjectType> ProjectTypes { get; }

        ProjectItemDefinition GetProjectItemDefinition(Type projectItemDefinitionType);

        ProjectInfo Load(string path);

        void Save(ProjectInfo info, string path);
    }
}