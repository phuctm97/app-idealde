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
        private Boolean _isCheckable;
        private Boolean _isChecked;
        private Boolean _isVisible;

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

        public Boolean IsCheckable
        {
            get { return _isCheckable; }
            set
            {
                if (Equals(_isCheckable, value)) return;
                _isCheckable = value;
                NotifyOfPropertyChange(() => IsCheckable);
            }
        }
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (Equals(_isChecked, value)) return;
                _isChecked = value;
                NotifyOfPropertyChange(() => IsChecked);
            }
        }
        public IObservableCollection<MenuItemDefinition> Childrens { get;set; }

        public Boolean IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (Equals(_isVisible, value)) return;
                _isVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        public MenuDefinition(String text, string name="")
        {
            Childrens = new BindableCollection<MenuItemDefinition>();
            _text = text;
            _name = name == "" ? text : name;
            IsCheckable = false;
            IsVisible = true;
            IsChecked = false;
        }
    }
}
