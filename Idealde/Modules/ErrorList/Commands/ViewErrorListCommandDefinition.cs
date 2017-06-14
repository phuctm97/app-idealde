#region Using Namespace

using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.ErrorList.Commands
{
    public class ViewErrorListCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.ErrorList";

        public override string Name => CommandName;

        public override string Text => Resources.ViewErrorListCommandText;

        public override string Tooltip => Resources.ViewErrorListCommandTooltip;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/ErrorList.png");
    }
}