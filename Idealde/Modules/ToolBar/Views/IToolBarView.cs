using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Idealde.Modules.ToolBar.Views
{
    public interface IToolBarView
    {
        ToolBarTray ToolBarTray
        {
            get;
        }
    }
}
