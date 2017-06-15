using System.Windows.Input;
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
        // Bind properties
        ICommand HideCommand { get; set; }

        PaneLocation PreferredLocation { get; }

        double PreferredWidth { get; }

        double PreferredHeight { get; }

        bool IsVisible { get; set; }
    }
}