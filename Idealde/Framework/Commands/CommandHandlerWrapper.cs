using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Idealde.Framework.Commands
{
    public sealed class CommandHandlerWrapper
    {
        public static CommandHandlerWrapper FromCommandHandler(Type commandHandlerInterfaceType, object commandHandler)
        {
            var updateMethod = commandHandlerInterfaceType.GetMethod("Update");
            var runMethod = commandHandlerInterfaceType.GetMethod("Run");
            return new CommandHandlerWrapper(commandHandler, updateMethod, runMethod);
        }

        private readonly object _commandHandler;
        private readonly MethodInfo _updateMethod;
        private readonly MethodInfo _runMethod;

        private CommandHandlerWrapper(
            object commandHandler,
            MethodInfo updateMethod,
            MethodInfo runMethod)
        {
            _commandHandler = commandHandler;
            _updateMethod = updateMethod;
            _runMethod = runMethod;
        }

        public void Update(Command command)
        {
            if (_updateMethod != null)
                _updateMethod.Invoke(_commandHandler, new object[] { command });
        }

        public Task Run(Command command)
        {
            return (Task)_runMethod.Invoke(_commandHandler, new object[] { command });
        }
    }
}