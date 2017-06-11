using System;
using Caliburn.Micro;

namespace Idealde.Modules.MainMenu.Models
{
    public class MenuItemModel : PropertyChangedBase
    {
        private readonly string _text;
        private readonly Uri _iconSource;
        public  IObservableCollection<MenuItemModel> Children { get; }



        public string Text { get { return _text; } }
        public Uri IconSource { get { return _iconSource; } }



        public void AddChild(string text, Uri iconSource)
        {
            Children.Add(new MenuItemModel(text, iconSource));
        }



        public MenuItemModel(string text, Uri iconSource)
        {
            Children = new BindableCollection<MenuItemModel>();
            _text = text;
            _iconSource = iconSource;
        }
    }
}
