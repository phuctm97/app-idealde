using Caliburn.Micro;

namespace Idealde.Framework.Panes
{
    public interface ILayoutItem : IScreen
    {   
        string ContentId { get; }
    }
}