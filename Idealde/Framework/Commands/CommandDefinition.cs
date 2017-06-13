using System;

namespace Idealde.Framework.Commands
{
    public abstract class CommandDefinition
    {
        public abstract string Name { get; }
        public virtual string Text => string.Empty;
        public virtual string Tooltip => string.Empty;
        public virtual Uri IconSource => null;
    }
}