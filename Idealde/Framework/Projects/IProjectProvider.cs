﻿#region Using Namespace

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Idealde.Framework.Projects;
using Idealde.Modules.Compiler;

#endregion

namespace Idealde.Framework.ProjectExplorer.Models
{
    public interface IProjectProvider
    {
        string Name { get; }

        ICompiler Compiler { get; }

        Type PropertiesDocumentType { get; }

        Type ProjectItemDefinitionType { get; }

        IEnumerable<ProjectType> ProjectTypes { get; }

        string GetBinPath(ProjectInfoBase project);

        string GetCompileDirectory(ProjectInfoBase project);

        ProjectInfoBase Load(string path);

        Task<string> Save(ProjectInfoBase info, string path);
    }
}