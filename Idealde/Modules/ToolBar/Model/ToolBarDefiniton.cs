using Caliburn.Micro;

namespace Idealde.Modules.ToolBar.Model
{
    public class ToolBarDefiniton
    {
        #region Fields

        private int _sortOrder;
        private string _name;
        private IObservableCollection < ToolBarGroupItemDefinition > _children;

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

        public IObservableCollection < ToolBarGroupItemDefinition > Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
            }
        }

        #endregion

        #region Initializations

        public ToolBarDefiniton ( string name )
        {
            _children=new BindableCollection < ToolBarGroupItemDefinition > ();
            //_sortOrder = sortOrder;
            _name = name;
        }

        #endregion
    }
}