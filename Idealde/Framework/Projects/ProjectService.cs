#region Using Namespace

using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Idealde.Framework.ProjectExplorer.Models;

#endregion

namespace Idealde.Framework.Projects
{
    public class ProjectService : IProjectService
    {
        // Backing fields

        #region Backing fields

        private readonly Dictionary<Type, ProjectItemDefinition> _projectItemTypeToDefinitionLookup;

        #endregion

        // Initializations

        #region Initializations

        public ProjectService()
        {
            _projectItemTypeToDefinitionLookup = new Dictionary<Type, ProjectItemDefinition>();
        }

        #endregion

        // Features

        #region Features

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

        #endregion
    }
}