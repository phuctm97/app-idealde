#region Using Namespace

using System.Collections.Generic;
using Idealde.Framework.Projects;

#endregion

namespace Idealde.Framework.ProjectExplorer.Models
{
    public interface IProjectProvider
    {
        string Name { get; }

        IEnumerable<ProjectType> ProjectTypes { get; }

        ProjectInfoBase Load(string path);

        void Save(ProjectInfoBase info, string path);
    }
}