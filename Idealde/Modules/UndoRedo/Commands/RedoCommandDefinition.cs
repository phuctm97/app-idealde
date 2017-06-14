using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.UndoRedo.Commands
{
    public class RedoCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Edit.Redo";

        public override string Name => CommandName;

        public override string Tooltip => Resources.EditRedoCommandTooltip;

        public override string Text => Resources.EditRedoCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Redo.png");
    }
}