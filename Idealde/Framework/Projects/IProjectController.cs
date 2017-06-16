using System;

namespace Idealde.Framework.ProjectExplorer.Models
{
    public interface IProjectController
    {
        ProjectItemDefinition GetProjectItemDefinition(Type projectItemDefinitionType);
    }
}