#region Using Namespace

using System.Linq;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Commands;
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
            MenuItems.Clear();

            MenuItemBase parentMenuItem = null;

            foreach (var command in item.OptionCommands)
            {
                MenuItemBase newMenuItem = null;

                // fake command definition
                if (command.CommandDefinition is FakeCommandDefinition)
                {
                    // fake to add separator
                    if (command.CommandDefinition.Name == "|")
                    {
                        newMenuItem = new MenuItemSeparator(string.Empty);
                    }
                    // fake to set parent
                    else if (command.CommandDefinition.Name.Contains('|'))
                    {
                        // extract ancestors
                        var parentNames = command.CommandDefinition.Name.Trim().Split(new[] {'|'},
                            System.StringSplitOptions.RemoveEmptyEntries);

                        // find parent
                        if (parentNames.Length > 0)
                        {
                            var parentToFind = MenuItems.FirstOrDefault(p => p.Name == parentNames.First());
                            for (var i = 1; i < parentNames.Length; i++)
                            {
                                if (parentToFind == null) break;
                                parentToFind = parentToFind.Children.FirstOrDefault(p => p.Name == parentNames[i]);
                            }
                            if (parentToFind != null) parentMenuItem = parentToFind;
                        }
                    }
                    // fake to add display item
                    else
                    {
                        newMenuItem = new DisplayMenuItem(command.CommandDefinition.Name, command.Text);
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