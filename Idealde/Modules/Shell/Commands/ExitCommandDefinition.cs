using Idealde.Framework.Commands;

namespace Idealde.Modules.Shell.Commands
{
    public class ExitCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.Exit";

        public override string Name => CommandName;

        public override string Text => "Exit";

        public override string Tooltip => "Exit application";
    }
}