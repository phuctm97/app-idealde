using Caliburn.Micro;
using Idealde.Modules.MainMenu.Models;

namespace Idealde.Modules.MainMenu.ViewModels
{
    public class MainMenuViewModel : PropertyChangedBase, IMenu
    {
        private IObservableCollection<MenuItemBase> _items { get; }
        public IObservableCollection<MenuItemBase> Items { get; }

        public MainMenuViewModel()
        {
            Items = new BindableCollection<MenuItemBase>();
        }
    }
}
