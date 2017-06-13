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

        public IObservableCollection<MenuItem> Children { get; }

        public string Name { get; }

        public virtual string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public virtual Uri IconSource
        {
            get { return _iconSource; }
            set
            {
                if (Equals(value, _iconSource)) return;
                _iconSource = value;
                NotifyOfPropertyChange(() => IconSource);
            }
        }

        public virtual KeyGesture KeyGesture => null;

        public virtual ICommand Command => null;

        public MenuItem(string name)
        {
            Children = new BindableCollection<MenuItem>();
            Name = name;
        }
    }
}