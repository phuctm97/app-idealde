using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.ProjectExplorer.Commands
{
    public class RemoveFileCommandDefinition: CommandDefinition
    {
        public const string CommandName = "Project.RemoveFile";

        public override string Name => CommandName;

        public override string Text => Resources.FileRemoveCommandText;

        public override string Tooltip => Resources.FileRemoveCommandTooltip;

        public override Uri IconSource =>
            new Uri("pack://application:,,,/Idealde;Component/Resources/Images/Remove.png", UriKind.Absolute);
    }
}
