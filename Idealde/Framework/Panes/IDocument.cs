using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Modules.UndoRedo;
using Idealde.Modules.UndoRedo.Commands;

namespace Idealde.Framework.Panes
{
    public interface IDocument : IScreen, ILayoutItem,
        ICommandHandler<UndoCommandDefinition>,
        ICommandHandler<RedoCommandDefinition>
    {
        IUndoRedoManager UndoRedoManager { get; }
    }
}