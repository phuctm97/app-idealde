#region Using Namespace

using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Modules.MainMenu;
using Idealde.Modules.StatusBar;

#endregion

namespace Idealde.Framework.Services
{
    public interface IShell : IGuardClose, IDeactivate
    {
        // Dependencies
        IMenu MainMenu { get; }

        IStatusBar StatusBar { get; }

        // Bind models
        ILayoutItem ActiveItem { get; set; }

        IDocument SelectedDocument { get; }

        IObservableCollection<IDocument> Documents { get; }

        IObservableCollection<ITool> Tools { get; }
    }
}