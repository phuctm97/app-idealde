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

        public List<SettingsPage> Pages { get; private set; }

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
            _settingsEditors = new List<ISettingsEditor>();
            CancelCommand = new RelayCommand(p => TryClose(false));
            OkCommand = new RelayCommand(SaveChanges);
            DisplayName = "Settings";
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var pages = new List<SettingsPage>();
            _settingsEditors = IoC.GetAll<ISettingsEditor>();

            foreach (var settingsEditor in _settingsEditors)
            {
                var parentCollection = GetParentCollection(settingsEditor, pages);

                var page =
                    parentCollection.FirstOrDefault(m => m.Name == settingsEditor.SettingsPageName);

                if (page == null)
                {
                    page = new SettingsPage
                    {
                        Name = settingsEditor.SettingsPageName
                    };
                    parentCollection.Add(page);
                }

                page.Editors.Add(settingsEditor);
            }

            Pages = pages;
            SelectedPage = GetFirstLeafPageRecursive(Pages);
        }

        private SettingsPage GetFirstLeafPageRecursive(List<SettingsPage> pages)
        {
            if (!pages.Any())
                return null;

            var firstPage = pages.First();
            if (!firstPage.Children.Any())
                return firstPage;

            return GetFirstLeafPageRecursive(firstPage.Children);
        }

        private List<SettingsPage> GetParentCollection(ISettingsEditor settingsEditor,
            List<SettingsPage> pages)
        {
            if (string.IsNullOrEmpty(settingsEditor.SettingsPagePath))
            {
                return pages;
            }

            var path = settingsEditor.SettingsPagePath.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pathElement in path)
            {
                var page = pages.FirstOrDefault(s => s.Name == pathElement);

                if (page == null)
                {
                    page = new SettingsPage {Name = pathElement};
                    pages.Add(page);
                }

                pages = page.Children;
            }

            return pages;
        }

        #endregion

        // Behaviors

        #region Methods

        private void SaveChanges(object obj)
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