#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;

#endregion

namespace Idealde.Modules.ToolBarTray.Models
{
    public class CommandToolBarItem : ToolBarItemBase
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

        public override bool IsShowText { get; }

        public override string Text => _command.Text;

        public override string Tooltip => _command.Tooltip.Length == 0 ? _command.Text : _command.Tooltip;

        public override Uri IconSource => _command.IconSource;

        public override ICommand Command => _commandService.GetTargetableCommand(_command);

        public override KeyGesture KeyGesture => null;

        #endregion

        // Initializations

        #region Initializations

        public CommandToolBarItem(string name, bool isShowText, Command command) : base(name)
        {
            IsShowText = isShowText;
            _commandService = IoC.Get<ICommandService>();
            _command = command;
            _command.PropertyChanged += (s, e) =>
            {
                NotifyOfPropertyChange(e.PropertyName);
                if (string.Equals(e.PropertyName, "Text")) NotifyOfPropertyChange(() => Tooltip);
            };
        }

        public CommandToolBarItem(string name, bool isShowText, CommandDefinition commandDefinition) : base(name)
        {
            IsShowText = isShowText;
            _commandService = IoC.Get<ICommandService>();
            _command = _commandService.GetCommand(commandDefinition);
            _command.PropertyChanged += (s, e) =>
            {
                NotifyOfPropertyChange(e.PropertyName);
                if (string.Equals(e.PropertyName, "Text")) NotifyOfPropertyChange(() => Tooltip);
            };
        }

        protected CommandToolBarItem(string name, bool isShowText, Type commandDefinitionType) : base(name)
        {
            IsShowText = isShowText;
            _commandService = IoC.Get<ICommandService>();
            _command = _commandService.GetCommand(_commandService.GetCommandDefinition(commandDefinitionType));
            _command.PropertyChanged += (s, e) =>
            {
                NotifyOfPropertyChange(e.PropertyName);
                if (string.Equals(e.PropertyName, "Text")) NotifyOfPropertyChange(() => Tooltip);
            };
        }

        #endregion
    }

    public class CommandToolBarItem<TCommandDefinition> : CommandToolBarItem
        where TCommandDefinition : CommandDefinition
    {
        public CommandToolBarItem(string name, bool isShowText = false) :
            base(name, isShowText, typeof(TCommandDefinition))
        {
        }
    }
}