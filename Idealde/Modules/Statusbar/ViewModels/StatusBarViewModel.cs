using System;
using System.Windows;
using Caliburn.Micro;
namespace Idealde.Modules.Statusbar.ViewModels
{
    public class StatusBarViewModel :PropertyChangedBase, IStatusbar
    {
        private readonly IObservableCollection<StatusBarItemViewModel> _items;
        public IObservableCollection<StatusBarItemViewModel> Items => _items;

        public StatusBarViewModel()
        {
            _items = new StatusBarItemCollection();
        }

        public void AddItem(string message, GridLength width)
        {
            Items.Add(new StatusBarItemViewModel(message, width));
        }

        private class StatusBarItemCollection : BindableCollection<StatusBarItemViewModel>
        {
            protected override void InsertItemBase(int index, StatusBarItemViewModel item)
            {
                item.Index = index;
                base.InsertItemBase(index, item);
            }

            protected override void SetItemBase(int index, StatusBarItemViewModel item)
            {
                item.Index = index;
                base.SetItemBase(index, item);
            }
        }
    }
}
