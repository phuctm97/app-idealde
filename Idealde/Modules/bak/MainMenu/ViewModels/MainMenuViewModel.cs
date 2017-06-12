using System;
using Caliburn.Micro;
using Idealde.Modules.MainMenu.Models;

namespace Idealde.Modules.MainMenu.ViewModels
{
    public class MainMenuViewModel : PropertyChangedBase, IMenu
    {
        private readonly IObservableCollection<MenuDefinition> _items;
        public IObservableCollection<MenuDefinition> Items { get { return _items; } }

        public void AddMenu(params MenuDefinition[] menu)
        {
            foreach (var menuDefinition in menu)
            {
                _items.Add(menuDefinition);
            }
        }

        public void AddMenuItem(MenuDefinition parent, params MenuItemDefinition[] menuItem)
        {
            foreach (var menuItemDefinition in menuItem)
            {
                parent.Childrens.Add(menuItemDefinition);
            }
        }

        public MainMenuViewModel()
        {
            _items = new BindableCollection<MenuDefinition>();
        }
    }
}
