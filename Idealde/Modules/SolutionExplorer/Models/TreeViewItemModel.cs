using System;
using Caliburn.Micro;

namespace Idealde.Modules.SolutionExplorer.Models
{
    public enum DirType
    {
        Folder = 0,
        File = 1
    }

    public class TreeViewItemModel : PropertyChangedBase
    {
        private Uri _imageSource;
        private int _fontSize;
        private int _objectType;
        private string _name;

        public TreeViewItemModel(string name, string path)
        {
            SubItems = new BindableCollection<TreeViewItemModel>();
            Name = name.Substring(name.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            Path = path;
            FontSize = 12;
        }

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

        public int ObjectType
        {
            get { return _objectType; }
            set
            {
                if (_objectType.Equals(value)) return;
                _objectType = value;
                NotifyOfPropertyChange(() => ObjectType);
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

        public Uri ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (Equals(_imageSource, value)) return;
                _imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

        public IObservableCollection<TreeViewItemModel> SubItems { get; set; }
    }
}