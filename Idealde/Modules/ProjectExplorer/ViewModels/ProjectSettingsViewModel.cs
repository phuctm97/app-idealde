#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class ProjectSettingsViewModel : PersistedDocument
    {
        private string _foldersInclude;
        private string _libraryFiles;
        private ProjectOutputType _outputType;
        public IEnumerable<ProjectOutputType> ListOutputs { get; }

        public string FoldersInclude
        {
            get { return _foldersInclude; }
            set
            {
                if (_foldersInclude != value)
                {
                    _foldersInclude = value;
                    NotifyOfPropertyChange(() => FoldersInclude);
                }
            }
        }

        public string LibraryFiles
        {
            get { return _libraryFiles; }
            set
            {
                if (_libraryFiles != value)
                {
                    _libraryFiles = value;
                    NotifyOfPropertyChange(() => LibraryFiles);
                }
            }
        }

        public ProjectOutputType OutputType
        {
            get { return _outputType; }
            set
            {
                if (_outputType != value)
                {
                    _outputType = value;
                    NotifyOfPropertyChange(() => LibraryFiles);
                }
            }
        }

        public ProjectSettingsViewModel()
        {
            ListOutputs = Enum.GetValues(typeof(ProjectOutputType)).Cast<ProjectOutputType>();
        }

        protected override Task DoNew()
        {
            return Task.FromResult(true);
        }

        protected override Task DoLoad()
        {
            var projectManager = IoC.Get<IProjectManager>();
            var projectInfo = projectManager.Load(FilePath);

            foreach (var folder in projectInfo.IncludeDirectories)
            {
                FoldersInclude += folder + Environment.NewLine;
            }
            FoldersInclude = FoldersInclude.Trim();

            foreach (var libraryFile in projectInfo.PrebuiltLibraries)
            {
                LibraryFiles += libraryFile + Environment.NewLine;
            }
            LibraryFiles = LibraryFiles.Trim();

            OutputType = projectInfo.OutputType;

            return Task.FromResult(true);
        }

        protected override Task DoSave()
        {
            var projectInfo = new ProjectInfo();
            projectInfo.IncludeDirectories.AddRange(FoldersInclude.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries));

            projectInfo.PrebuiltLibraries.AddRange(LibraryFiles.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries));

            projectInfo.OutputType = projectInfo.OutputType;

            var projectManager = IoC.Get<IProjectManager>();
            projectManager.Save( projectInfo, FilePath);

            return Task.FromResult(true);
        }
    }
}