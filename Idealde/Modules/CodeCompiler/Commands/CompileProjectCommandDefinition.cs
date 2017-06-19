using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.CodeCompiler.Commands
{
    public class CompileProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Run.CompileProject";

        public override string Name => CommandName;

        public override string Tooltip => Resources.CompileProjectCommandTooltip;

        public override string Text => Resources.CompileProjectCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/BuildSolution.png");
    }
}