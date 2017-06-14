using Caliburn.Micro;

namespace Idealde.Modules.ToolBar.Model
{
    public class ToolBarDefiniton
    {
        #region Fields

        private int _sortOrder;
        private string _name;
        private IObservableCollection < ToolBarItemDefinition > _toolBarItems;

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

        public IObservableCollection < ToolBarItemDefinition > ToolBarItems
        {
            get
            {
                return _toolBarItems;
            }
            set
            {
                _toolBarItems = value;
            }
        }

        #endregion

        #region Initializations

        public ToolBarDefiniton ( string name )
        {
            _toolBarItems=new BindableCollection < ToolBarItemDefinition > ();
            //_sortOrder = sortOrder;
            _name = name;
        }

        #endregion
    }
}