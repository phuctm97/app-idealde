#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Themes
{
    public class ThemeManager : IThemeManager
    {
        // Backing fields
        private ResourceDictionary _applicationResourceDictionary;

        // Logic properties
        public event EventHandler CurrentThemeChanged;

        public IEnumerable<ITheme> Themes { get; }

        public ITheme CurrentTheme { get; private set; }

        // Initializations
        public ThemeManager()
        {
            Themes = IoC.GetAll<ITheme>();
        }

        // Behaviors
        public bool SetCurrentTheme(string name)
        {
            var theme = Themes.FirstOrDefault(t => t.Name == name);
            if (theme == null) return false;

            CurrentTheme = theme;

            // add resource dictionary if not exist
            if (_applicationResourceDictionary == null)
            {
                _applicationResourceDictionary = new ResourceDictionary();
                Application.Current.Resources.MergedDictionaries.Add(_applicationResourceDictionary);
            }

            // begin update application and window resources
            _applicationResourceDictionary.BeginInit();
            _applicationResourceDictionary.MergedDictionaries.Clear();

            // get window resource dictionary
            var windowResourceDictionary = Application.Current.MainWindow.Resources.MergedDictionaries[0];
            windowResourceDictionary.BeginInit();
            windowResourceDictionary.MergedDictionaries.Clear();

            // load theme application resources
            foreach (var uri in theme.ApplicationResources)
            {
                _applicationResourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = uri
                });
            }

            // load main window resources
            foreach (var uri in theme.MainWindowResources)
            {
                windowResourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = uri
                });
            }

            // finish update
            _applicationResourceDictionary.EndInit();
            windowResourceDictionary.EndInit();
            
            // raise changed event
            CurrentThemeChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }
    }
}