using System;
using System.Windows;
using Caliburn.Micro;
namespace Idealde.Modules.StatusBar.ViewModels
{
    public class StatusBarViewModel :PropertyChangedBase, IStatusBar
    {
        public IObservableCollection<StatusBarItemBase> Items { get; }

        public StatusBarViewModel()
        {
            Items = new StatusBarItemCollection();
        }

        public void AddItem(string message, GridLength width)
        {
            Items.Add(new StatusBarItemBase(message, width));
        }

        private class StatusBarItemCollection : BindableCollection<StatusBarItemBase>
        {
            protected override void InsertItemBase(int index, StatusBarItemBase item)
            {
                item.Index = index;
                base.InsertItemBase(index, item);
            }

            protected override void SetItemBase(int index, StatusBarItemBase item)
            {
                item.Index = index;
                base.SetItemBase(index, item);
            }
        }
    }
}
