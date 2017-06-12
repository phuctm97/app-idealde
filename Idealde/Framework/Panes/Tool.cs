#region Using Namespace

using System.Windows.Input;
using Idealde.Framework.Commands;

#endregion

namespace Idealde.Framework.Panes
{
    public abstract class Tool : LayoutItem, ITool
    {
        // Backing fields
        private ICommand _closeCommand;
        private bool _isVisible;

        // Bind properties
        public override ICommand CloseCommand
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

        // Initializations
        protected Tool()
        {
            _isVisible = false;
        }
    }
}