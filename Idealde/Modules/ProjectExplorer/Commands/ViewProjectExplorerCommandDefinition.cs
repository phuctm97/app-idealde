using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.ProjectExplorer.Commands
{
    public class ViewProjectExplorerCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.ProjectExplorer";

        public override string Name => CommandName;

        public override string Tooltip => Resources.ViewProjectExplorerCommandTooltip;

        public override string Text => Resources.ViewProjectExplorerCommadText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Application.png");
    }
}