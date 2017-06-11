using System;
using Caliburn.Micro;

namespace Idealde.Framework.Panes
{
    public abstract class LayoutItem : Screen, ILayoutItem
    {
        // Backing fields
        protected readonly Guid _id;

        public string ContentId => _id.ToString();

        // Initializations
        protected LayoutItem()
        {
            _id = Guid.NewGuid();
        }
    }
}