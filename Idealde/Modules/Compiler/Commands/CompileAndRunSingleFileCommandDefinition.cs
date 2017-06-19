using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.Compiler.Commands
{
    public class CompileAndRunSingleFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Run.CompileSingleFile";

        public override string Name => CommandName;

        public override string Tooltip => Resources.CompileAndRunSingleFileCommandTooltip;

        public override string Text => Resources.CompileAndRunSingleFileCommandText;

        public override Uri IconSource => null;
    }
}