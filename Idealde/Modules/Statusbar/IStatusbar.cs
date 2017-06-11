using System.Windows;
using Caliburn.Micro;
using Idealde.Modules.Statusbar.ViewModels;

namespace Idealde.Modules.Statusbar
{
    internal interface IStatusbar
    {
        IObservableCollection<StatusBarItemViewModel> Items { get; }

        void AddItem(string message, GridLength width);
    }
}