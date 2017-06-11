using System;
using Caliburn.Micro;

namespace Idealde.Framework
{
    public abstract class LayoutItemBase : Screen, ILayoutItem
    {
        // Backing fields
        protected readonly Guid _id;

        public string ContentId => _id.ToString();

        // Initializations
        protected LayoutItemBase()
        {
            _id = Guid.NewGuid();
        }
    }
}