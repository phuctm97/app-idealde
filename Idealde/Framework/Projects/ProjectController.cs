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
        public List < FileInfo > Files;
        public List < string > Folders;
        public List < string > LibraryFiles;
        public List < string > OutputType;

        public ProjectController()
        {
            _projectItemTypeToDefinitionLookup = new Dictionary<Type, ProjectItemDefinition>();
            Folders=new List < string > ();
            LibraryFiles=new List < string > ();
            OutputType =new List < string > ();
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

        public void Load (string path)
        {
            XElement projectFile=XElement.Load(path);

            foreach ( var file in projectFile.Descendants("FileItem") )
            {
                Files.Add(new FileInfo(file.Element("Name").Value.ToString(),
                    file.Element("VitualAdsress").Value.ToString(),
                    file.Element("MemoryAddress").Value.ToString()));
            }

            foreach ( var folder in projectFile.Descendants("FolderGroup") )
            {
                Folders.Add(folder.Value.ToString());
            }

            foreach (var libFile in projectFile.Descendants("LibraryFileGroup"))
            {
                LibraryFiles.Add(libFile.Value.ToString());
            }

            foreach ( var outputType in projectFile.Descendants("OutputGroup") )
            {
                OutputType.Add(outputType.Value.ToString());
            }

        }

        public void Save ( string path )
        {
            XDocument projectFile= new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("Project",
                    new XElement("FileGroup"),
                    new XElement("FolderGroup"),
                    new XElement("LibraryFileGroup"),
                    new XElement("OutputGroup")
                ));

            foreach ( var fileInfo in Files )
            {

                projectFile.Root.Element("FileGroup").Add(new XElement("FileItem",
                    new XElement("Name", fileInfo.Name),
                    new XElement("VitualAdsress", fileInfo.VirtualAddress),
                    new XElement("MemoryAddress", fileInfo.MemoryAddress)
                ));
            }

            foreach ( var folder in Folders )
            {
                projectFile.Root.Element("FolderGroup").Add(new XElement("FolderItem", folder));
            }

            foreach ( var libraryFile in LibraryFiles )
            {
                projectFile.Root.Element("LibraryFileGroup").Add(new XElement("FolderItem", libraryFile));
            }

            foreach (var outputType in OutputType)
            {
                projectFile.Root.Element("OutputGroup").Add(new XElement("FolderItem", outputType));
            }

            projectFile.Save(path);
        }
    }
}