#region Using Namespace

using System;
using System.Collections.Generic;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Commands
{
    public class CommandService : ICommandService
    {
        // Backing fields

        #region Backing fields

        private readonly Dictionary<Type, CommandDefinition> _commandDefinitionsLookup;
        private readonly Dictionary<CommandDefinition, Command> _commandsLookup;
        private readonly Dictionary<Command, TargatableCommand> _targetableCommandsLookup;

        #endregion

        // Initializations

        #region Initializations

        public CommandService()
        {
            _commandDefinitionsLookup = new Dictionary<Type, CommandDefinition>();

            _commandsLookup = new Dictionary<CommandDefinition, Command>();

            _targetableCommandsLookup = new Dictionary<Command, TargatableCommand>();
        }

        #endregion

        // Features

        #region Features

        public TargatableCommand GetTargetableCommand(Command command)
        {
            TargatableCommand targetableCommand = null;
            // try look up in stored hash table
            if (!_targetableCommandsLookup.TryGetValue(command, out targetableCommand))
            {
                // create a new one and hash to table
                targetableCommand = new TargatableCommand(command);
                _targetableCommandsLookup.Add(command, targetableCommand);
            }

            return targetableCommand;
        }

        public CommandDefinition GetCommandDefinition(Type commandDefinitionType)
        {
            CommandDefinition commandDefinition;
            // try look up in stored hash table
            if (!_commandDefinitionsLookup.TryGetValue(commandDefinitionType, out commandDefinition))
            {
                // create a new one and hash to table
                commandDefinition = (CommandDefinition) IoC.GetInstance(commandDefinitionType, string.Empty);
                _commandDefinitionsLookup.Add(commandDefinitionType, commandDefinition);
            }
            return commandDefinition;
        }

        public Command GetCommand(CommandDefinition commandDefinition)
        {
            if (commandDefinition is FakeCommandDefinition)
            {
                return new Command(commandDefinition);
            }

            Command command = null;
            // try look up in stored hash table
            if (!_commandsLookup.TryGetValue(commandDefinition, out command))
            {
                // create a new one and hash to table
                command = new Command(commandDefinition);
                _commandsLookup.Add(commandDefinition, command);
            }

            return command;
        }

        #endregion
    }
}