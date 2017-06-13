#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;

#endregion

namespace Idealde.Modules.MainMenu.Models
{
    public class MenuItem : PropertyChangedBase
    {
        private string _text;
        private Uri _iconSource;
        private KeyGesture _keyGesture;
        private ICommand _command;

        public IObservableCollection<MenuItem> Children { get; }

        public string Name { get; }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public Uri IconSource
        {
            get { return _iconSource; }
            set
            {
                if (Equals(value, _iconSource)) return;
                _iconSource = value;
                NotifyOfPropertyChange(() => IconSource);
            }
        }

        public KeyGesture KeyGesture
        {
            get { return _keyGesture; }
            set
            {
                if (Equals(value, _keyGesture)) return;
                _keyGesture = value;
                NotifyOfPropertyChange(() => KeyGesture);
            }
        }

        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (Equals(value, _command)) return;
                _command = value;
                NotifyOfPropertyChange(() => Command);
            }
        }

        public MenuItem(string name)
        {
            Children = new BindableCollection<MenuItem>();
            Name = name;
        }
    }
}