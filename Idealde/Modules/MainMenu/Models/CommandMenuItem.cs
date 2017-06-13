#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;

#endregion

namespace Idealde.Modules.MainMenu.Models
{
    public class CommandMenuItem : MenuItem
    {
        private readonly ICommandService _commandService;
        private readonly Command _command;

        public override ICommand Command
        {
            get { return _commandService.GetTargetableCommand(_command); }
        }

        public CommandMenuItem(string name, Command command) : base(name)
        {
            _commandService = IoC.Get<ICommandService>();

            _command = command;
        }

        public CommandMenuItem(string name, CommandDefinition commandDefinition) : base(name)
        {
            _commandService = IoC.Get<ICommandService>();

            _command = _commandService.GetCommand(commandDefinition);
        }

        public CommandMenuItem(string name, Type commandDefinitionType) : base(name)
        {
            _commandService = IoC.Get<ICommandService>();

            _command = _commandService.GetCommand(_commandService.GetCommandDefinition(commandDefinitionType));
        }
    }

    public class CommandMenuItem<TCommandDefinition> : CommandMenuItem
        where TCommandDefinition : CommandDefinition
    {
        public CommandMenuItem(string name) :
            base(name, typeof(TCommandDefinition))
        {
        }
    }
}