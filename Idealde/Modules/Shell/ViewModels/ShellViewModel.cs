#region Using Namespace

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;
using Idealde.Modules.StatusBar;
using Idealde.Modules.Tests.ViewModels;

#endregion

namespace Idealde.Modules.Shell.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        // Backing fields

        #region Backing fields

        private ILayoutItem _activeItem;

        #endregion

        // Bind models

        #region Bind models

        public IStatusBar StatusBar { get; }

        public ILayoutItem ActiveItem
        {
            get { return _activeItem; }
            set
            {
                if (Equals(value, _activeItem)) return;
                _activeItem = value;

                if (_activeItem is IDocument)
                {
                    OpenDocument((IDocument) _activeItem);
                }
                else if (_activeItem is ITool)
                {
                    ShowTool((ITool) _activeItem);
                }

                NotifyOfPropertyChange(() => ActiveItem);
            }
        }

        public IDocument SelectedDocument { get; private set; }

        public IObservableCollection<IDocument> Documents { get; }

        public IObservableCollection<ITool> Tools { get; }

        #endregion

        // Initializations

        #region Initializations

        public ShellViewModel(IStatusBar statusBar)
        {
            StatusBar = statusBar;

            Documents = new BindableCollection<IDocument>();
            Documents.CollectionChanged += OnDocumentsCollectionChanged;

            Tools = new BindableCollection<ITool>();
            Tools.CollectionChanged += OnToolsCollectionChanged;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Documents.AddRange(new IDocument[]
            {
                new DocumentTestViewModel {DisplayName = "Document 1"},
                new DocumentTestViewModel {DisplayName = "Document 2"},
                new DocumentTestViewModel {DisplayName = "Document 3"}
            });

            Tools.AddRange(new ITool[]
            {
                new ToolTestViewModel(PaneLocation.Left) {DisplayName = "Tool 1", IsVisible = true},
                new ToolTestViewModel(PaneLocation.Right) {DisplayName = "Tool 2", IsVisible = true},
                new ToolTestViewModel(PaneLocation.Bottom) {DisplayName = "Tool 3", IsVisible = true}
            });

            StatusBar.AddItem("Status 1", new GridLength(100));
            StatusBar.AddItem("Status 2", new GridLength(100));
            StatusBar.AddItem("Status 3", new GridLength(100));
        }

        #endregion

        // Item actions

        #region Item actions

        public void OpenDocument(IDocument document)
        {
            if (Equals(document, SelectedDocument)) return;

            //deactivate old document
            SelectedDocument?.Deactivate(false);

            //add to opened documents
            if (!Documents.Contains(document))
            {
                Documents.Add(document);
            }

            //update selected one
            SelectedDocument = document;
            SelectedDocument.Activate();
            NotifyOfPropertyChange(() => SelectedDocument);
        }

        public void CloseDocument(IDocument document)
        {
            ActiveItem = ChooseNextActiveItem(document);

            document.Deactivate(true);
        }

        public void ShowTool(ITool tool)
        {
            if (!Tools.Contains(tool))
            {
                Tools.Add(tool);
            }
            tool.IsVisible = true;
        }

        public void ShowTool<TTool>() where TTool : ITool
        {
            ShowTool(IoC.Get<TTool>());
        }

        protected virtual ILayoutItem ChooseNextActiveItem(ILayoutItem oldItem)
        {
            if (oldItem is ITool)
            {
                return SelectedDocument;
            }

            if (oldItem is IDocument)
            {
                for (var i = 0; i < Documents.Count - 1; i++)
                {
                    if (oldItem == Documents[i + 1])
                    {
                        return Documents[i];
                    }
                }
            }

            return null;
        }

        private void OnToolsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           
        }

        private void OnDocumentsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems?.OfType<IDocument>().Apply(p => p.Parent = this);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    var collection = sender as IObservableCollection<IDocument>;
                    collection?.Apply(p => p.Parent = this);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    e.OldItems?.OfType<IDocument>().Apply(p => p.Parent = null);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    e.OldItems?.OfType<IDocument>().Apply(p => p.Parent = null);
                    e.NewItems?.OfType<IDocument>().Apply(p => p.Parent = this);
                    break;
            }
        }

        #endregion

        // Closing

        #region Closing

        public void Close()
        {
            Application.Current.MainWindow.Close();
        }

        #endregion
    }
}