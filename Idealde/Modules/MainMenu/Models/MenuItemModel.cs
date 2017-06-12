using System;
using Caliburn.Micro;

namespace Idealde.Modules.MainMenu.Models
{
    public class MenuItemModel : PropertyChangedBase
    {
        private string _text;
        private Uri _iconSource;
        private Boolean _isEnable;
        public IObservableCollection<MenuItemModel> Children { get; }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (Equals(_text, value)) return;
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public Uri IconSource
        {
            get
            {
                return _iconSource;
            }
            set
            {
                if (Equals(_iconSource, value)) return;
                _iconSource = value;
                NotifyOfPropertyChange(() => IconSource);
            }
        }

        public Boolean IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (Equals(_isEnable, value)) return;
                _isEnable = value;
                NotifyOfPropertyChange(() => IsEnable);
            }
        }

        public void AddChild(params string[] menuHeader)
        {
            foreach (var header in menuHeader)
            {
            Children.Add(new MenuItemModel(header));
            }
        }


        public MenuItemModel(string text)
        {
            Children = new BindableCollection<MenuItemModel>();
            Text = text;
            IconSource = null;
            IsEnable = true;
        }
    }
}
