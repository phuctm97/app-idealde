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
        void AddMenuItem(Menu parent, params MenuItemBase[] displayMenuItem);
        void AddMenuItem(MenuItemBase parent, params MenuItemBase[] displayMenuItem);

        MenuItemBase FindMenuItem(Menu menu, string name);
        Menu FindMenu(string name);
    }
}