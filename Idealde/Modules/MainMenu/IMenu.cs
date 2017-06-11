
using Caliburn.Micro;
using Idealde.Modules.MainMenu.Models;

namespace Idealde.Modules.MainMenu
{
    public interface IMenu
    {
        IObservableCollection<MenuItemBase> Items { get; }
    }
}
