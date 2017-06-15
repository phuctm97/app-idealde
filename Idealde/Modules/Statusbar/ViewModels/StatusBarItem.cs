#region Using Namespace

using System.Windows;
using Caliburn.Micro;

#endregion

namespace Idealde.Modules.StatusBar.ViewModels
{
    public class StatusBarItem : PropertyChangedBase
    {
        // Backing fields

        #region Backing fields

        private int _index;

        private string _message;

        #endregion

        // Bind properties

        #region Bind properties

        public int Index
        {
            get { return _index; }
            set
            {
                if (Equals(value, _index)) return;
                _index = value;
                NotifyOfPropertyChange(() => Index);
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (Equals(value, _message)) return;
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        public GridLength Width { get; }

        #endregion

        // Initializations

        #region Initializations

        public StatusBarItem(string message, GridLength width)
        {
            _message = message;
            Width = width;
        }

        #endregion
    }
}