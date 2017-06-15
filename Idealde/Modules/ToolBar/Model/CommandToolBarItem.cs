using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Caliburn.Micro;

using Idealde.Framework.Commands;

namespace Idealde.Modules.ToolBar.Model
{
    public class CommandToolBarItem:ToolBarItem
    {
        private readonly ICommandService _commandService;
        private readonly Command _command;

        public override string Text => _command.Text;

        public override string Tooltip => _command.Tooltip;

        public override Uri IconSource => _command.IconSource;

        public override ICommand Command => _commandService.GetTargetableCommand(_command);

        public override KeyGesture KeyGesture => null;

        public CommandToolBarItem(string name) : base(name)
        {
        }

        public CommandToolBarItem(string name, Command command) : base(name)
        {
            _commandService = IoC.Get<ICommandService>();
            _command = command;
            _command.PropertyChanged += (s, e) => { NotifyOfPropertyChange(e.PropertyName); };
        }

        public CommandToolBarItem(string name, CommandDefinition commandDefinition) : base(name)
        {
            _commandService = IoC.Get<ICommandService>();
            _command = _commandService.GetCommand(commandDefinition);
            _command.PropertyChanged += (s, e) => { NotifyOfPropertyChange(e.PropertyName); };
        }

        protected CommandToolBarItem(string name, Type commandDefinitionType) : base(name)
        {
            _commandService = IoC.Get<ICommandService>();
            _command = _commandService.GetCommand(_commandService.GetCommandDefinition(commandDefinitionType));
            _command.PropertyChanged += (s, e) => { NotifyOfPropertyChange(e.PropertyName); };
        }

    }
}
