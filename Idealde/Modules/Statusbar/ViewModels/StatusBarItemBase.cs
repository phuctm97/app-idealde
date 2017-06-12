using System.Windows;
using Caliburn.Micro;

namespace Idealde.Modules.StatusBar.ViewModels
{
    public class StatusBarItemBase : PropertyChangedBase
    {
        private int _index;

        private string _message;

        private readonly GridLength _width;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                NotifyOfPropertyChange(() => Index);
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Index);
            }
        }

        public GridLength Width => _width;

        public StatusBarItemBase(string message, GridLength width)
        {
            _message = message;
            _width = width;
        }
    }
}