using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.Shell.Commands
{
    public class OpenFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.Open";

        public override string Name => CommandName;

        public override string Tooltip => Resources.FileOpenCommandTooltip;

        public override string Text => Resources.FileOpenCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Open.png");
    }
}