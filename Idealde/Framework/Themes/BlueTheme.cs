#region Using Namespace

using System;
using System.Collections.Generic;

#endregion

namespace Idealde.Framework.Themes
{
    public class BlueTheme : ITheme
    {
        public string Name => "Blue";

        public IEnumerable<Uri> ApplicationResources
        {
            get
            {
                yield return
                    new Uri("pack://application:,,,/Xceed.Wpf.AvalonDock.Themes.VS2013;component/BlueTheme.xaml");
                yield return new Uri("pack://application:,,,/Idealde;component/Themes/VS2013/BlueTheme.xaml");
            }
        }

        public IEnumerable<Uri> MainWindowResources
        {
            get { yield break; }
        }
    }
}