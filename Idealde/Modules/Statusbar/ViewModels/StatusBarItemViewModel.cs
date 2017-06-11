using System.Windows;
using Caliburn.Micro;

namespace Idealde.Modules.StatusBar.ViewModels
{
    public class StatusBarItemViewModel : PropertyChangedBase
    {
        private int _index;


        private string _message;

        public StatusBarItemViewModel(string message, GridLength width)
        {
            _message = message;
            _width = width;
        }

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

        private readonly GridLength _width;

        public GridLength Width => _width;
    }
}