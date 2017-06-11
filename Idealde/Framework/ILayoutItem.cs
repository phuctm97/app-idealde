using Caliburn.Micro;

namespace Idealde.Framework
{
    public interface ILayoutItem : IScreen
    {   
        string ContentId { get; }
    }
}