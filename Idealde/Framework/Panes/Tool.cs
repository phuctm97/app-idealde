#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;

#endregion

namespace Idealde.Framework.Panes
{
    public abstract class Tool : ViewAware, ITool
    {
        // Backing fields
        private ICommand _closeCommand;
        private bool _isVisible;
        private string _displayName;
        private bool _isSelected;

        // Bind properties
        public string ContentId { get; }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (value == _displayName) return;
                _displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => IsVisible = false)); }
        }

        public abstract PaneLocation PreferredLocation { get; }

        public virtual double PreferredWidth => 200;

        public virtual double PreferredHeight => 200;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
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

        // Initializations
        protected Tool()
        {
            _isVisible = false;

            _isSelected = false;

            _displayName = string.Empty;

            ContentId = Guid.NewGuid().ToString();
        }

    }
}