#region Using Namespace

using System;
using System.IO;
using System.Windows;
using Caliburn.Micro;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class NewProjectSettingsViewModel
        : Screen
    {
        private string _projectName;
        private string _projectRootDirectory;
        private bool _closing;

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

            _closing = false;

            base.OnInitialize();
        }

        public override void TryClose(bool? dialogResult = default(bool?))
        {
            if (dialogResult != null)
            {
                _closing = dialogResult.Value;
            }

            base.TryClose(dialogResult);
        }

        public override void CanClose(Action<bool> callback)
        {
            var canClose = true;

            if (_closing)
            {
                if (string.IsNullOrWhiteSpace(ProjectName))
                {
                    canClose = false;
                    MessageBox.Show(Resources.PleaseEnterProjectNameText);
                }
                else if (!Directory.Exists(ProjectRootDirectory))
                {
                    canClose = false;
                    MessageBox.Show(Resources.ProjectRootDirectoryNotExistText);
                }
            }

            callback(canClose);
        }
    }
}