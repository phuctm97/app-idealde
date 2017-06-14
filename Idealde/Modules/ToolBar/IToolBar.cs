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

        IObservableCollection < ToolBarDefiniton > Items
        {
            get;
        }

        #endregion

        #region Methods

        void AddToolBarItem (ToolBarDefiniton parent,params ToolBarItemDefinition[] toolBarItems);

        void AddToolBar (params ToolBarDefiniton[] toolBars);

        #endregion
    }
}
