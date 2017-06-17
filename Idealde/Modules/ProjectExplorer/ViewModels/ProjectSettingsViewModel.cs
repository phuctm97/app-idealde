using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

using Idealde.Framework.Panes;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Modules.Settings;

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class ProjectSettingsViewModel:PersistedDocument
    {
        private string _foldersInclude;
        private string _libraryFiles;
        private string _outputType;
        public string [ ] ListOutputs
        {
            get;
            set;
        }
        public string FoldersInclude
        {
            get
            {
                return _foldersInclude;
            }
            set
            {
                if ( _foldersInclude != value )
                {
                    _foldersInclude = value;
                    NotifyOfPropertyChange(() => FoldersInclude);
                }

            }
        }

        public string LibraryFiles
        {
            get
            {
                return _libraryFiles;
            }
            set
            {
                if (_libraryFiles != value)
                {
                    _libraryFiles = value;
                    NotifyOfPropertyChange(() => LibraryFiles);
                }

            }
        }

        public string OutputType
        {
            get
            {
                return _outputType;
            }
            set
            {
                if (_outputType != value)
                {
                    _outputType = value;
                    NotifyOfPropertyChange(() => LibraryFiles);
                }

            }
        }
        

        public ProjectSettingsViewModel ( )
        {
            IoC.Get<ProjectController>().Load("E:\\chophuc.vcxproj");
            DoLoad ( );
            ListOutputs = new string [ ]
            {
                "dll",
                "exe"
            };

        }

        protected override Task DoNew ( )
        {
            return Task.FromResult(true);
        }

        protected override Task DoLoad ( )
        {
            foreach ( var folder in IoC.Get<IProjectController>().Folders )
            {
                FoldersInclude += folder + Environment.NewLine;
            }

            FoldersInclude=FoldersInclude.Trim ( );

            foreach ( var libraryFile in IoC.Get<IProjectController>().LibraryFiles )
            {
                LibraryFiles += libraryFile + Environment.NewLine;
            }

            LibraryFiles=LibraryFiles.Trim ( );

            foreach ( var outputType in IoC.Get<IProjectController>().OutputType )
            {
                OutputType = outputType;
            }

            OutputType=OutputType.Trim ( );
            
            return Task.FromResult(true);
        }

        protected override Task DoSave ( )
        {
            IoC.Get < IProjectController > ( ).Folders = FoldersInclude.Split ( new string [ ]
            {
                Environment.NewLine
            }, StringSplitOptions.None ).ToList ( );

            IoC.Get < IProjectController > ( ).LibraryFiles = LibraryFiles.Split ( new string [ ]
            {
                Environment.NewLine
            }, StringSplitOptions.None ).ToList ( );

            IoC.Get<IProjectController>().OutputType = OutputType.Split(new string[]
            {
                Environment.NewLine
            }, StringSplitOptions.None).ToList();

            IoC.Get<IProjectController>().Save(FilePath);

            return Task.FromResult(true);
        }
    }
}
