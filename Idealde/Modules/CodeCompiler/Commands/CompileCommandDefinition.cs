using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.CodeCompiler.Commands
{
    public class CompileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Run.Build";

        public override string Name => CommandName;

        public override string Tooltip => Resources.RunBuildCommandTooltip;

        public override string Text => Resources.RunBuildCommandText;

        public override Uri IconSource => null;
    }
}