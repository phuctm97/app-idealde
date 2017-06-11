#region Using Namespace

using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Services
{
    public interface IShell : IGuardClose, IDeactivate
    {
        ILayoutItem ActiveItem { get; set; }

        ILayoutItem SelectedDocument { get; set; }

        IObservableCollection<ILayoutItem> Documents { get; }
        void OpenDocument(ILayoutItem document);
        void CloseDocument(ILayoutItem document);

        IObservableCollection<ILayoutItem> Tools { get; }
        void ShowTool(ILayoutItem tool);
        void ShowTool<TTool>() where TTool : ILayoutItem;

        void Close();
    }
}