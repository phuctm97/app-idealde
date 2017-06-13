#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Commands
{
    public class TargatableCommand : ICommand
    {
        // Dependencies 

        #region Dependencies

        private readonly ICommandRouter _commandRouter;

        #endregion

        // Backing fields

        #region Backing fields

        private readonly Command _command;

        #endregion

        // Initializations

        #region Initializations

        public TargatableCommand(Command command)
        {
            _command = command;
            _commandRouter = IoC.Get<ICommandRouter>();
        }

        #endregion

        // Behaviors

        #region Behaviors

        public bool CanExecute(object parameter)
        {
            // routing to current active handler of this command
            var handler = _commandRouter.GetHandler(_command.CommandDefinition);
            if (handler == null) return false;

            // call handler update method
            handler.Update(_command);
            return _command.IsEnabled;
        }

        public async void Execute(object parameter)
        {
            // routing to current active handler of this command
            var handler = _commandRouter.GetHandler(_command.CommandDefinition);
            if (handler == null) return;

            // call handler run method
            await handler.Run(_command);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}