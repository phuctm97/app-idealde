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
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            ActivateItem(new DocumentTestViewModel {DisplayName = "Document 1"});
            ActivateItem(new DocumentTestViewModel {DisplayName = "Document 2"});
            ActivateItem(new DocumentTestViewModel {DisplayName = "Document 3"});
        }
    }
}