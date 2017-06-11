#region Using Namespace

using System.Windows;
using System.Windows.Controls;
using Idealde.Framework.Panes;

#endregion

namespace Idealde.Modules.Shell.Controls
{
    public class PaneTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DocumentTemplate { get; set; }

        public DataTemplate ToolTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IDocument)
            {
                return DocumentTemplate;
            }

            if (item is ITool)
            {
                return ToolTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}