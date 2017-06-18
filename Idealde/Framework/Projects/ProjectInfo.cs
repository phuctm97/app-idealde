#region Using Namespace

using System.Collections.Generic;

#endregion

namespace Idealde.Framework.Projects
{
    public class FileInfo
    {
        public string Name { set; get; }

        public string VirtualPath { set; get; }

        public string RealPath { set; get; }

        public FileInfo(string name, string virtualPath, string realPath)
        {
            Name = name;
            VirtualPath = virtualPath;
            RealPath = realPath;
        }
    }

    public enum ProjectOutputType
    {
        Dll,
        Exe
    }

    public class ProjectInfo
    {
        public List<FileInfo> Files { get; }

        public List<string> IncludeDirectories { get; }

        public List<string> PrebuiltLibraries { get; }

        public ProjectOutputType OutputType { get; set; }

        public string Path { get; set; }

        public ProjectInfo()
        {
            Files = new List<FileInfo>();
            IncludeDirectories = new List<string>();
            PrebuiltLibraries = new List<string>();
            OutputType = ProjectOutputType.Exe;
        }
    }
}