using System.Windows;
using System.Windows.Controls;
using Idealde.Modules.MainMenu.Models;

namespace Idealde.Modules.MainMenu.Controls
{
    public class MenuItemStyleSelector : StyleSelector
    {
        public Style SeparatorStyle { get; set; }

        public Style MenuItemStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is MenuItemSeparator)
            {
                return SeparatorStyle;
            }

            if (item is MenuItemBase)
            {
                return MenuItemStyle;
            }

            return base.SelectStyle(item, container);
        }

    }
}