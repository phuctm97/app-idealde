#region Using Namespace

using System;
using System.Collections.Generic;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.ProjectExplorer.Models
{
    public class ProjectController : IProjectController
    {
        private readonly Dictionary<Type, ProjectItemDefinition> _projectItemTypeToDefinitionLookup;

        public ProjectController()
        {
            _projectItemTypeToDefinitionLookup = new Dictionary<Type, ProjectItemDefinition>();
        }

        public ProjectItemDefinition GetProjectItemDefinition(Type projectItemDefinitionType)
        {
            ProjectItemDefinition projectItemDefinition;
            if (!_projectItemTypeToDefinitionLookup.TryGetValue(projectItemDefinitionType, out projectItemDefinition))
            {
                projectItemDefinition = (ProjectItemDefinition) IoC.GetInstance(projectItemDefinitionType, string.Empty);
                _projectItemTypeToDefinitionLookup.Add(projectItemDefinitionType, projectItemDefinition);
            }
            return projectItemDefinition;
        }
    }
}