#region Using Namespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Caliburn.Micro;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;
using Idealde.Modules.CodeCompiler;
using Idealde.Modules.ProjectExplorer.Models;
using FileInfo = Idealde.Framework.Projects.FileInfo;

#endregion

namespace Idealde.Modules.ProjectExplorer.Providers
{
    public class CppProjectProvider : IProjectProvider
    {
        public string Name => "C++ Project";

        public string GetBinPath(ProjectInfoBase project)
        {
            CreateExtensionDirectoriesIfNotExist(project.Path);
            return $"{Path.GetDirectoryName(project.Path)}\\Output\\bin";
        }

        public string GetCompileDirectory(ProjectInfoBase project)
        {
            CreateExtensionDirectoriesIfNotExist(project.Path);
            return $"{Path.GetDirectoryName(project.Path)}\\Output\\obj";
        }

        public Type ProjectItemDefinitionType => typeof(CppProjectItemDefinition);

        public IEnumerable<ProjectType> ProjectTypes
        {
            get { yield return new ProjectType("CX Project", ".cxproj"); }
        }

        public ICompiler Compiler => IoC.Get<CppCompiler>();

        private bool ValidateExtension(string path)
        {
            var extension = Path.GetExtension(path);
            foreach (var projectType in ProjectTypes)
            {
                if (string.Equals(projectType.Extension, extension, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void CreateExtensionDirectoriesIfNotExist(string path)
        {
            var projectDirectory = Path.GetDirectoryName(path);

            // create project output directory
            foreach (var childDirectory in new[] {"Output", "Output\\obj", "Output\\bin"})
            {
                var d = $"{projectDirectory}\\{childDirectory}";
                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                }
            }
        }

        public ProjectInfoBase Load(string path)
        {
            if (!ValidateExtension(path))
            {
                return null;
            }

            var projectFile = XElement.Load(path);
            var projectInfo = new CppProjectInfo(this);
            var projectDirectory = Path.GetDirectoryName(path);

            // load files
            foreach (var file in projectFile.Descendants("FileItem"))
            {
                var virtualPath = file.Element("VirtualPath")?.Value ?? string.Empty;
                var realPath = file.Element("RealPath")?.Value ?? string.Empty;

                if (realPath.StartsWith(".\\"))
                {
                    realPath = projectDirectory + realPath.Remove(0, 1);
                }

                projectInfo.Files.Add(new FileInfo(virtualPath, realPath));
            }

            // load include directories
            foreach (var folder in projectFile.Descendants("FolderItem"))
            {
                projectInfo.IncludeDirectories.Add(folder?.Value ?? string.Empty);
            }

            // load prebuilt libraries
            foreach (var libFile in projectFile.Descendants("LibFileItem"))
            {
                projectInfo.PrebuiltLibraries.Add(libFile?.Value ?? string.Empty);
            }

            // load output types
            foreach (var outputType in projectFile.Descendants("OutputItem"))
            {
                var values = Enum.GetValues(typeof(CppProjectOutputType)).Cast<CppProjectOutputType>();
                foreach (var value in values)
                {
                    if (value.ToString().ToLower() == outputType?.Value.ToLower())
                    {
                        projectInfo.OutputType = value;
                        break;
                    }
                }
            }

            // load project name
            projectInfo.ProjectName = projectFile.Attribute("Name")?.Value;
            projectInfo.ProjectItem.Text = projectInfo.ProjectName;
            projectInfo.Path = path;

            // create extension directories
            CreateExtensionDirectoriesIfNotExist(path);

            return projectInfo;
        }

        public async Task<string> Save(ProjectInfoBase info, string path)
        {
            if (!(info is CppProjectInfo)) return string.Empty;
            var cppInfo = (CppProjectInfo) info;

            var projectFile = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("Project", new XAttribute("Name", cppInfo.ProjectName),
                    new XElement("FileGroup"),
                    new XElement("FolderGroup"),
                    new XElement("LibraryFileGroup"),
                    new XElement("OutputGroup")
                ));

            var projectDirectory = Path.GetDirectoryName(path) ?? string.Empty;

            // load root
            var root = projectFile.Root;
            if (root == null) return string.Empty;

            // save files
            var fileGroup = root.Element("FileGroup");
            if (fileGroup != null)
            {
                foreach (var file in info.Files)
                {
                    var realPath = file.RealPath;
                    if (realPath.StartsWith(projectDirectory, StringComparison.OrdinalIgnoreCase))
                    {
                        realPath = "." + realPath.Remove(0, projectDirectory.Length);
                    }

                    fileGroup.Add(new XElement("FileItem",
                        new XElement("VirtualPath", file.VirtualPath),
                        new XElement("RealPath", realPath)
                    ));
                }
            }

            // save include directories
            var folderGroup = root.Element("FolderGroup");
            if (folderGroup != null)
            {
                foreach (var folder in cppInfo.IncludeDirectories)
                {
                    folderGroup.Add(new XElement("FolderItem", folder));
                }
            }

            // save prebuilt libraries
            var libraryGroup = root.Element("LibraryFileGroup");
            if (libraryGroup != null)
            {
                foreach (var libraryFile in cppInfo.PrebuiltLibraries)
                {
                    libraryGroup.Add(new XElement("LibFileItem", libraryFile));
                }
            }

            // save output types
            var outputGroup = root.Element("OutputGroup");
            if (outputGroup != null)
            {
                outputGroup.Add(new XElement("OutputItem", cppInfo.OutputType.ToString()));
            }

            // save
            if (!ValidateExtension(path))
            {
                path = Path.ChangeExtension(path, ProjectTypes.First().Extension);
            }
            projectFile.Save(path);

            // create extension directories
            CreateExtensionDirectoriesIfNotExist(path);

            var timeToLives = 2000;
            while (timeToLives > 0 && !File.Exists(path))
            {
                await Task.Delay(25);
                timeToLives -= 25;
            }

            return path;
        }
    }
}