#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;

#endregion

namespace Idealde.Modules.MainMenu.Models
{
    public class CommandMenuItem : MenuItemBase
    {
        // Dependencies

        #region Dependencies

        private readonly ICommandService _commandService;

        #endregion

        // Backing fields

        #region Backing fields

        private readonly Command _command;

        #endregion

        // Bind properties

        #region Bind properties

        public override string Text => _command.Text;

        public override string Tooltip => _command.Tooltip;

        public override Uri IconSource => _command.IconSource;

        public override ICommand Command => _commandService.GetTargetableCommand(_command);

        public override KeyGesture KeyGesture => null;

        #endregion

        // Initializations

        #region Initializations

        public CommandMenuItem(string name, Command command) : base(name)
        {
            _commandService = IoC.Get<ICommandService>();
            _command = command;
            _command.PropertyChanged += (s, e) => { NotifyOfPropertyChange(e.PropertyName); };
        }

        public CommandMenuItem(string name, CommandDefinition commandDefinition) : base(name)
        {
            _commandService = IoC.Get<ICommandService>();
            _command = _commandService.GetCommand(commandDefinition);
            _command.PropertyChanged += (s, e) => { NotifyOfPropertyChange(e.PropertyName); };
        }

        protected CommandMenuItem(string name, Type commandDefinitionType) : base(name)
        {
            _commandService = IoC.Get<ICommandService>();
            _command = _commandService.GetCommand(_commandService.GetCommandDefinition(commandDefinitionType));
            _command.PropertyChanged += (s, e) => { NotifyOfPropertyChange(e.PropertyName); };
        }

        #endregion
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