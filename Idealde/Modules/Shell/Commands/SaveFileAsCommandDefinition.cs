using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.Shell.Commands
{
    public class SaveFileAsCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.SaveAs";

        public override string Name => CommandName;

        public override string Tooltip => Resources.FileSaveAsCommandTooltip;

        public override string Text => Resources.FileSaveAsCommandText;

        public override Uri IconSource => null;
    }
}