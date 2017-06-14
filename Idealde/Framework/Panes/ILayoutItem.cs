using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Modules.Shell.Commands;

namespace Idealde.Framework.Panes
{
    public interface ILayoutItem : IHaveDisplayName, INotifyPropertyChangedEx,
        ICommandHandler<CloseFileCommandDefinition>
    {
        // Bind properties
        string ContentId { get; }

        ICommand CloseCommand { get; }

        bool IsSelected { get; set; }
    }
}