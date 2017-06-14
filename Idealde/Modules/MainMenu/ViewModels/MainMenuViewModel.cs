using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Idealde.Modules.MainMenu.Models;

namespace Idealde.Modules.MainMenu.ViewModels
{
    public class MainMenuViewModel : PropertyChangedBase, IMenu
    {
        public MainMenuViewModel()
        {
            Items = new BindableCollection<Menu>();
            MenuItemNameList = new List<string>();
        }

        private List<string> MenuItemNameList { get; set; }
        public IObservableCollection<Menu> Items { get; }

        public void AddMenu(params Menu[] menu)
        {
            foreach (var menuDefinition in menu)
            {
                if (MenuItemNameList.Contains(menuDefinition.Name))
                    throw new Exception("Menu item name duplicated");
                Items.Add(menuDefinition);
                MenuItemNameList.Add(menuDefinition.Name);
            }
        }

        public void AddMenuItem(Menu parent, params MenuItemBase[] displayMenuItem)
        {
            foreach (var menuItemDefinition in displayMenuItem)
            {
                if (MenuItemNameList.Contains(menuItemDefinition.Name))
                    throw new Exception("Menu item name duplicated");
                parent.Children.Add(menuItemDefinition);
                MenuItemNameList.Add(menuItemDefinition.Name);

            }
        }

        public void AddMenuItem(MenuItemBase parent, params MenuItemBase[] displayMenuItem)
        {
            foreach (var menuItemDefinition in displayMenuItem)
            {
                if (MenuItemNameList.Contains(menuItemDefinition.Name))
                    throw new Exception("Menu item name duplicated");
                parent.Children.Add(menuItemDefinition);
                MenuItemNameList.Add(menuItemDefinition.Name);
            }
        }

        private MenuItemBase FindRecursive(MenuItemBase displayMenuItem, string name)
        {
            foreach (var child in displayMenuItem.Children)
            {
                if (displayMenuItem.Name == name) return displayMenuItem;
                return FindRecursive(child, name);
            }
            return null;
        }

        public MenuItemBase FindMenuItem(Menu menu, string name)
        {
            if (!MenuItemNameList.Contains(name)) return null;
            foreach (var child in menu.Children)
            {
                if (child.Name == name) return child;
                return FindRecursive(child, name);
            }
            return null;
        }

        public Menu FindMenu(string name)
        {
            throw new NotImplementedException();
        }
    }
}