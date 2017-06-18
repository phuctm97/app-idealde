using System;
using Idealde.Framework.ProjectExplorer.Models;

namespace Idealde.Framework.Projects
{
    public interface IProjectService
    {
        ProjectItemDefinition GetProjectItemDefinition(Type projectItemDefinitionType);
    }
}