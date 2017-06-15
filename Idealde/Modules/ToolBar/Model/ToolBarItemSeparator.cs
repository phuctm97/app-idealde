using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Idealde.Modules.ToolBar.Model
{
    public class ToolBarItemSeparator:ToolBarItem
    {
        public ToolBarItemSeparator(string name) : base(name)
        {
        }

        public override string Text => string.Empty;
        public override string Tooltip => string.Empty;
        public override Uri IconSource => null;
        public override KeyGesture KeyGesture => null;
        public override ICommand Command => null;
    }
}
