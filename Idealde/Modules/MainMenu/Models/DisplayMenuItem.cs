#region Using Namespace

using System;
using System.Windows.Input;

#endregion

namespace Idealde.Modules.MainMenu.Models
{
    public class DisplayMenuItem : MenuItemBase
    {
        // Bind properties 

        #region Bind properties

        public override string Text { get; }

        public override string Tooltip { get; }

        public override Uri IconSource { get; }

        public override KeyGesture KeyGesture => null;

        public override ICommand Command => null;

        #endregion

        // Initializations

        #region Initializations

        public DisplayMenuItem(string name, string text, string tooltip = "", Uri iconSource = null)
            : base(name)
        {
            Text = text;
            Tooltip = tooltip;
            IconSource = iconSource;
        }

        #endregion
    }
}