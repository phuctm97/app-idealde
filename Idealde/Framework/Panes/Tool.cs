#region Using Namespace

using System.Windows.Input;
using Idealde.Framework.Commands;

#endregion

namespace Idealde.Framework.Panes
{
    public abstract class Tool : LayoutItem, ITool
    {
        // Backing fields

        #region Backing fields

        private double _preferredWidth;
        private double _preferredHeight;
        private bool _isVisible;
        protected ICommand _closeCommand;

        #endregion

        // Bind properties

        #region Backing properties

        public override ICommand CloseCommand
        {
            get { return _closeCommand ?? new RelayCommand(p => IsVisible = false, p => true); }
        }

        public PaneLocation PreferredLocation { get; }

        public double PreferredWidth
        {
            get { return _preferredWidth; }
            set
            {
                if (value.Equals(_preferredWidth)) return;
                _preferredWidth = value;
                NotifyOfPropertyChange(() => PreferredWidth);
            }
        }

        public double PreferredHeight
        {
            get { return _preferredHeight; }
            set
            {
                if (value.Equals(_preferredHeight)) return;
                _preferredHeight = value;
                NotifyOfPropertyChange(() => PreferredHeight);
            }
        }

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

        #endregion

        // Initializations

        #region Initializations

        protected Tool(PaneLocation preferredLocation)
        {
            PreferredLocation = preferredLocation;

            _closeCommand = null;
        }

        #endregion
    }
}