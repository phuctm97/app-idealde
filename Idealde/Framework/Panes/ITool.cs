namespace Idealde.Framework.Panes
{
    public enum PaneLocation
    {
        Left,
        Right,
        Bottom
    }

    public interface ITool : ILayoutItem
    {
        PaneLocation PreferredLocation { get; }

        double PreferredWidth { get; set; }

        double PreferredHeight { get; set; }

        bool IsVisible { get; set; }
    }
}