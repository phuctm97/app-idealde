#region Using Namespace

using System;
using System.Linq;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Models;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class ProjectExplorerViewModel : Tool, IProjectExplorer
    {
        public override PaneLocation PreferredLocation => PaneLocation.Right;


        public ProjectExplorerViewModel()
        {
            DisplayName = "Solution Explorer";
            Items = new BindableCollection<ProjectItemBase>();

            MenuItems = new BindableCollection<MenuItemBase>
            {
                new DisplayMenuItem("OpenMenu", "Open")
            };
        }


        public IObservableCollection<ProjectItemBase> Items { get; }

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
                                var nextMenuItem = parentMenuItem.Children.FirstOrDefault(p => p.Name == parentNames[i]);
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

                if (newMenuItem == null) continue;

                if (parentMenuItem != null)
                {
                    parentMenuItem.Children.Add(newMenuItem);
                }
                else
                {
                    MenuItems.Add(newMenuItem);
                }
            }
        }

        public IObservableCollection<MenuItemBase> MenuItems { get; }
    }
}