using System;
using Caliburn.Micro;

namespace Idealde.Modules.MainMenu.Models
{
    public class MenuItemBase : PropertyChangedBase, IMenuItem
    {
        private readonly string _text;
        private readonly Uri _iconSource;
        public string Text { get { return _text; } }
        public Uri IconSource { get { return _iconSource; } }
    }
}
