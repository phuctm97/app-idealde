#region Using Namespace

using System;

#endregion

namespace Idealde.Framework.Commands
{
    public abstract class CommandDefinition
    {
        public abstract string Name { get; }
        public abstract string Text { get; }
        public virtual string ToolTip => string.Empty;
        public virtual Uri IconSource => null;
    }
}