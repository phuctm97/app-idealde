#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Commands
{
    public abstract class CommandKeyboardShortcut
    {
        private readonly Func<CommandDefinition> _getCommandDefinition;

        protected CommandKeyboardShortcut(KeyGesture keyGesture, Func<CommandDefinition> getCommandDefinition)
        {
            KeyGesture = keyGesture;

            _getCommandDefinition = getCommandDefinition;
        }

        public KeyGesture KeyGesture { get; }

        public CommandDefinition CommandDefinition => _getCommandDefinition();
    }

    public class CommandKeyboardShortcut<TCommandDefinition> : CommandKeyboardShortcut
        where TCommandDefinition : CommandDefinition
    {
        public CommandKeyboardShortcut(KeyGesture keyGesture) :
            base(keyGesture, () => IoC.Get<ICommandService>().GetCommandDefinition(typeof(TCommandDefinition)))
        {
        }
    }
}