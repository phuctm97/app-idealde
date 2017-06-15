#region Using Namespace

using Caliburn.Micro;
using Idealde.Modules.ToolBarTray.Models;

#endregion

namespace Idealde.Modules.ToolBarTray
{
    public interface IToolBarTray
    {
        IObservableCollection<ToolBar> Items { get; }

        void AddToolBarItem(ToolBar parent, params ToolBarItemBase[] toolBarItemsBase);

        void AddToolBar(params ToolBar[] toolBars);
    }
}