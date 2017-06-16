#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Modules.Settings.Models;

#endregion

namespace Idealde.Modules.Settings.ViewModels
{
    public sealed class SettingsViewModel : Screen
    {
        // Backing fields

        #region Backing fields

        private IEnumerable<ISettingsEditor> _settingsEditors;
        private SettingsPage _selectedPage;

        #endregion

        // Bind models
        
        #region Bind models

        public IObservableCollection<SettingsPage> Pages { get; }

        public SettingsPage SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                _selectedPage = value;
                NotifyOfPropertyChange(() => SelectedPage);
            }
        }

        #endregion

        // Bind properties

        #region Bind properties

        public ICommand CancelCommand { get; }

        public ICommand OkCommand { get; }

        #endregion

        // Initializations

        #region Initializations

        public SettingsViewModel()
        {
            OkCommand = new RelayCommand(p => SaveChanges());
            CancelCommand = new RelayCommand(p => TryClose(false));
            Pages = new BindableCollection<SettingsPage>();
            DisplayName = "Settings";
        }

        protected override void OnInitialize()
        {
            Pages.Clear();
            _settingsEditors = IoC.GetAll<ISettingsEditor>();

            foreach (var settingsEditor in _settingsEditors)
            {
                var parentPageCollection = GenerateAndGetParentPages(settingsEditor, Pages);

                var page =
                    parentPageCollection.FirstOrDefault(m => m.Name == settingsEditor.SettingsPageName);

                if (page == null)
                {
                    page = new SettingsPage
                    {
                        Name = settingsEditor.SettingsPageName
                    };
                    parentPageCollection.Add(page);
                }

                page.Editors.Add(settingsEditor);
            }

            SelectedPage = GetFirstLeafPage(Pages);

            base.OnInitialize();
        }

        private SettingsPage GetFirstLeafPage(ICollection<SettingsPage> pages)
        {
            while (true)
            {
                if (!pages.Any())
                    return null;

                var firstPage = pages.First();
                if (!firstPage.Children.Any())
                    return firstPage;

                pages = firstPage.Children;
            }
        }

        private ICollection<SettingsPage> GenerateAndGetParentPages(ISettingsEditor editor,
            ICollection<SettingsPage> pages)
        {
            if (string.IsNullOrEmpty(editor.SettingsPagePath))
            {
                return pages;
            }

            var parentPageNames = editor.SettingsPagePath.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pageName in parentPageNames)
            {
                var page = pages.FirstOrDefault(s => s.Name == pageName);

                if (page == null)
                {
                    page = new SettingsPage {Name = pageName};
                    pages.Add(page);
                }

                pages = page.Children;
            }

            return pages;
        }

        #endregion

        // Behaviors

        #region Behaviors

        private void SaveChanges()
        {
            var q = true;
            foreach (var settingsEditor in _settingsEditors)
            {
                var e = settingsEditor.ApplyChanges();
                if (!e) q = false;
            }
            if (q)
                TryClose(true);
        }

        #endregion
    }
}