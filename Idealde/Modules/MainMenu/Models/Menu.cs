#region Using Namespace

using Caliburn.Micro;

#endregion

namespace Idealde.Modules.MainMenu.Models
{
    public class Menu : PropertyChangedBase
    {
        private string _text;

        public string Name { get; }

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

        public IObservableCollection<MenuItem> Children { get; }

        public Menu(string text, string name = "")
        {
            Children = new BindableCollection<MenuItem>();
            Text = text;
            Name = name == "" ? text : name;
        }
    }
}