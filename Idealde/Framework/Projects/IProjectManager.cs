#region Using Namespace

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Idealde.Framework.Projects;

#endregion

namespace Idealde.Framework.ProjectExplorer.Models
{
    public interface IProjectManager
    {
        ProjectItemDefinition GetProjectItemDefinition(Type projectItemDefinitionType);

        ProjectInfo Load(string path);

        void Save(ProjectInfo info, string path);
    }
}