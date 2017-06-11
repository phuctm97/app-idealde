using System.Windows.Input;
using Caliburn.Micro;

namespace Idealde.Framework.Panes
{
    public interface ILayoutItem : IScreen
    {   
        string ContentId { get; }

        ICommand CloseCommand { get; }
    }
}