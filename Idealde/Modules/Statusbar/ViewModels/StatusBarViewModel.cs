using System;
using System.Windows;
using Caliburn.Micro;
namespace Idealde.Modules.StatusBar.ViewModels
{
    public class StatusBarViewModel :PropertyChangedBase, IStatusBar
    {
        public IObservableCollection<StatusBarItem> Items { get; }

        public StatusBarViewModel()
        {
            Items = new StatusBarItemCollection();
        }

        public void AddItem(string message, GridLength width)
        {
            Items.Add(new StatusBarItem(message, width));
        }

        private class StatusBarItemCollection : BindableCollection<StatusBarItem>
        {
            protected override void InsertItemBase(int index, StatusBarItem item)
            {
                item.Index = index;
                base.InsertItemBase(index, item);
            }

            protected override void SetItemBase(int index, StatusBarItem item)
            {
                item.Index = index;
                base.SetItemBase(index, item);
            }
        }
    }
}
