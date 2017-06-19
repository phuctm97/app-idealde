using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.Compiler.Commands
{
    public class RunSingleFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Run.RunSingleFile";

        public override string Name => CommandName;

        public override string Tooltip => Resources.RunSingleFileCommandTooltip;

        public override string Text => Resources.RunSingleFileCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Run.png");
    }
}