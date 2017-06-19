﻿#region Using Namespace

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Idealde.Framework.Projects;

#endregion

namespace Idealde.Framework.ProjectExplorer.Models
{
    public interface IProjectProvider
    {
        string Name { get; }

        Type ProjectItemDefinitionType { get; }

        IEnumerable<ProjectType> ProjectTypes { get; }

        ProjectInfoBase Load(string path);

        Task<string> Save(ProjectInfoBase info, string path);
    }
}