#region Using Namespace

using System;
using System.Windows.Input;

#endregion

namespace Idealde.Modules.ToolBarTray.Models
{
    public class DisplayToolBarItem : ToolBarItemBase
    {
        // Bind properties 

        #region Bind properties

        public override string Text { get; }

        public override string Tooltip { get; }

        public override Uri IconSource { get; }

        public override KeyGesture KeyGesture => null;

        public override ICommand Command => null;

        public override bool IsShowText { get; }

        #endregion

        // Initializations

        #region Initializations

        public DisplayToolBarItem(string name, string text, string tooltip = "", Uri iconSource = null, bool showText = false)
            : base(name)
        {
            Text = text;
            IsShowText = showText;
            Tooltip = tooltip;
            IconSource = iconSource;
        }


        #endregion
    }
}