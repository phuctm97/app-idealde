using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.UndoRedo.Commands
{
    public class UndoCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Edit.Undo";

        public override string Name => CommandName;

        public override string Tooltip => Resources.EditUndoCommandTooltip;

        public override string Text => Resources.EditUndoCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/Undo.png");
    }
}