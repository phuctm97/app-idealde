#region Using Namespace

using System.Linq;
using System.Windows;
using System.Windows.Input;

#endregion

namespace Idealde.Framework.Commands
{
    public class CommandKeyGestureService : ICommandKeyGestureService
    {
        // Dependencies
        private readonly ICommandService _commandService;
        private readonly CommandKeyboardShortcut[] _commandKeyboardShorts;

        public CommandKeyGestureService(
            ICommandService commandService,
            CommandKeyboardShortcut[] commandKeyboardShorts)
        {
            _commandService = commandService;
            _commandKeyboardShorts = commandKeyboardShorts;
        }

        public void BindKeyGestures(UIElement uiElement)
        {
            foreach (var keyboardShort in _commandKeyboardShorts)
            {
                if (keyboardShort.KeyGesture == null) continue;
            }
        }

        public KeyGesture GetPrimaryKeyGesture(CommandDefinition commandDefinition)
        {
            var keyboardShortcut = _commandKeyboardShorts.FirstOrDefault(c => c.CommandDefinition == commandDefinition);
            return keyboardShortcut?.KeyGesture;
        }
    }
}