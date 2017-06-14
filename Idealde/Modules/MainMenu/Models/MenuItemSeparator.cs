using System;
using System.Windows.Input;

namespace Idealde.Modules.MainMenu.Models
{
    public class MenuItemSeparator : MenuItemBase
    {
        public MenuItemSeparator(string name) : base(name)
        {
        }

        public override string Text => string.Empty;
        public override string Tooltip => string.Empty;
        public override Uri IconSource => null;
        public override KeyGesture KeyGesture => null;
        public override ICommand Command => null;
    }
}