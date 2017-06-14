using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using Idealde.Framework.Controls;
using Idealde.Modules.ToolBar.Model;

namespace Idealde.Modules.ToolBar.Controls
{
    public class ToolBarBase : System.Windows.Controls.ToolBar
    {
        private object _currentItem;

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            _currentItem = item;
            return base.IsItemItsOwnContainerOverride(item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            if ( _currentItem is ToolBarItemSeparator )
                return new Separator ( );

            if (_currentItem is ToolBarItemDefinition)
                return CreateButton<Button>(ButtonStyleKey, "ToolBarButton");

            return base.GetContainerForItemOverride();
        }

        private static T CreateButton<T>(object baseStyleKey, string styleKey)
            where T : FrameworkElement, new()
        {
            var result = new T();
            result.SetResourceReference(DynamicStyle.BaseStyleProperty, baseStyleKey);
            result.SetResourceReference(DynamicStyle.DerivedStyleProperty, styleKey);
            return result;
        }

        public ToolBarBase ( )
        {

            SetOverflowMode(this, OverflowMode.Always);
            SetResourceReference(StyleProperty, typeof(System.Windows.Controls.ToolBar));
        }
    }
}
