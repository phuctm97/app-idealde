using System;
using System.IO;
using System.Windows;
using Caliburn.Micro;
using Idealde.Properties;

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class NewProjectSettingsViewModel
        : Screen
    {
        private string _projectName;
        private string _projectRootDirectory;

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                if (value == _projectName) return;
                _projectName = value;
                NotifyOfPropertyChange(() => ProjectName);
            }
        }

        public string ProjectRootDirectory
        {
            get { return _projectRootDirectory; }
            set
            {
                if (value == _projectRootDirectory) return;
                _projectRootDirectory = value;
                NotifyOfPropertyChange(() => ProjectRootDirectory);
            }
        }

        protected override void OnInitialize()
        {
            DisplayName = "New Project";

            ProjectName = string.Empty;

            ProjectRootDirectory = string.Empty; 

            base.OnInitialize();
        }

        public override void TryClose(bool? dialogResult = default(bool?))
        {
            if (string.IsNullOrWhiteSpace(ProjectName))
            {
                MessageBox.Show(Resources.PleaseEnterProjectNameText);
                return;
            }

            if (!Directory.Exists(ProjectRootDirectory))
            {
                MessageBox.Show(Resources.ProjectRootDirectoryNotExistText);
                return;
            }

            base.TryClose(dialogResult);
        }
    }
}