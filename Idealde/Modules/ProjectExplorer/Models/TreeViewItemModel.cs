using System;
using Caliburn.Micro;

namespace Idealde.Modules.ProjectExplorer.Models
{
    public enum DirType
    {
        Default= 0,
        Root = 1,
        FolderClosed = 2,
        FolderOpened = 3,
        File = 4
    }

    public class TreeViewItemModel : PropertyChangedBase
    {
        #region Definition
        private Uri _imageSource;
        private int _fontSize;
        private DirType _objectType;
        private string _name;
        private bool _expanded;


        // Image source for each file type
        private const string RootImage =
            @"pack://application:,,,/Idealde;Component/Modules/SolutionExplorer/IconSource/solution-explorer.png";
        private const string FolderClosedImage =
            @"pack://application:,,,/Idealde;Component/Modules/SolutionExplorer/IconSource/folder-closed.png";
        private const string FolderOpenedImage =
            @"pack://application:,,,/Idealde;Component/Modules/SolutionExplorer/IconSource/folder-opened.png";
        private const string FileImage =
            @"pack://application:,,,/Idealde;Component/Modules/SolutionExplorer/IconSource/file.png";

        #endregion

        #region Initializtion
        // Initialization of each item
        public TreeViewItemModel(string name, string path)
        {
            SubItems = new BindableCollection<TreeViewItemModel>();
            Name = name.Substring(name.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            Path = path;
            FontSize = 12;
        }
        #endregion

        #region Properties
        public string Path { get; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (Equals(value, _name)) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public DirType ObjectType
        {
            get { return _objectType; }
            set
            {
                if (_objectType.Equals(value)) return;
                _objectType = value;
                NotifyOfPropertyChange(() => ObjectType);
                ChangeImageSource();
            }
        }

        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize.Equals(value)) return;
                _fontSize = value;
                NotifyOfPropertyChange(() => FontSize);
            }
        }

        // private set whenever ObjectType changed
        #region Automatic image source
        public Uri ImageSource
        {
            get { return _imageSource; }
            private set
            {
                if (Equals(_imageSource, value)) return;
                _imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }
        private void ChangeImageSource()
        {
            switch (ObjectType)
            {
                case DirType.FolderClosed:
                    ImageSource = new Uri(FolderClosedImage, UriKind.Absolute);
                    break;
                case DirType.FolderOpened:
                    ImageSource = new Uri(FolderOpenedImage, UriKind.Absolute);
                    break;
                case DirType.Root:
                    ImageSource = new Uri(RootImage, UriKind.Absolute);
                    break;
                case DirType.File:
                    ImageSource = new Uri(FileImage, UriKind.Absolute);
                    break;
                default:
                    break;
            }
        }
        #endregion // automatic image source

        #endregion

        #region Item contains
        public IObservableCollection<TreeViewItemModel> SubItems { get; set; }
        #endregion

        #region On item expanded ( Change image source )
        // On item expanded
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (value.Equals(_expanded)) return;
                _expanded = value;
                NotifyOfPropertyChange(() => Expanded);
                OnItemExpanded(_expanded);
            }
        }

        private void OnItemExpanded(bool expanded)
        {
            switch (ObjectType)
            {
                case DirType.FolderClosed:
                    ObjectType = DirType.FolderOpened;
                    break;
                case DirType.FolderOpened:
                    ObjectType = DirType.FolderClosed;
                    break;
                default:
                    break;
            }
        }

        #endregion // On item expanded
    }
}