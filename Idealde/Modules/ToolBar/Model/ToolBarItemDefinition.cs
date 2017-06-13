using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Idealde.Modules.ToolBar.Model
{
    public class ToolBarItemDefinition
    {
        #region Fields
        private int _sortOrder;
        private ToolBarItemDisplay _display;
        private string _text;
        private string _name;
        private Uri _iconSource;
        private KeyGesture _keyGesture;
        #endregion

        #region Properties       

        public int SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                _sortOrder = value;
            }
        }

        public ToolBarItemDisplay Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
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
                _iconSource = value;
            }
        }

        public KeyGesture KeyGesture
        {
            get
            {
                return _keyGesture;
            }
            set
            {
                _keyGesture = value;
            }
        }
        #endregion

        #region Initializations

        public ToolBarItemDefinition ( string name, string text )
        {
            _name = name;
            //_sortOrder = sortOrder;
            _text = text;
        }

        #endregion

    }

    #region Enums
    public enum ToolBarItemDisplay
    {
        IconOnly,
        IconAndText
    }
    #endregion

}
