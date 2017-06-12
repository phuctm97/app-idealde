#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Panes
{
    public abstract class LayoutItem : Screen, ILayoutItem
    {
        // Bind properties
        public string ContentId { get; }

        public abstract ICommand CloseCommand { get; }

        // Initializations
        protected LayoutItem()
        {
            ContentId = Guid.NewGuid().ToString();
        }
    }
}