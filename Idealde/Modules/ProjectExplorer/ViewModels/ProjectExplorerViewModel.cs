#region Using Namespace

using System.IO;
using System.Windows.Input;
using Caliburn.Micro;
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

            MenuItems = new BindableCollection<MenuItemBase>()
            {
                new DisplayMenuItem("OpenMenu","Open")
            };
        }


        public IObservableCollection<ProjectItemBase> Items { get; }

        public void PopulateMenu(ProjectItemBase item)
        {
            MenuItems.Clear();
            foreach (var command in item.OptionCommands)
            {
                MenuItems.Add(new CommandMenuItem(string.Empty, command));
            }
        }

        public IObservableCollection<MenuItemBase> MenuItems { get; }

    }
}