#region Using Namespace

using Caliburn.Micro;
using Idealde.Modules.StatusBar;

#endregion

namespace Idealde.Framework.Services
{
    public interface IShell : IGuardClose, IDeactivate
    {
        // Dependencies
        IStatusBar StatusBar { get; }

        // Active item (document or tool)
        ILayoutItem ActiveItem { get; set; }

        // Documents
        ILayoutItem SelectedDocument { get; set; }

        IObservableCollection<ILayoutItem> Documents { get; }
        void OpenDocument(ILayoutItem document);
        void CloseDocument(ILayoutItem document);

        // Tools
        IObservableCollection<ILayoutItem> Tools { get; }
        void ShowTool(ILayoutItem tool);
        void ShowTool<TTool>() where TTool : ILayoutItem;

        // Close
        void Close();
    }
}