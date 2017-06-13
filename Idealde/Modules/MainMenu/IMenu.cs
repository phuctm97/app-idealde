#region Using Namespace

using Caliburn.Micro;
using Idealde.Modules.MainMenu.Models;

#endregion

namespace Idealde.Modules.MainMenu
{
    public interface IMenu
    {
        IObservableCollection<Menu> Items { get; }

        void AddMenu(params Menu[] menu);
        void AddMenuItem(Menu parent, params MenuItem[] menuItem);
        void AddMenuItem(MenuItem parent, params MenuItem[] menuItem);

        MenuItem FindMenuItemByName(Menu menu, string name);
    }
}