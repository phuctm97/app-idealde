#region Using Namespace

using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;
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

        // Bind models
        public IMenu MainMenu { get; }

        public IStatusBar StatusBar { get; }

        public IDocument SelectedDocument
        {
            get { return ActiveItem; }
            set { ActiveItem = value; }
        }

        public IObservableCollection<IDocument> Documents => Items;

        // Initializations
        public ShellViewModel(IMenu mainMenu, IStatusBar statusBar)
        {
            MainMenu = mainMenu;
            StatusBar = statusBar;

            _closing = false;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            ActivateItem(new DocumentTestViewModel {DisplayName = "Document 1"});
            ActivateItem(new DocumentTestViewModel {DisplayName = "Document 2"});
            ActivateItem(new DocumentTestViewModel {DisplayName = "Document 3"});
        }

        // Item actions
        public override void ActivateItem(IDocument item)
        {
            //bug: complex bug, temporary solution
            if (_closing) return;

            base.ActivateItem(item);
        }

        protected override void OnDeactivate(bool close)
        {
            //bug: complex bug, temporary solution
            _closing = close;

            base.OnDeactivate(close);
        }
    }
}