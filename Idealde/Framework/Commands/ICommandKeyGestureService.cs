#region Using Namespace

using System.Windows;
using System.Windows.Input;

#endregion

namespace Idealde.Framework.Commands
{
    public interface ICommandKeyGestureService
    {
        void BindKeyGestures(UIElement uiElement);

        KeyGesture GetPrimaryKeyGesture(CommandDefinition commandDefinition);
    }
}