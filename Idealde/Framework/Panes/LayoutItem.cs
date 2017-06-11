#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Panes
{
    public abstract class LayoutItem : Screen, ILayoutItem
    {
        // Backing fields
        protected readonly Guid _id;

        // Bind models
        public string ContentId => _id.ToString();

        public abstract ICommand CloseCommand { get; }

        // Initializations

        protected LayoutItem()
        {
            _id = Guid.NewGuid();
        }
    }
}