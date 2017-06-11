#region Using Namespace

using System.Windows;
using System.Windows.Controls;
using Idealde.Framework.Panes;

#endregion

namespace Idealde.Modules.Shell.Controls
{
    public class PaneStyleSelector : StyleSelector
    {
        public Style DocumentStyle { get; set; }

        public Style ToolStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is IDocument)
            {
                return DocumentStyle;
            }

            if (item is ITool)
            {
                return ToolStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}