#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;

#endregion

namespace Idealde.Modules.ToolBarTray.Models
{
    public abstract class ToolBarItemBase : PropertyChangedBase
    {
        public string Name { get; }
        public abstract string Text { get; }
        public abstract string Tooltip { get; }
        public abstract Uri IconSource { get; }
        public abstract KeyGesture KeyGesture { get; }
        public abstract ICommand Command { get; }
        public abstract bool IsShowText { get; }

        protected ToolBarItemBase(string name)
        {
            Name = name;
        }
    }

    #region Enums

    public enum ToolBarItemDisplay
    {
        IconOnly,
        IconAndText,
        Empty
    }

    #endregion
}