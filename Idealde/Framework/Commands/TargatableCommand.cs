using System;
using System.Windows.Input;
using Caliburn.Micro;

namespace Idealde.Framework.Commands
{
    public class TargatableCommand : ICommand
    {
        // Dependencies 
        private readonly ICommandRouter _commandRouter;

        // Backing fields
        private readonly Command _command;

        public TargatableCommand(Command command)
        {
            _command = command;
            _commandRouter = IoC.Get<ICommandRouter>();
        }

        public bool CanExecute(object parameter)
        {
            var handler = _commandRouter.GetHandler(_command.CommandDefinition);
            if (handler == null) return false;

            handler.Update(_command);
            return _command.IsEnabled;
        }

        public async void Execute(object parameter)
        {
            var handler = _commandRouter.GetHandler(_command.CommandDefinition);
            if (handler == null) return;

            await handler.Run(_command);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}