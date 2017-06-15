using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Caliburn.Micro;


namespace Idealde.Modules.ToolBar.Model
{
    public abstract class ToolBarItem : PropertyChangedBase
    {
        #region Fields
        
        public string Name { get; }
        //public int SortOrder
        //{
        //    get;
        //    set;
        //}
        //public ToolBarItemDisplay Display
        //{
        //    get;
        //    set;
        //}

        public abstract string Text { get; }
        public abstract string Tooltip { get; }
        public abstract Uri IconSource { get; }
        public abstract KeyGesture KeyGesture { get; }
        public abstract ICommand Command { get; }
        #endregion

        #region Initializations

        public ToolBarItem ( string name)
        {
            Name = name;
        }

        #endregion

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
