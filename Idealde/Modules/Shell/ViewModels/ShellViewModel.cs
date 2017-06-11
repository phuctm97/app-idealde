#region Using Namespace

using System;
using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Services;

#endregion

namespace Idealde.Modules.Shell.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        public override void CanClose(Action<bool> callback)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Confirm", MessageBoxButton.YesNo);
            callback(result == MessageBoxResult.Yes);
        }
    }
}