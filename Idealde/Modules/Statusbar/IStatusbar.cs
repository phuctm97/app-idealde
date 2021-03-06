﻿using System.Windows;
using Caliburn.Micro;
using Idealde.Modules.StatusBar.ViewModels;

namespace Idealde.Modules.StatusBar
{
    public interface IStatusBar
    {
        IObservableCollection<StatusBarItem> Items { get; }

        void AddItem(string message, GridLength width);
    }
}