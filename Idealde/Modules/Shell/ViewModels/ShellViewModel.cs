#region Using Namespace

using Caliburn.Micro;
using Idealde.Framework;
using Idealde.Framework.Services;
using Idealde.Modules.Tests;

#endregion

namespace Idealde.Modules.Shell.ViewModels
{
    public class ShellViewModel : Conductor<ILayoutItem>.Collection.OneActive, IShell
    {
        private ILayoutItem _selectedDocument;
        // Bind models

        #region Bind models

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

        public ShellViewModel()
        {
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
        }

        #endregion

        protected override void ChangeActiveItem(ILayoutItem newItem, bool closePrevious)
        {
            base.ChangeActiveItem(newItem, closePrevious);

            if (Documents.Contains(newItem))
            {
                _selectedDocument = newItem;
                NotifyOfPropertyChange(() => SelectedDocument);
            }
        }
    }
}