using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Models;

namespace Idealde.Modules.ProjectExplorer
{
    public interface IProjectExplorer : ITool
    {
        IObservableCollection<ProjectItemBase> Items { get; }
        IObservableCollection<MenuItemBase> MenuItems { get; }
        void PopulateMenu(ProjectItemBase item);
    }
}