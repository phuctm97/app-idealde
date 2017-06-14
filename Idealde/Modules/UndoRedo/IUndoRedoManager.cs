#region Using Namespace

using Caliburn.Micro;

#endregion

namespace Idealde.Modules.UndoRedo
{
    public interface IUndoRedoManager
    {
        IObservableCollection<IUndoableAction> UndoStack { get; }
        IObservableCollection<IUndoableAction> RedoStack { get; }

        void Execute(IUndoableAction action);

        void Undo(int actionCount);
        void UndoTo(IUndoableAction action);
        void UndoAll();

        void Redo(int actionCount);
        void RedoTo(IUndoableAction action);
    }
}