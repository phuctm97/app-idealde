using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Modules.Settings.Models;
using Idealde.Modules.Settings.Options.Compiler.ViewModels;

namespace Idealde.Modules.Settings.ViewModels
{
    public sealed class SettingsViewModel : Screen
    {
        // Backing field

        #region Backing field

        private readonly List<ISettingsEditor> _settingsEditors;
        private SettingsPage _selectedPage;

        #endregion

        // Initialization

        #region Initialization

        public SettingsViewModel()
        {
            _settingsEditors = new List<ISettingsEditor>();
            CancelCommand = new RelayCommand(o => TryClose(false));
            OkCommand = new RelayCommand(SaveChanges);
            DisplayName = "Settings";
            _settingsEditors.Add(new CompilerSettingsViewModel());
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var pages = new List<SettingsPage>();

            foreach (ISettingsEditor settingsEditor in _settingsEditors)
            {
                List<SettingsPage> parentCollection = GetParentCollection(settingsEditor, pages);

                SettingsPage page =
                    parentCollection.FirstOrDefault(m => m.Name == settingsEditor.SettingsPageName);

                if (page == null)
                {
                    page = new SettingsPage
                    {
                        Name = settingsEditor.SettingsPageName,
                    };
                    parentCollection.Add(page);
                }

                page.Editors.Add(settingsEditor);
            }

            Pages = pages;
            SelectedPage = GetFirstLeafPageRecursive(Pages);
        }

        #endregion

        // Methods

        #region Methods

        public void AddSettings(ISettingsEditor settingsEditor)
        {
            _settingsEditors.Add(settingsEditor);
        }

        #endregion

        // Commands

        #region Button Command

        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
        private void SaveChanges(object obj)
        {
            var q = true;
            foreach (ISettingsEditor settingsEditor in _settingsEditors)
            {
                var e = settingsEditor.ApplyChanges();
                if (!e) q = false;
            }
            if (q)
            TryClose(true);
        }

        #endregion


        // Properties

        #region Properties
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

        // Initialize methods

        #region Initial & Add setting editor method

        // Get the first setting to view on view active
        private static SettingsPage GetFirstLeafPageRecursive(List<SettingsPage> pages)
        {
            if (!pages.Any())
                return null;

            var firstPage = pages.First();
            if (!firstPage.Children.Any())
                return firstPage;

            return GetFirstLeafPageRecursive(firstPage.Children);
        }

        // Get parents setting of settingsEditor
        private List<SettingsPage> GetParentCollection(ISettingsEditor settingsEditor,
            List<SettingsPage> pages)
        {
            if (string.IsNullOrEmpty(settingsEditor.SettingsPagePath))
            {
                return pages;
            }

            string[] path = settingsEditor.SettingsPagePath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string pathElement in path)
            {
                SettingsPage page = pages.FirstOrDefault(s => s.Name == pathElement);

                if (page == null)
                {
                    page = new SettingsPage { Name = pathElement };
                    pages.Add(page);
                }

                pages = page.Children;
            }

            return pages;
        }

        #endregion
    }
}
