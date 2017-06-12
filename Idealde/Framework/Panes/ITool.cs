using Caliburn.Micro;

namespace Idealde.Framework.Panes
{
    public enum PaneLocation
    {
        Left,
        Right,
        Bottom
    }

    public interface ITool : IViewAware, ILayoutItem
    {
        PaneLocation PreferredLocation { get; }

        double PreferredWidth { get; }

        double PreferredHeight { get; }

        bool IsVisible { get; set; }
    }
}