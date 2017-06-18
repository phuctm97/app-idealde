using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.Shell.Commands
{
    public class OpenFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.OpenFile";

        public override string Name => CommandName;

        public override string Tooltip => Resources.FileOpenFileCommandTooltip;

        public override string Text => Resources.FileOpenFileCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Open.png");
    }
}