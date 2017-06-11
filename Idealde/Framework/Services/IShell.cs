#region Using Namespace

using System.Windows.Controls.Primitives;
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

        // Active item (document or tool)
        ILayoutItem ActiveItem { get; set; }

        // Documents
        IDocument SelectedDocument { get; }

        IObservableCollection<IDocument> Documents { get; }
        void OpenDocument(IDocument document);
        void CloseDocument(IDocument document);

        // Tools
        IObservableCollection<ITool> Tools { get; }
        void ShowTool(ITool tool);
        void ShowTool<TTool>() where TTool : ITool;

        // Close
        void Close();
    }
}