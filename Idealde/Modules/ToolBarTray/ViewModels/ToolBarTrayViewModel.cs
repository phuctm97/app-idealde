#region Using Namespace

using System.Collections.Specialized;
using Caliburn.Micro;
using Idealde.Modules.ToolBarTray.Controls;
using Idealde.Modules.ToolBarTray.Models;
using Idealde.Modules.ToolBarTray.Views;

#endregion

namespace Idealde.Modules.ToolBarTray.ViewModels
{
    class ToolBarTrayViewModel : ViewAware, IToolBarTray
    {
        // Backing fields

        #region Backing fields

        private IToolBarView _toolBarView;

        #endregion

        // Bind models

        #region Bind models

        public IObservableCollection<ToolBar> Items { get; }

        #endregion

        // Initializations

        #region Initializations

        public ToolBarTrayViewModel()
        {
            Items = new BindableCollection<ToolBar>();
            Items.CollectionChanged += OnItemsCollectionChanged;
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshToolBarTray();
        }

        protected override void OnViewLoaded(object view)
        {
            _toolBarView = (IToolBarView) view;
            RefreshToolBarTray();

            base.OnViewLoaded(view);
        }

        private void RefreshToolBarTray()
        {
            _toolBarView?.ToolBarTray.ToolBars.Clear();
            foreach (var toolBar in Items)
            {
                _toolBarView?.ToolBarTray.ToolBars.Add(new ToolBarBase
                {
                    ItemsSource = toolBar.Items
                });
            }
        }

        #endregion

        // Behaviors

        #region Behaviors

        public void AddToolBarItem(ToolBar parent, params ToolBarItemBase[] toolBarItemsBase)
        {
            foreach (var toolBarItem in toolBarItemsBase)
            { 
                parent.Items.Add(toolBarItem);
            }
        }

        public void AddToolBar(params ToolBar[] toolBars)
        {
            foreach (var toolBar in toolBars)
            {
                Items.Add(toolBar);
            }
        }

        #endregion
    }
}