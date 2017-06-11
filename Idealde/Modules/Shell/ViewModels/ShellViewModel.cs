#region Using Namespace

using System.Windows;
using Caliburn.Micro;
using Idealde.Framework;
using Idealde.Framework.Services;
using Idealde.Modules.StatusBar;
using Idealde.Modules.Tests;

#endregion

namespace Idealde.Modules.Shell.ViewModels
{
    public class ShellViewModel : Conductor<ILayoutItem>.Collection.OneActive, IShell
    {
        private ILayoutItem _selectedDocument;

        // Bind models

        #region Bind models

        public IStatusBar StatusBar { get; }

        public IObservableCollection<ILayoutItem> Documents { get; }

        public IObservableCollection<ILayoutItem> Tools { get; }

        public ILayoutItem SelectedDocument
        {
            get { return _selectedDocument; }
            set
            {
                if (Equals(value, _selectedDocument)) return;
                ActivateItem(_selectedDocument);
            }
        }

        #endregion

        // Initializations
        #region Initializations

        public ShellViewModel(IStatusBar statusBar)
        {
            StatusBar = statusBar;
            Documents = new BindableCollection<ILayoutItem>
            {
                new LayoutItemTest {DisplayName = "Document 1"},
                new LayoutItemTest {DisplayName = "Document 2"},
                new LayoutItemTest {DisplayName = "Document 3"}
            };

            Tools = new BindableCollection<ILayoutItem>
            {
                new LayoutItemTest {DisplayName = "Tool 1"},
                new LayoutItemTest {DisplayName = "Tool 2"},
                new LayoutItemTest {DisplayName = "Tool 3"}
            };

            StatusBar.AddItem("Status 1", new GridLength(100));
            StatusBar.AddItem("Status 2", new GridLength(100));
            StatusBar.AddItem("Status 3", new GridLength(100));
        }

        #endregion

        // Item actions
        #region Item actions
        protected override void ChangeActiveItem(ILayoutItem newItem, bool closePrevious)
        {
            base.ChangeActiveItem(newItem, closePrevious);

            if (Documents.Contains(newItem))
            {
                _selectedDocument = newItem;
                NotifyOfPropertyChange(() => SelectedDocument);
            }
        }

        public void OpenDocument(ILayoutItem document)
        {
            throw new System.NotImplementedException();
        }

        public void CloseDocument(ILayoutItem document)
        {
            throw new System.NotImplementedException();
        }

        public void ShowTool(ILayoutItem tool)
        {
            throw new System.NotImplementedException();
        }

        public void ShowTool<TTool>() where TTool : ILayoutItem
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public void Close()
        {
            Application.Current.MainWindow.Close();
        }
    }
}