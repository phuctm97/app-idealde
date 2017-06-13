using Caliburn.Micro;

namespace Idealde.Modules.ToolBar.Model
{
    public class ToolBarGroupItemDefinition
    {
        #region Fields

        private IObservableCollection < ToolBarItemDefinition > _children;
        private string _name;
        private int _sortOrder;

        #endregion

        #region Properties

        public IObservableCollection<ToolBarItemDefinition> Children
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

        #endregion

        #region Initializations

        public ToolBarGroupItemDefinition (string name )
        {
            _children=new BindableCollection < ToolBarItemDefinition > ();
            _name = name;
            //_sortOrder = sortOrder;
        }
        #endregion
    }
}