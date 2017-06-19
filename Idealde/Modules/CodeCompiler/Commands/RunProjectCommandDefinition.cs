#region Using Namespace

using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.CodeCompiler.Commands
{
    public class RunProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Run.RunProject";

        public override string Name => CommandName;

        public override string Tooltip => Resources.CompileProjectCommandTooltip;

        public override string Text => Resources.CompileProjectCommandText;

        public override Uri IconSource
            => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Running.png");
    }
}