#region Using Namespace

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Framework.Projects;
using Idealde.Modules.CodeEditor.Commands;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Commands;
using Idealde.Modules.ProjectExplorer.Models;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class ProjectExplorerViewModel : Tool, IProjectExplorer,
        ICommandHandler<NewCppHeaderCommandDefinition>,
        ICommandHandler<RemoveFileCommandDefinition>,
        ICommandHandler<NewCppSourceCommandDefinition>
    {
        // Backing fields

        #region Backing fields

        private ProjectItemBase _selectedItem;
        private ProjectInfo _projectInfo;

        #endregion

        // Bind properties

        #region Bind properties

        public override PaneLocation PreferredLocation => PaneLocation.Right;

        #endregion

        // Initializations

        #region Initializations

        public ProjectExplorerViewModel()
        {
            DisplayName = "Project Explorer";
            Items = new BindableCollection<ProjectItemBase>();
            MenuItems = new BindableCollection<MenuItemBase>();
        }

        #endregion

        // Bind models

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

        // Behaviors

        #region Behaviors

        public ProjectInfo ProjectInfo
        {
            get { return _projectInfo; }
            set
            {
                _projectInfo = value;
                Refresh();
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

        private void Refresh()
        {
            Items.Clear();
        }

        public void Load(string path)
        {
            Refresh();
        }

        public void Clear()
        {
            Items.Clear();
        }

        #endregion

        // Command handlers

        #region Command handlers

        Task ICommandHandler<NewCppSourceCommandDefinition>.Run(Command command)
        {
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
            return Task.FromResult(true);
        }

        Task ICommandHandler<RemoveFileCommandDefinition>.Run(Command command)
        {
            // delete children
            File.Delete((string) SelectedItem.Tag);
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
    }
}