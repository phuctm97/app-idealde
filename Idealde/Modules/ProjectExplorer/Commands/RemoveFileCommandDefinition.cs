using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.ProjectExplorer.Commands
{
    public class RemoveFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.RemoveFile";

        public override string Name => CommandName;

        public override string Tooltip => Resources.RemoveFileCommandTooltip;

        public override string Text => Resources.RemoveFileCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Cancel.png");

    }
}