#region Using Namespace

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Caliburn.Micro;
using Idealde.Framework.Projects;

#endregion

namespace Idealde.Framework.ProjectExplorer.Models
{
    public class ProjectController : IProjectController
    {
        private readonly Dictionary<Type, ProjectItemDefinition> _projectItemTypeToDefinitionLookup;
        public List<FileInfo> Files { get;set;}
        public List<string> Folders { get; set; }
        public List<string> LibraryFiles { get; set; }
        public List<string> OutputType { get; set; }

        public ProjectController()
        {
            _projectItemTypeToDefinitionLookup = new Dictionary<Type, ProjectItemDefinition>();
            Files = new List<FileInfo>();
            Folders = new List<string>();
            LibraryFiles = new List<string>();
            OutputType = new List<string>();
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

        public void Load(string path)
        {
            var projectFile = XElement.Load(path);

            foreach (var file in projectFile.Descendants("FileItem"))
            {
                Files.Add(new FileInfo(file.Element("Name")?.Value,
                    file.Element("VitualAddress")?.Value,
                    file.Element("MemoryAddress")?.Value));
            }

            foreach (var folder in projectFile.Descendants("FolderItem"))
            {
                Folders.Add(folder.Value);
            }

            foreach (var libFile in projectFile.Descendants("LibFileItem"))
            {
                LibraryFiles.Add(libFile.Value);
            }

            foreach (var outputType in projectFile.Descendants("OutputItem"))
            {
                OutputType.Add(outputType.Value);
            }
        }

        public void Save(string path)
        {
            var projectFile = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("Project",
                    new XElement("FileGroup"),
                    new XElement("FolderGroup"),
                    new XElement("LibraryFileGroup"),
                    new XElement("OutputGroup")
                ));

            foreach (var fileInfo in Files)
            {
                projectFile.Root?.Element("FileGroup")?.Add(new XElement("FileItem",
                    new XElement("Name", fileInfo.Name),
                    new XElement("VitualAddress", fileInfo.VirtualAddress),
                    new XElement("MemoryAddress", fileInfo.MemoryAddress)
                ));
            }

            foreach (var folder in Folders)
            {
                projectFile.Root?.Element("FolderGroup")?.Add(new XElement("FolderItem", folder));
            }

            foreach (var libraryFile in LibraryFiles)
            {
                projectFile.Root?.Element("LibraryFileGroup")?.Add(new XElement("LibFileItem", libraryFile));
            }

            foreach (var outputType in OutputType)
            {
                projectFile.Root?.Element("OutputGroup")?.Add(new XElement("OutputItem", outputType));
            }

            projectFile.Save(path);
        }
    }
}