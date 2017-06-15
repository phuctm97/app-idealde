using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

using Idealde.Modules.ToolBar.Model;

namespace Idealde.Modules.ToolBar
{
    public interface IToolBar
    {
        #region Fields

        IObservableCollection <Model.ToolBar > Items
        {
            get;
        }

        #endregion

        #region Methods

        void AddToolBarItem (Model.ToolBar parent,params ToolBarItem[] toolBarItems);

        void AddToolBar (params Model.ToolBar[] toolBars);

        #endregion
    }
}
