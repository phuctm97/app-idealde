#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;

#endregion

namespace Idealde.Modules.MainMenu.Models
{
    public class DisplayMenuItem : MenuItemBase
    {
        public override string Text { get; }

        public override string Tooltip { get; }

        public override Uri IconSource { get; }

        public override KeyGesture KeyGesture => null;

        public override ICommand Command => null;

        public DisplayMenuItem(string name, string text, string tooltip = "", Uri iconSource = null)
            : base(name)
        {
            Text = text;
            Tooltip = tooltip;
            IconSource = iconSource;
        }
    }
}