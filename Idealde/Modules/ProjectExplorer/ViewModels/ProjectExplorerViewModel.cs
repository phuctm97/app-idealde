#region Using Namespace

using System;
using System.IO;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Models;
using Idealde.Properties;
using FileInfo = Idealde.Framework.Projects.FileInfo;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class ProjectExplorerViewModel : Tool, IProjectExplorer
    {
        // Dependencies

        #region Dependencies

        private readonly IProjectManager _projectManager;

        #endregion

        // Backing fields

        #region Backing fields

        private ProjectItemBase _selectedItem;
        private ProjectInfo _currentProjectInfo;

        #endregion

        // Bind properties

        #region Bind properties

        public override PaneLocation PreferredLocation => PaneLocation.Right;

        #endregion

        // Bind models

        #region Bind models

        public IObservableCollection<ProjectItemBase> ProjectItems { get; }

        public IObservableCollection<MenuItemBase> MenuItems { get; }

        public ProjectItemBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        #endregion

        // Initializations

        #region Initializations

        public ProjectExplorerViewModel(IProjectManager projectManager)
        {
            _projectManager = projectManager;
            DisplayName = "Project Explorer";
            ProjectItems = new BindableCollection<ProjectItemBase>();
            MenuItems = new BindableCollection<MenuItemBase>();
        }

        #endregion

        // Behaviors

        #region Behaviors

        public ProjectInfo CurrentProjectInfo
        {
            get { return _currentProjectInfo; }
            set
            {
                if (value == null || Equals(_currentProjectInfo, value)) return;
                _currentProjectInfo = value;
                RefreshProject();
            }
        }

        public void PopulateMenu(ProjectItemBase item)
        {
            if (item == null) return;

            MenuItems.Clear();
            MenuItemBase parentMenuItem = null;

            foreach (var command in item.OptionCommands)
            {
                MenuItemBase newMenuItem = null;

                // fake command definition
                if (command.CommandDefinition is FakeCommandDefinition)
                {
                    // reset parent
                    if (string.IsNullOrEmpty(command.CommandDefinition.Name))
                    {
                        parentMenuItem = null;
                    }
                    // fake to add separator
                    else if (command.CommandDefinition.Name == "|")
                    {
                        newMenuItem = new MenuItemSeparator(string.Empty);
                    }
                    // fake to set parent
                    else if (command.CommandDefinition.Name.Contains('|'))
                    {
                        // extract ancestors
                        var parentNames = command.CommandDefinition.Name.Trim().Split(new[] {'|'},
                            StringSplitOptions.RemoveEmptyEntries);

                        // reset parent
                        if (parentNames.Length == 0)
                        {
                            parentMenuItem = null;
                        }
                        // find parent
                        else
                        {
                            parentMenuItem = MenuItems.FirstOrDefault(p => p.Name == parentNames.First());
                            if (parentMenuItem == null)
                            {
                                parentMenuItem = new DisplayMenuItem(parentNames.First(), parentNames.First());
                                MenuItems.Add(parentMenuItem);
                            }

                            for (var i = 1; i < parentNames.Length; i++)
                            {
                                var nextMenuItem =
                                    parentMenuItem.Children.FirstOrDefault(p => p.Name == parentNames[i]);
                                if (nextMenuItem == null)
                                {
                                    nextMenuItem = new DisplayMenuItem(parentNames[i], parentNames[i]);
                                    parentMenuItem.Children.Add(nextMenuItem);
                                }
                                parentMenuItem = nextMenuItem;
                            }
                        }
                    }
                }
                else
                {
                    newMenuItem = new CommandMenuItem(string.Empty, command);
                }

                if (newMenuItem == null) continue;

                if (parentMenuItem != null)
                    parentMenuItem.Children.Add(newMenuItem);
                else
                    MenuItems.Add(newMenuItem);
            }
        }

        private void RefreshProject()
        {
            ProjectItems.Clear();

            // TODO: update name

            foreach (var fileInfo in CurrentProjectInfo.Files)
            {
                var parent = GenerateAndGetFolder(fileInfo);
                if (parent == null) continue;

                var fileName = Path.GetFileName(fileInfo.RealPath);
                parent.Children.Add(new ProjectItem<FileProjectItemDefinition>
                {
                    Text = fileName,
                    Tag = fileInfo.RealPath
                });
            }
        }

        private ProjectItemBase GenerateAndGetFolder(FileInfo fileInfo)
        {
            var parentNames = fileInfo.VirtualPath.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
            if (parentNames.Length == 0) return null;

            // find first item
            var parentItem = ProjectItems.FirstOrDefault(i =>
            {
                var item = i as ProjectItem;
                if (!(item?.ProjectItemDefintion is FolderProjectItemDefinition)) return false;

                return string.Equals(item.Text, parentNames.First(), StringComparison.OrdinalIgnoreCase);
            });

            // create if not exist
            if (parentItem == null)
            {
                parentItem = new ProjectItem<FolderProjectItemDefinition>
                {
                    Text = parentNames.First()
                };
                ProjectItems.Add(parentItem);
            }

            for (var i = 1; i < parentNames.Length - 1; i++)
            {
                // find next item
                var childItem = parentItem.Children.FirstOrDefault(it =>
                {
                    var item = it as ProjectItem;
                    if (!(item?.ProjectItemDefintion is FolderProjectItemDefinition)) return false;

                    return string.Equals(item.Text, parentNames.First(), StringComparison.OrdinalIgnoreCase);
                });

                // create if not exist
                if (childItem == null)
                {
                    childItem = new ProjectItem<FolderProjectItemDefinition>
                    {
                        Text = parentNames[i]
                    };
                    parentItem.Children.Add(childItem);
                }

                parentItem = childItem;
            }

            return parentItem;
        }

        public void LoadProject(string path)
        {
            // load new project
            var newProjectInfo = _projectManager.Load(path);

            // TODO
            newProjectInfo.Path = path;

            // new project not exist
            if (string.IsNullOrWhiteSpace(newProjectInfo.Path))
            {
                MessageBox.Show(Resources.ProjectFileNotExistText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // confirm close old project
            if (CurrentProjectInfo != null)
            {
                if (!ConfirmCloseCurrentProject()) return;
            }

            // update current project
            CurrentProjectInfo = newProjectInfo;
        }

        private bool ConfirmCloseCurrentProject()
        {
            var result = MessageBox.Show(string.Format(Resources.AreYouWantToCloseProjectText, "<ProjectName>"),
                "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                return true;
            return false;
        }

        public void CloseCurrentProject()
        {
            ProjectItems.Clear();
        }

        #endregion
    }
}