#region Using Namespace

using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.Shell.Commands
{
    public class SaveFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.Save";

        public override string Name => CommandName;

        public override string Tooltip => Resources.FileSaveCommandTooltip;

        public override string Text => Resources.FileSaveCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Save.png");
    }
}