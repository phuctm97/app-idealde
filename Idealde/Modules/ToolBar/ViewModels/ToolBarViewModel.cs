#region Using Namespace

using Caliburn.Micro;
using Idealde.Modules.ToolBar.Model;

#endregion

namespace Idealde.Modules.ToolBar.ViewModels
{
    class ToolBarViewModel : ViewAware, IToolBar
    {
        #region Fields

        public IObservableCollection<ToolBarDefiniton> Items { get; }

        #endregion

        #region Initiliazation

        public ToolBarViewModel()
        {
            Items = new BindableCollection<ToolBarDefiniton>();
        }

        #endregion

        #region Methods

        public void AddToolBarItem(ToolBarGroupItemDefinition parent, params ToolBarItemDefinition[] toolBarItems)
        {
            foreach (var toolBarItem in toolBarItems)
            {
                parent.Children.Add(toolBarItem);
            }
        }

        public void AddToolBarGroupItem(ToolBarDefiniton parent, params ToolBarGroupItemDefinition[] toolBarGroupItems)
        {
            foreach (var toolBarGroupItem in toolBarGroupItems)
            {
                parent.Children.Add(toolBarGroupItem);
            }
        }

        public void AddToolBar(params ToolBarDefiniton[] toolBars)
        {
            foreach (var toolBar in toolBars)
            {
                Items.Add(toolBar);
            }
        }

        #endregion
    }
}