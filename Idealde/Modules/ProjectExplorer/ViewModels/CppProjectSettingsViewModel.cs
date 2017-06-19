#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Modules.ProjectExplorer.Providers;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public sealed class CppProjectSettingsViewModel : PersistedDocument, IPersistedDocument
    {
        private string _foldersInclude;
        private string _libraryFiles;
        private CppProjectOutputType _outputType;
        public IEnumerable<CppProjectOutputType> ListOutputs { get; }

        public string FoldersInclude
        {
            get { return _foldersInclude; }
            set
            {
                if (_foldersInclude != value)
                {
                    _foldersInclude = value;
                    NotifyOfPropertyChange(() => FoldersInclude);
                    IsDirty = true;
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
                    IsDirty = true;
                }
            }
        }

        public CppProjectOutputType OutputType
        {
            get { return _outputType; }
            set
            {
                if (_outputType != value)
                {
                    _outputType = value;
                    NotifyOfPropertyChange(() => LibraryFiles);
                    IsDirty = true;
                }
            }
        }

        public CppProjectSettingsViewModel()
        {
            ListOutputs = Enum.GetValues(typeof(CppProjectOutputType)).Cast<CppProjectOutputType>();
            LibraryFiles = string.Empty;
            FoldersInclude = string.Empty;
            OutputType = CppProjectOutputType.Dll;
        }

        protected override Task DoNew()
        {
            return Task.FromResult(true);
        }

        protected override Task DoLoad()
        {
            var provider = IoC.Get<CppProjectProvider>();
            var projectInfo = (CppProjectInfo) provider.Load(FilePath);

            if (string.IsNullOrWhiteSpace(projectInfo?.Path)) return Task.FromResult(false);

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

        protected override async Task DoSave()
        {
            var projectExplorer = IoC.Get<IProjectExplorer>();

            var projectInfo = (CppProjectInfo) projectExplorer.CurrentProjectInfo;

            projectInfo.IncludeDirectories.Clear();
            projectInfo.IncludeDirectories.AddRange(FoldersInclude.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries));

            projectInfo.PrebuiltLibraries.Clear();
            projectInfo.PrebuiltLibraries.AddRange(LibraryFiles.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries));

            projectInfo.OutputType = projectInfo.OutputType;

            var provider = IoC.Get<CppProjectProvider>();
            await provider.Save(projectInfo, FilePath);
        }
    }
}