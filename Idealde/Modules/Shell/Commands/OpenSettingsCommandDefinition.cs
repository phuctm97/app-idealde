using System;
using Idealde.Framework.Commands;

namespace Idealde.Modules.Shell.Commands
{
    public class OpenSettingsCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Tools.Settings";

        public override string Name => CommandName;

        public override string Text => "Settings";

        public override string Tooltip => string.Empty;

        public override Uri IconSource => null;
    }
}