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
            Items = new BindableCollection<MenuDefinition>();
            MenuItemNameList = new List<string>();
        }

        private List<string> MenuItemNameList { get; set; }
        public IObservableCollection<MenuDefinition> Items { get; }

        public void AddMenu(params MenuDefinition[] menu)
        {
            foreach (var menuDefinition in menu)
            {
                if (MenuItemNameList.Contains(menuDefinition.Name))
                    throw new Exception("Menu item name duplicated");
                Items.Add(menuDefinition);
                MenuItemNameList.Add(menuDefinition.Name);
            }
        }

        public void AddMenuItem(MenuDefinition parent, params MenuItemDefinition[] menuItem)
        {
            foreach (var menuItemDefinition in menuItem)
            {
                if (MenuItemNameList.Contains(menuItemDefinition.Name))
                    throw new Exception("Menu item name duplicated");
                parent.Childrens.Add(menuItemDefinition);
                MenuItemNameList.Add(menuItemDefinition.Name);

            }
        }

        public void AddMenuItem(MenuItemDefinition parent, params MenuItemDefinition[] menuItem)
        {
            foreach (var menuItemDefinition in menuItem)
            {
                if (MenuItemNameList.Contains(menuItemDefinition.Name))
                    throw new Exception("Menu item name duplicated");
                parent.Childrens.Add(menuItemDefinition);
                MenuItemNameList.Add(menuItemDefinition.Name);
            }
        }

        public MenuItemDefinition FindMenuItemByName(MenuDefinition menu, string name)
        {
            if (!MenuItemNameList.Contains(name)) return null;
            foreach (var child in menu.Childrens)
            {
                if (child.Name == name) return child;
                return FindRecursive(child, name);
            }
            return null;
        }

        private MenuItemDefinition FindRecursive(MenuItemDefinition menuItem, string name)
        {
            foreach (var child in menuItem.Childrens)
            {
                if (menuItem.Name == name) return menuItem;
                return FindRecursive(child, name);
            }
            return null;
        }

    }
}