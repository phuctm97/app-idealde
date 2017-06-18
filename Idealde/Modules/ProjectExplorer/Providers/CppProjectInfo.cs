#region Using Namespace

using System.Collections.Generic;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;

#endregion

namespace Idealde.Modules.ProjectExplorer.Providers
{
    public enum CppProjectOutputType
    {
        Dll,
        Exe
    }

    public class CppProjectInfo : ProjectInfoBase
    {
        public override IList<FileInfo> Files { get; }

        public List<string> IncludeDirectories { get; }

        public List<string> PrebuiltLibraries { get; }

        public CppProjectOutputType OutputType { get; set; }

        public CppProjectInfo(IProjectProvider provider = null)
            : base(provider)
        {
            Files = new List<FileInfo>();
            IncludeDirectories = new List<string>();
            PrebuiltLibraries = new List<string>();
            OutputType = CppProjectOutputType.Exe;
        }
    }
}