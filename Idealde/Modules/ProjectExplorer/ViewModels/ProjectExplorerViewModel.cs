#region Using Namespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;
using Idealde.Framework.Services;
using Idealde.Modules.CodeEditor;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Commands;
using Idealde.Modules.ProjectExplorer.Models;
using Idealde.Modules.Shell.Commands;
using Idealde.Properties;
using Microsoft.Win32;
using FileInfo = Idealde.Framework.Projects.FileInfo;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class ProjectExplorerViewModel : Tool, IProjectExplorer,
        ICommandHandler<AddFolderToProjectCommandDefinition>,
        ICommandHandler<AddNewCppHeaderToProjectCommandDefinition>,
        ICommandHandler<AddNewCppSourceToProjectCommandDefinition>,
        ICommandHandler<AddExistingFileToProjectCommandDefinition>,
        ICommandHandler<RemoveFileCommandDefinition>,
        ICommandHandler<SaveFileCommandDefinition>
    {
        // Backing fields

        #region Backing fields

        private ProjectItemBase _selectedItem;
        private ProjectInfoBase _currentProjectInfo;

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

        public ProjectExplorerViewModel()
        {
            DisplayName = "Project Explorer";
            ProjectItems = new BindableCollection<ProjectItemBase>();
            MenuItems = new BindableCollection<MenuItemBase>();
        }

        #endregion

        // Behaviors

        #region Behaviors

        private bool _projectIsDirty;

        public ProjectInfoBase CurrentProjectInfo
        {
            get { return _currentProjectInfo; }
            set
            {
                if (Equals(_currentProjectInfo, value)) return;
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
                    command.Tag = item;
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
            _projectIsDirty = false;
            ProjectItems.Clear();
            ProjectItems.Add(CurrentProjectInfo.ProjectItem);

            foreach (var fileInfo in CurrentProjectInfo.Files)
            {
                var parent = GenerateAndGetFolder(fileInfo);
                if (parent == null) continue;

                var fileName = Path.GetFileName(fileInfo.RealPath);
                parent.AddChild(new ProjectItem<FileProjectItemDefinition>
                {
                    Text = fileName,
                    Tag = fileInfo.RealPath
                });
            }
        }

        private ProjectItemBase GenerateAndGetFolder(FileInfo fileInfo)
        {
            var parentNames = fileInfo.VirtualPath.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
            if (parentNames.Length == 0) return CurrentProjectInfo.ProjectItem;

            // find first item
            var parentItem = CurrentProjectInfo.ProjectItem.Children.FirstOrDefault(i =>
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
                CurrentProjectInfo.ProjectItem.AddChild(parentItem);
            }

            for (var i = 1; i < parentNames.Length; i++)
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
                    parentItem.AddChild(childItem);
                }

                parentItem = childItem;
            }

            return parentItem;
        }

        public void LoadProject(string path, IProjectProvider provider)
        {
            if (provider == null) return;

            // load new project
            var newProjectInfo = provider.Load(path);

            // new project not exist
            if (string.IsNullOrWhiteSpace(newProjectInfo?.Path))
            {
                MessageBox.Show(Resources.ProjectFileNotExistText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // confirm close old project
            if (CurrentProjectInfo != null)
            {
                if (!ConfirmCloseCurrentProject()) return;
                CloseCurrentProject();
            }

            // update current project
            CurrentProjectInfo = newProjectInfo;
        }

        private bool ConfirmCloseCurrentProject()
        {
            var result =
                MessageBox.Show(string.Format(Resources.AreYouWantToCloseProjectText, CurrentProjectInfo.ProjectName),
                    "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                return true;
            return false;
        }

        public void CloseCurrentProject()
        {
            if (_projectIsDirty)
            {
                var result =
                    MessageBox.Show(string.Format(Resources.AskForSaveFileBeforeExit, CurrentProjectInfo.ProjectName),
                        "Confirm", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    SaveCurrentProject();
                }
            }

            ProjectItems.Clear();
        }

        private void SaveCurrentProject()
        {
            // reset files
            CurrentProjectInfo.Files.Clear();

            var stack = new Stack<ProjectItemBase>();

            stack.Push(CurrentProjectInfo.ProjectItem);
            while (stack.Count > 0)
            {
                var parentItem = stack.Pop();
                if (parentItem == null) continue;

                foreach (var item in parentItem.Children.OfType<ProjectItem>())
                {
                    if (item.ProjectItemDefintion is FileProjectItemDefinition)
                    {
                        var virtualPath = TravVirtualPath(item);
                        var fileName = item.Tag as string;
                        if (string.IsNullOrWhiteSpace(fileName)) continue;

                        CurrentProjectInfo.Files.Add(new FileInfo(virtualPath, fileName));
                    }
                    else if (item.ProjectItemDefintion is FolderProjectItemDefinition)
                    {
                        stack.Push(item);
                    }
                }
            }

            var provider = CurrentProjectInfo.Provider;
            provider.Save(CurrentProjectInfo, CurrentProjectInfo.Path);

            _projectIsDirty = false;
        }

        void ICommandHandler<AddFolderToProjectCommandDefinition>.Update(Command command)
        {
        }

        Task ICommandHandler<AddFolderToProjectCommandDefinition>.Run(Command command)
        {
            // get project item active this command
            var item = command.Tag as ProjectItem;
            if (item == null) return Task.FromResult(false);

            // show dialog for new name
            var dialog = IoC.Get<NewItemViewModel>();
            var windowManager = IoC.Get<IWindowManager>();
            var result = windowManager.ShowDialog(dialog) ?? false;
            if (!result) return Task.FromResult(false);

            // check duplicated name
            var newItemName = dialog.Name.Trim();
            if (item.Children.Any(p => string.Equals(p.Text, newItemName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(Resources.TheSameNameItemExistText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return Task.FromResult(false);
            }

            // create new item
            var newItem = new ProjectItem<FolderProjectItemDefinition> {Text = newItemName};
            item.AddChild(newItem);

            item.IsOpen = true;
            SelectedItem = newItem;
            return Task.FromResult(true);
        }

        void ICommandHandler<AddNewCppHeaderToProjectCommandDefinition>.Update(Command command)
        {
        }

        async Task ICommandHandler<AddNewCppHeaderToProjectCommandDefinition>.Run(Command command)
        {
            // get project item active this command
            var item = command.Tag as ProjectItem;
            if (item == null) return;

            await AddNewFile(item, ".h");
        }

        void ICommandHandler<AddNewCppSourceToProjectCommandDefinition>.Update(Command command)
        {
        }

        async Task ICommandHandler<AddNewCppSourceToProjectCommandDefinition>.Run(Command command)
        {
            // get project item active this command
            var item = command.Tag as ProjectItem;
            if (item == null) return;

            await AddNewFile(item, ".cpp");
        }

        private async Task AddNewFile(ProjectItem parent, string extension)
        {
            // show dialog for new name
            var dialog = IoC.Get<NewItemViewModel>();
            var windowManager = IoC.Get<IWindowManager>();
            var result = windowManager.ShowDialog(dialog) ?? false;
            if (!result) return;

            // check duplicated name
            var newItemName = dialog.Name.Trim();
            if (!newItemName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
            {
                newItemName += extension;
            }
            var newItemPath = Path.GetDirectoryName(CurrentProjectInfo.Path) + "\\" + newItemName;

            if (parent.Children.Any(p => string.Equals(p.Text, newItemName, StringComparison.OrdinalIgnoreCase))
                || File.Exists(newItemPath))
            {
                MessageBox.Show(Resources.TheSameNameItemExistText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // create new file
            var newFile = File.Create(newItemPath);
            newFile?.Close();

            // create new item
            var newItem = new ProjectItem<FileProjectItemDefinition>
            {
                Text = newItemName,
                Tag = newItemPath
            };
            parent.AddChild(newItem);
            _projectIsDirty = true;

            parent.IsOpen = true;
            SelectedItem = newItem;

            // open document
            var editor = IoC.Get<ICodeEditor>();
            await editor.Load(newItemPath);

            var shell = IoC.Get<IShell>();
            shell.OpenDocument(editor);
        }

        void ICommandHandler<AddExistingFileToProjectCommandDefinition>.Update(Command command)
        {
        }

        Task ICommandHandler<AddExistingFileToProjectCommandDefinition>.Run(Command command)
        {
            // get project item active this command
            var item = command.Tag as ProjectItem;
            if (item == null) return Task.FromResult(false);

            return AddExistingFile(item);
        }

        private async Task AddExistingFile(ProjectItem parent)
        {
            // show dialog to open files
            var dialog = new OpenFileDialog {Multiselect = true, CheckFileExists = true, CheckPathExists = true};
            var result = dialog.ShowDialog() ?? false;
            if (!result) return;

            // first file added to open
            ProjectItem firstItem = null;

            // add all selected files
            foreach (var fileName in dialog.FileNames)
            {
                if (string.IsNullOrWhiteSpace(fileName)) continue;

                // check duplicated name
                if (TravAllItems(fileName) != null)
                {
                    MessageBox.Show(Resources.TheSameNameItemExistText, "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    continue;
                }

                // create and add new item
                var newItem = new ProjectItem<FileProjectItemDefinition>
                {
                    Text = Path.GetFileName(fileName),
                    Tag = fileName
                };
                parent.AddChild(newItem);
                _projectIsDirty = true;

                if (firstItem == null)
                {
                    firstItem = newItem;
                }
            }

            if (firstItem != null)
            {
                parent.IsOpen = true;
                SelectedItem = firstItem;

                var editor = IoC.Get<ICodeEditor>();
                await editor.Load(firstItem.Tag as string);

                var shell = IoC.Get<IShell>();
                shell.OpenDocument(editor);
            }
        }

        private ProjectItem TravAllItems(string tag)
        {
            tag = tag.Trim().TrimEnd('\\');

            var stack = new Stack<ProjectItem>();
            foreach (var item in ProjectItems.OfType<ProjectItem>())
            {
                var itemTag = item.Tag as string;
                if (string.Equals(itemTag?.Trim().TrimEnd('\\'), tag, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }

                stack.Push(item);
            }

            while (stack.Count > 0)
            {
                var parentItem = stack.Pop();
                foreach (var item in parentItem.Children.OfType<ProjectItem>())
                {
                    var itemTag = item.Tag as string;
                    if (string.Equals(itemTag?.Trim().TrimEnd('\\'), tag, StringComparison.OrdinalIgnoreCase))
                    {
                        return item;
                    }

                    stack.Push(item);
                }
            }

            return null;
        }

        private string TravVirtualPath(ProjectItemBase item)
        {
            var path = string.Empty;
            while (item.Parent != null && item.Parent != CurrentProjectInfo.ProjectItem)
            {
                path = item.Parent.Text.Trim() + "\\" + path;
                item = item.Parent;
            }

            return path;
        }

        void ICommandHandler<RemoveFileCommandDefinition>.Update(Command command)
        {
        }

        Task ICommandHandler<RemoveFileCommandDefinition>.Run(Command command)
        {
            // get project item active this command
            var item = command.Tag as ProjectItem;
            if (item == null) return Task.FromResult(false);

            var parent = item.Parent;
            if (parent == null) return Task.FromResult(false);

            if (item.ProjectItemDefintion is FolderProjectItemDefinition && ConfirmRemoveFolder())
            {
                parent.Children.Remove(item);
                if (item.Children.Count > 0)
                {
                    _projectIsDirty = true;
                }
            }
            else if (item.ProjectItemDefintion is FileProjectItemDefinition && ConfirmRemoveFile(item.Text))
            {
                parent.Children.Remove(item);
                _projectIsDirty = true;
            }

            return Task.FromResult(true);
        }

        private bool ConfirmRemoveFile(string name)
        {
            var result = MessageBox.Show(
                string.Format(Resources.DoYouWantToRemoveText, name), "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) return true;
            return false;
        }

        private bool ConfirmRemoveFolder()
        {
            var result = MessageBox.Show(Resources.DoYouWantToRemoveFolderText, "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) return true;
            return false;
        }

        void ICommandHandler<SaveFileCommandDefinition>.Update(Command command)
        {
            command.Text = Resources.ProjectSaveCommandText;
            command.Tooltip = Resources.ProjectSaveCommandTooltip;
            if (string.IsNullOrWhiteSpace(command.Tooltip)) command.Tooltip = command.Text;

            command.IsEnabled = CurrentProjectInfo != null && _projectIsDirty;
        }

        Task ICommandHandler<SaveFileCommandDefinition>.Run(Command command)
        {
            SaveCurrentProject();
            return Task.FromResult(true);
        }

        #endregion
    }
}