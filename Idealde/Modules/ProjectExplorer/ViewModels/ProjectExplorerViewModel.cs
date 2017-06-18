#region Using Namespace

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;
using Idealde.Modules.CodeEditor;
using Idealde.Modules.CodeEditor.Commands;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Commands;
using Idealde.Modules.ProjectExplorer.Models;
using Idealde.Modules.Shell.Commands;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class ProjectExplorerViewModel : Tool, IProjectExplorer,
        ICommandHandler<NewCppHeaderCommandDefinition>, ICommandHandler<RemoveFileCommandDefinition>,
        ICommandHandler<NewCppSourceCommandDefinition>
    {
        #region backing field
        private ProjectItemBase _selectedItem;
        #endregion

        #region Initialization
        public override PaneLocation PreferredLocation => PaneLocation.Right;

        public ProjectExplorerViewModel()
        {
            DisplayName = "Solution Explorer";
            Items = new BindableCollection<ProjectItemBase>();
            MenuItems = new BindableCollection<MenuItemBase>();
        }
        #endregion

        #region Bind models
        public IObservableCollection<ProjectItemBase> Items { get; }
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

        #region  Command Handler

        Task ICommandHandler<NewCppSourceCommandDefinition>.Run(Command command)
        {
            AddNewFile("Create source file", ".cpp");
            return Task.FromResult(true);
        }


        void ICommandHandler<NewCppSourceCommandDefinition>.Update(Command command)
        {
            // Nothing to do
        }

        void ICommandHandler<NewCppHeaderCommandDefinition>.Update(Command command)
        {
            // nothing to do
        }

        Task ICommandHandler<NewCppHeaderCommandDefinition>.Run(Command command)
        {
            AddNewFile("Create new header", ".h");
            return Task.FromResult(true);
        }

        Task ICommandHandler<RemoveFileCommandDefinition>.Run(Command command)
        {
            
            // delete children
            File.Delete((string)SelectedItem.Tag);
            SelectedItem.Children.Clear();

            // delete file from project tree
            //????????????????????????????????????


            return Task.FromResult(true);
        }

        void ICommandHandler<RemoveFileCommandDefinition>.Update(Command command)
        {
            // nothing to do
        }

        #endregion

        #region Methods
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

                        // find parent
                        if (parentNames.Length == 0)
                        {
                            parentMenuItem = null;
                        }
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

        private void AddNewFile(string work, string fileExtension)
        {
            var selectedItem = SelectedItem as ProjectItem;

            if (!(selectedItem?.ProjectItemDefintion is FolderProjectItemDefinition)) return;

            var folderPath = selectedItem.Tag as string;

            // itit new input window
            var userInputWindow = new UserInputViewModel(work, folderPath, fileExtension);

            // Show window enter file name
            var window = IoC.Get<IWindowManager>();
            window.ShowDialog(userInputWindow);

            // Check new file name is empty?
            if (userInputWindow.FileName == string.Empty) return;

            var fileName = userInputWindow.FileName + fileExtension;
            var filePath = folderPath + "\\" + fileName;

            // check new file exist (AGAIN) ( after checked in input window )
            if (File.Exists(filePath)) return;

            // create new file
            using (var sw = File.Create(filePath))
            {
            }

            // add file to project tree
            selectedItem.Children.Add(
                new ProjectItem<FileProjectItemDefinition> { Text = fileName, Tag = filePath });
            selectedItem.IsOpen = true;

            // add editor
            var editor = IoC.Get<ICodeEditor>();
            editor.New(fileName);
            var shell = IoC.Get<IShell>();
            shell.OpenDocument(editor);
        }
<<<<<<< HEAD

        public IObservableCollection < ProjectItemBase > Items
        {
            get;
        }
=======
        #endregion

>>>>>>> master
    }
}