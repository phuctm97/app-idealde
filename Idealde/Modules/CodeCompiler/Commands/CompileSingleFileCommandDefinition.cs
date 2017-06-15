#region Using Namespace

using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.CodeCompiler.Commands
{
    public class CompileSingleFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Run.CompileSingleFile";

        public override string Name => CommandName;

        public override string Tooltip => Resources.CompileSingleFileCommandTooltip;

        public override string Text => Resources.CompileSingleFileCommandText;

        public override Uri IconSource => null;
    }
}