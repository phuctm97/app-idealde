#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;

#endregion

namespace Idealde.Modules.MainMenu.Models
{
    public abstract class MenuItemBase : PropertyChangedBase
    {
        public IObservableCollection<MenuItemBase> Children { get; }

        public string Name { get; }
        public abstract string Text { get; }
        public abstract string Tooltip { get; }
        public abstract Uri IconSource { get; }
        public abstract KeyGesture KeyGesture { get; }
        public abstract ICommand Command { get; }

        protected MenuItemBase(string name)
        {
            Name = name;
            Children = new BindableCollection<MenuItemBase>();
        }
    }
}