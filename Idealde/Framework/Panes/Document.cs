#region Using Namespace

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Modules.Shell.Commands;
using Idealde.Properties;

#endregion

namespace Idealde.Framework.Panes
{
    public abstract class Document : Screen, IDocument
    {
        // Backing fields
        #region Backing fields
        private ICommand _closeCommand;
        private bool _isSelected;

        #endregion

        // Bind properties
        #region Bind properties
        public string ContentId { get; }

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => TryClose())); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }
        #endregion

        // Initializations
        #region Initializations
        protected Document()
        {
            _isSelected = false;

            ContentId = Guid.NewGuid().ToString();
        }

        #endregion

        void ICommandHandler<CloseFileCommandDefinition>.Update(Command command)
        {
            command.Tooltip = string.Format(Resources.FileCloseCommandTooltip, DisplayName);
        }

        Task ICommandHandler<CloseFileCommandDefinition>.Run(Command command)
        {
            TryClose();
            return Task.FromResult(true);
        }

    }
}