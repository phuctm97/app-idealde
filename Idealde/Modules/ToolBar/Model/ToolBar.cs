using Caliburn.Micro;

namespace Idealde.Modules.ToolBar.Model
{
    public class ToolBar : PropertyChangedBase
    {
        #region Fields

        //public int SortOrder
        //{
        //    get;
        //    set;
        //}
        public string Name
        {
            get;
        }
        public IObservableCollection < ToolBarItem > ToolBarItems
        {
            get;
        }

        #endregion


        #region Initializations

        public ToolBar ( string name )
        {
            ToolBarItems = new BindableCollection < ToolBarItem > ( );
            Name = name;
        }

        #endregion
    }
}