#region Using Namespace

using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.Shell.Commands
{
    public class CloseFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.Close";

        public override string Name => CommandName;

        public override string Tooltip => Resources.FileCloseCommandTooltip;

        public override string Text => Resources.FileCloseCommandText;

        public override Uri IconSource => null;
    }
}