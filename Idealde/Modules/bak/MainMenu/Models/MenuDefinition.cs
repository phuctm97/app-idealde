using System;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;

namespace Idealde.Modules.MainMenu.Models
{
    public class MenuDefinition : PropertyChangedBase
    {
        private String _name;
        private string _text;
        private Uri _iconSource;
        private KeyGesture _keyGesture;
        private ICommandMenuItem _command;

        public String Name
        {
            get { return _name; }
            set
            {
                if (Equals(_name, value)) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (Equals(_text, value)) return;
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public Uri IconSource
        {
            get { return _iconSource; }
            set
            {
                if (Equals(_iconSource, value)) return;
                _iconSource = value;
                NotifyOfPropertyChange(() => IconSource);
            }
        }

        public KeyGesture KeyGesture
        {
            get { return _keyGesture; }
            set
            {
                if (Equals(_keyGesture, value)) return;
                _keyGesture = value;
                NotifyOfPropertyChange(() => KeyGesture);
            }
        }

        public ICommandMenuItem Command
        {
            get { return _command; }
            set
            {
                if (Equals(_command, value)) return;
                _command = value;
                NotifyOfPropertyChange(() => Command);
            }
        }

        public IObservableCollection<MenuItemDefinition> Childrens { get;set; }

        public MenuDefinition(String name, string text="")
        {
            Childrens = new BindableCollection<MenuItemDefinition>();
            _name = name;
            _text = text == "" ? name : text;
        }
    }
}
