﻿#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;
using Idealde.Modules.ProjectExplorer.Providers;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class CppProjectSettingsViewModel : PersistedDocument
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

        public CppProjectOutputType OutputType
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

        public CppProjectSettingsViewModel()
        {
            ListOutputs = Enum.GetValues(typeof(CppProjectOutputType)).Cast<CppProjectOutputType>();
        }

        protected override Task DoNew()
        {
            return Task.FromResult(true);
        }

        protected override Task DoLoad()
        {
            var provider = IoC.Get<CppProjectProvider>();
            CppProjectInfo projectInfo = (CppProjectInfo) provider.Load(FilePath);

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

        protected override Task DoSave()
        {
            var projectInfo = new CppProjectInfo();
            projectInfo.IncludeDirectories.AddRange(FoldersInclude.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries));

            projectInfo.PrebuiltLibraries.AddRange(LibraryFiles.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries));

            projectInfo.OutputType = projectInfo.OutputType;

            var provider = IoC.Get<CppProjectProvider>();
            return provider.Save( projectInfo, FilePath);
        }
    }
}