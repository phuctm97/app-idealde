#region Using Namespace

using System;

#endregion

namespace Idealde.Framework.Commands
{
    public abstract class CommandDefinition
    {
        // Command unit name
        public abstract string Name { get; }

        // Command display name
        public virtual string Text => string.Empty;

        // Command display description
        public virtual string Tooltip => string.Empty;

        // Command display icon
        public virtual Uri IconSource => null;
    }
}