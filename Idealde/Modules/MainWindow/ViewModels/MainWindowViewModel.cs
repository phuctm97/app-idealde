#region Using Namespace

using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using Idealde.Framework.Services;

#endregion

namespace Idealde.Modules.MainWindow.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase, IMainWindow
    {
        // Backing fields
        private string _title;
        private ImageSource _icon;
        private double _width;
        private double _height;
        private WindowState _windowState;

        // Bind properties
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                if (Equals(value, _icon)) return;
                _icon = value;
                NotifyOfPropertyChange(() => Icon);
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                if (value.Equals(_width)) return;
                _width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                if (value.Equals(_height)) return;
                _height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                if (value == _windowState) return;
                _windowState = value;
                NotifyOfPropertyChange(() => WindowState);
            }
        }

        // Initializations
        public MainWindowViewModel()
        {
            Title = @"Idealde";

            Width = 1280;

            Height = 720;

            WindowState = WindowState.Normal;
        }
    }
}