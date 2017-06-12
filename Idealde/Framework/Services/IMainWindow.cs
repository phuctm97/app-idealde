using System.Windows;
using System.Windows.Media;

namespace Idealde.Framework.Services
{
    public interface IMainWindow
    {
        // Bind models
        IShell Shell { get; }
     
        // Bind properties
        string Title { get; set; }

        ImageSource Icon { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        WindowState WindowState { get; set; }
    }
}