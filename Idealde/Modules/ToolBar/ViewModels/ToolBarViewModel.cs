#region Using Namespace

using Caliburn.Micro;

using Idealde.Modules.ToolBar.Controls;
using Idealde.Modules.ToolBar.Model;
using Idealde.Modules.ToolBar.Views;

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

        public void AddToolBarItem(ToolBarDefiniton parent, params ToolBarItemDefinition[] toolBarItems)
        {
            foreach (var toolBarItem in toolBarItems)
            {
                parent.ToolBarItems.Add(toolBarItem);
            }
        }

        public void AddToolBar(params ToolBarDefiniton[] toolBars)
        {
            foreach (var toolBar in toolBars)
            {
                Items.Add(toolBar);
            }
        }

        protected override void OnViewLoaded ( object view )
        {
            ToolBarItemDefinition item=new ToolBarItemDefinition("file","view");
            ToolBarItemDefinition item1 = new ToolBarItemDefinition("help", "win");
            ToolBarDefiniton toolbar = new ToolBarDefiniton ( "ae" );
            AddToolBarItem(toolbar, item);
            AddToolBarItem(toolbar, item1);
            AddToolBar(toolbar);

            ToolBarItemDefinition item2 = new ToolBarItemDefinition("help", "you");
            ToolBarItemDefinition item3 = new ToolBarItemDefinition("kra", "me");
            ToolBarDefiniton toolbar1 = new ToolBarDefiniton("ae");
            AddToolBarItem(toolbar1, item2);
            AddToolBarItem(toolbar1, item3);
            AddToolBar(toolbar1);

            foreach (var toolBar in Items)
            {
                ((IToolBarView)view).ToolBarTray.ToolBars.Add(new ToolBarBase()
                {
                    ItemsSource = toolBar.ToolBarItems
                });
            }
            base.OnViewLoaded ( view );
        }

        #endregion
    }
}