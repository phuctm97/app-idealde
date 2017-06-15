#region Using Namespace

using System;
using System.Windows.Input;

#endregion

namespace Idealde.Modules.ToolBarTray.Models
{
    public class ToolBarItemSeparator : ToolBarItemBase
    {
        public ToolBarItemSeparator(string name) : base(name)
        {
        }

        public override string Text => string.Empty;
        public override string Tooltip => string.Empty;
        public override Uri IconSource => null;
        public override KeyGesture KeyGesture => null;
        public override ICommand Command => null;
        public override bool IsShowText => false;
    }
}