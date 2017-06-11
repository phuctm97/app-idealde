namespace Idealde.Framework.Panes
{
    public abstract class Tool : LayoutItem, ITool
    {
        // Backing fields
        #region Backing fields
        private double _preferredWidth;
        private double _preferredHeight;
        private bool _isVisible;
        #endregion

        // Bind properties
        #region Backing properties
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
        } 
        #endregion
    }
}