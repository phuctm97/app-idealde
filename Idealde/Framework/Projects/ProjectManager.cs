#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Caliburn.Micro;
using Idealde.Framework.Projects;

#endregion

namespace Idealde.Framework.ProjectExplorer.Models
{
    public class ProjectManager : IProjectManager
    {
        // backing fields
        private readonly Dictionary<Type, ProjectItemDefinition> _projectItemTypeToDefinitionLookup;

        public ProjectManager()
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

        public ProjectInfo Load(string path)
        {
            var projectFile = XElement.Load(path);
            var projectInfo = new ProjectInfo();

            foreach (var file in projectFile.Descendants("FileItem"))
            {
                var name = file.Element("Name")?.Value ?? string.Empty;
                var virtualPath = file.Element("VitualAddress")?.Value ?? string.Empty;
                var realPath = file.Element("MemoryAddress")?.Value ?? string.Empty;

                projectInfo.Files.Add(new FileInfo(name, virtualPath, realPath));
            }

            foreach (var folder in projectFile.Descendants("FolderItem"))
            {
                projectInfo.IncludeDirectories.Add(folder?.Value ?? string.Empty);
            }

            foreach (var libFile in projectFile.Descendants("LibFileItem"))
            {
                projectInfo.PrebuiltLibraries.Add(libFile?.Value ?? string.Empty);
            }

            foreach (var outputType in projectFile.Descendants("OutputItem"))
            {
                var values = Enum.GetValues(typeof(ProjectOutputType)).Cast<ProjectOutputType>();
                foreach (var value in values)
                {
                    if (value.ToString().ToLower() == outputType?.Value.ToLower())
                    {
                        projectInfo.OutputType = value;
                        break;
                    }
                }
            }

            return projectInfo;
        }

        public void Save(ProjectInfo info, string path)
        {
            var projectFile = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("Project",
                    new XElement("FileGroup"),
                    new XElement("FolderGroup"),
                    new XElement("LibraryFileGroup"),
                    new XElement("OutputGroup")
                ));

            var root = projectFile.Root;
            if (root == null) return;

            var fileGroup = root.Element("FileGroup");
            if (fileGroup != null)
            {
                foreach (var file in info.Files)
                {
                    fileGroup.Add(new XElement("FileItem",
                        new XElement("Name", file.Name),
                        new XElement("VitualAddress", file.VirtualPath),
                        new XElement("MemoryAddress", file.RealPath)
                    ));
                }
            }

            var folderGroup = root.Element("FolderGroup");
            if (folderGroup != null)
            {
                foreach (var folder in info.IncludeDirectories)
                {
                    folderGroup.Add(new XElement("FolderItem", folder));
                }
            }

            var libraryGroup = root.Element("LibraryFileGroup");
            if (libraryGroup != null)
            {
                foreach (var libraryFile in info.PrebuiltLibraries)
                {
                    libraryGroup.Add(new XElement("LibFileItem", libraryFile));
                }
            }

            var outputGroup = root.Element("OutputGroup");
            if (outputGroup != null)
            {
               outputGroup.Add(new XElement("OutputItem", info.OutputType.ToString()));
            }

            projectFile.Save(path);
        }
    }
}