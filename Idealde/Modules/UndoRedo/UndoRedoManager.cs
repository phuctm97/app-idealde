using Caliburn.Micro;

namespace Idealde.Modules.UndoRedo
{
    public class UndoRedoManager : IUndoRedoManager
    {
        public IObservableCollection<IUndoableAction> UndoStack { get; }
        public IObservableCollection<IUndoableAction> RedoStack { get; }

        public UndoRedoManager()
        {
            UndoStack = new BindableCollection<IUndoableAction>();

            RedoStack = new BindableCollection<IUndoableAction>();
        }

        public void Execute(IUndoableAction action)
        {
            action.Execute();
            Push(UndoStack, action);
            RedoStack.Clear();
        }

        public void Undo(int actionCount)
        {
            for (int i = 0; i < actionCount; i++)
            {
                var action = Pop(UndoStack);
                if (action == null) continue;

                action.Undo();
                Push(RedoStack, action);
            }
        }

        public void UndoTo(IUndoableAction action)
        {
            while (Peek(UndoStack) != action)
            {
                var currentAction = Pop(UndoStack);
                currentAction.Undo();
                Push(RedoStack, currentAction);
            }
        }

        public void UndoAll()
        {
            Undo(UndoStack.Count);
        }

        public void Redo(int actionCount)
        {
            for (int i = 0; i < actionCount; i++)
            {
                var action = Pop(RedoStack);
                action.Execute();
                Push(UndoStack, action);
            }
        }

        public void RedoTo(IUndoableAction action)
        {
            while (true)
            {
                var currentAction = Pop(RedoStack);
                currentAction.Execute();
                Push(UndoStack, currentAction);
                if (currentAction == action)
                    return;
            }
        }

        private void Push(IObservableCollection<IUndoableAction> stack, IUndoableAction action)
        {
            UndoStack.Add(action);
        }

        private IUndoableAction Peek(IObservableCollection<IUndoableAction> stack)
        {
            return stack.Count == 0 ? null : stack[stack.Count - 1];
        }

        private IUndoableAction Pop(IObservableCollection<IUndoableAction> stack)
        {
            if (stack.Count == 0) return null;

            var r = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
            return r;
        }
    }
}