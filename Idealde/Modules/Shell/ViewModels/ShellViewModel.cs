#region Using Namespace

using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;
using Idealde.Modules.StatusBar;
using Idealde.Modules.Tests;

#endregion

namespace Idealde.Modules.Shell.ViewModels
{
    public class ShellViewModel : Conductor<ILayoutItem>.Collection.OneActive, IShell
    {
        private IDocument _selectedDocument;

        // Bind models

        #region Bind models

        public IStatusBar StatusBar { get; }

        public IObservableCollection<IDocument> Documents { get; }

        public IObservableCollection<ITool> Tools { get; }

        public IDocument SelectedDocument
        {
            get { return _selectedDocument; }
            set
            {
                if (value == null || Equals(value, _selectedDocument)) return;
                OpenDocument(_selectedDocument);
            }
        }

        #endregion

        // Initializations

        #region Initializations

        public ShellViewModel(IStatusBar statusBar)
        {
            StatusBar = statusBar;
            Documents = new BindableCollection<IDocument>
            {
                new DocumentTest {DisplayName = "Document 1"},
                new DocumentTest {DisplayName = "Document 2"},
                new DocumentTest {DisplayName = "Document 3"}
            };

            Tools = new BindableCollection<ITool>
            {
                new ToolTest(PaneLocation.Left) {DisplayName = "Tool 1", IsVisible = true},
                new ToolTest(PaneLocation.Right) {DisplayName = "Tool 2", IsVisible = true},
                new ToolTest(PaneLocation.Bottom) {DisplayName = "Tool 3", IsVisible = true}
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

            //new item behaviors
            if (newItem is IDocument)
            {
                var document = (IDocument) newItem;
                if (!Documents.Contains(document))
                {
                    Documents.Add(document);
                }
                _selectedDocument = document;
                NotifyOfPropertyChange(() => SelectedDocument);
            }
            else if (newItem is ITool)
            {
                var tool = (ITool) newItem;
                if (!Tools.Contains(tool))
                {
                    Tools.Add(tool);
                }
                tool.IsVisible = true;
            }
        }

        public override void DeactivateItem(ILayoutItem item, bool close)
        {
            //check to remove document from list
            if (close && item is IDocument)
            {
                var document = (IDocument)item;
                Documents.Remove(document);
            }

            base.DeactivateItem(item, close);
        }

        public override void ActivateItem(ILayoutItem item)
        {
            base.ActivateItem(item);
        }

        public void OpenDocument(IDocument document)
        {
            ActivateItem(document);
        }

        public void CloseDocument(IDocument document)
        {
            DeactivateItem(document, true);
        }

        public void ShowTool(ITool tool)
        {
            ActivateItem(tool);
        }

        public void ShowTool<TTool>() where TTool : ITool
        {
            ActivateItem(IoC.Get<TTool>());
        }

        #endregion

        public void Close()
        {
            Application.Current.MainWindow.Close();
        }
    }
}