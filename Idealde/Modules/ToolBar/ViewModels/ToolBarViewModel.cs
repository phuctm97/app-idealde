#region Using Namespace

using System;
using System.Collections.Specialized;

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

        public IObservableCollection<Model.ToolBar> Items { get; }

        private static IToolBarView _toolBarView;
        #endregion

        #region Initiliazation

        public ToolBarViewModel()
        {
            Items = new BindableCollection<Model.ToolBar>();
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _toolBarView.ToolBarTray.ToolBars.Clear();
            foreach (var toolBar in Items)
            {
                _toolBarView.ToolBarTray.ToolBars.Add(new ToolBarBase()
                {
                    ItemsSource = toolBar.ToolBarItems
                });
            }
        }

        #endregion

        #region Methods

        public void AddToolBarItem(Model.ToolBar parent, params ToolBarItemBase[] toolBarItems)
        {
            foreach (var toolBarItem in toolBarItems)
            {
                parent.ToolBarItems.Add(toolBarItem);
            }
            Items.Add(parent);
        }

        public void AddToolBar(params Model.ToolBar[] toolBars)
        {
            foreach (var toolBar in toolBars)
            {
                Items.Add(toolBar);
            }
        }

        protected override void OnViewLoaded ( object view )
        {
            _toolBarView = (IToolBarView)view;

            foreach (var toolBar in Items)
            {
                _toolBarView.ToolBarTray.ToolBars.Add(new ToolBarBase()
                {
                    ItemsSource = toolBar.ToolBarItems
                });
            }
            base.OnViewLoaded ( view );
        }

        #endregion
    }
}