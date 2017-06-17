using System;
using System.Collections.Generic;

using Idealde.Framework.Projects;

namespace Idealde.Framework.ProjectExplorer.Models
{
    public interface IProjectController
    {
        ProjectItemDefinition GetProjectItemDefinition(Type projectItemDefinitionType);
        List<FileInfo> Files { get; set; }
        List<string> Folders { get; set; }
        List<string> LibraryFiles { get; set; }
        List<string> OutputType { get; set; }

        void Load ( string path );

        void Save ( string path );
    }
}