using Caliburn.Micro;
using Idealde.Modules.MainMenu.Models;

namespace Idealde.Modules.MainMenu
{
    public interface IMenu
    {
        IObservableCollection<MenuDefinition> Items { get; }

        void AddMenu(params MenuDefinition[] menu);

        void AddMenuItem(MenuDefinition parent, params MenuItemDefinition[] menuItem);
    }
}
