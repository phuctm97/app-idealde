using System.Windows.Input;
using Caliburn.Micro;

namespace Idealde.Framework.Panes
{
    public interface ILayoutItem : IHaveDisplayName
    {
        string ContentId { get; }

        ICommand CloseCommand { get; }

        bool IsSelected { get; set; }
    }
}