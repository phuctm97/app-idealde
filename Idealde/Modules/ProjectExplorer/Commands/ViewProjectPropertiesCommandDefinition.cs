using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.ProjectExplorer.Commands
{
    public class ViewProjectPropertiesCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Properties";

        public override string Name => CommandName;

        public override string Tooltip => Resources.ViewProjectPropertiesCommandTooltip;

        public override string Text => Resources.ViewProjectPropertiesCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Property.png");
    }
}