using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.Output.Commands
{
    public class ViewOutputCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.Output";

        public override string Name => CommandName;

        public override string Tooltip => Resources.ViewOutputCommandTooltip;

        public override string Text => Resources.ViewOutputCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Output.png");
    }
}