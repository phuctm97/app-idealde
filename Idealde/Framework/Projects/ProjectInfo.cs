#region Using Namespace

using System.Collections.Generic;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Modules.ProjectExplorer.Models;

#endregion

namespace Idealde.Framework.Projects
{
    public class FileInfo
    {
        public string VirtualPath { set; get; }

        public string RealPath { set; get; }

        public FileInfo(string virtualPath, string realPath)
        {
            VirtualPath = virtualPath;
            RealPath = realPath;
        }
    }

    public class ProjectType
    {
        public ProjectType(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }

        public string Name { get; }
        public string Extension { get; }
    }

    public abstract class ProjectInfoBase
    {
        protected ProjectInfoBase(IProjectProvider provider)
        {
            Provider = provider;
            if (provider == null) return;

            ProjectItem = new ProjectItem(provider.ProjectItemDefinitionType);
        }

        public ProjectItemBase ProjectItem { get; }

        public abstract IList<FileInfo> Files { get; }

        public string Path { get; set; }

        public string ProjectName { get; set; }

        public IProjectProvider Provider { get; }
    }
}