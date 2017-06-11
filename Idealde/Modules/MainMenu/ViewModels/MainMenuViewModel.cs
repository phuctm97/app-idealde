using System;
using Caliburn.Micro;
using Idealde.Modules.MainMenu.Models;

namespace Idealde.Modules.MainMenu.ViewModels
{
    public class MainMenuViewModel : PropertyChangedBase, IMenu
    {
        private readonly IObservableCollection<MenuItemModel> _items;
        public IObservableCollection<MenuItemModel> Items { get { return _items; } }

        public void AddMenuItem(string text, Uri iconSource)
        {
            _items.Add(new MenuItemModel(text, iconSource));
        }

        public MainMenuViewModel()
        {
            _items = new BindableCollection<MenuItemModel>();
        }
    }
}
