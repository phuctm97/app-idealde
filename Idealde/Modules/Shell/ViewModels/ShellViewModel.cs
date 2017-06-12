#region Using Namespace

using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;
using Idealde.Modules.CodeEditor.ViewModels;
using Idealde.Modules.MainMenu;
using Idealde.Modules.StatusBar;
using Idealde.Modules.Tests.ViewModels;

#endregion

namespace Idealde.Modules.Shell.ViewModels
{
    public class ShellViewModel : Conductor<IDocument>.Collection.OneActive, IShell
    {
        // Backing fields
        private bool _closing;
        private ILayoutItem _activeLayoutItem;

        // Bind models
        public IMenu MainMenu { get; }

        public IStatusBar StatusBar { get; }

        public ILayoutItem ActiveLayoutItem
        {
            get { return _activeLayoutItem; }
            set
            {
                if (Equals(value, _activeLayoutItem)) return;
                _activeLayoutItem = value;

                if (_activeLayoutItem is IDocument)
                {
                    OpenDocument((IDocument) _activeLayoutItem);
                }
                if (_activeLayoutItem is ITool)
                {
                    ShowTool((ITool) _activeLayoutItem);
                }
                NotifyOfPropertyChange(() => ActiveLayoutItem);
            }
        }

        public IObservableCollection<IDocument> Documents => Items;

        public IObservableCollection<ITool> Tools { get; }

        // Initializations

        public ShellViewModel(IMenu mainMenu, IStatusBar statusBar)
        {
            MainMenu = mainMenu;

            StatusBar = statusBar;

            Tools = new BindableCollection<ITool>();

            _closing = false;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            OpenDocument(new DocumentTestViewModel {DisplayName = "Document 1"});
            OpenDocument(new DocumentTestViewModel {DisplayName = "Document 2"});
            OpenDocument(new CodeEditorViewModel());

            ShowTool(new ToolTestViewModel(PaneLocation.Left) {DisplayName = "Tool 1"});
            ShowTool(new ToolTestViewModel(PaneLocation.Right) {DisplayName = "Tool 2"});
            ShowTool(new ToolTestViewModel(PaneLocation.Bottom) {DisplayName = "Tool 3"});
        }

        // Item actions

        public override void ActivateItem(IDocument item)
        {
            //bug: complex bug, temporary solution
            if (_closing) return;

            base.ActivateItem(item);
        }

        protected override void OnActivationProcessed(IDocument item, bool success)
        {
            base.OnActivationProcessed(item, success);

            if (!success) return;

            if (Equals(item, _activeLayoutItem)) return;

            _activeLayoutItem = item;
            NotifyOfPropertyChange(() => ActiveLayoutItem);
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

        protected override void OnDeactivate(bool close)
        {
            //bug: complex bug, temporary solution
            _closing = close;

            base.OnDeactivate(close);
        }
    }
}