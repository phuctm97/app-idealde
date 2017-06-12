#region Using Namespace

using Caliburn.Micro;

#endregion

namespace Idealde.Modules.ErrorList
{
    public enum ErrorListItemType
    {
        Error,
        Warning,
        Message
    }

    public class ErrorListItem : PropertyChangedBase
    {
        // Backing fields

        #region Backing fields

        private ErrorListItemType _type;
        private int _code;
        private string _description;
        private string _path;
        private int? _line;
        private int? _column;

        #endregion

        // Bind properties

        #region Bind properties

        public ErrorListItemType Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                NotifyOfPropertyChange(() => Type);
            }
        }

        public int Code
        {
            get { return _code; }
            set
            {
                if (value == _code) return;
                _code = value;
                NotifyOfPropertyChange(() => Code);
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                if (value == _path) return;
                _path = value;
                NotifyOfPropertyChange(() => Path);
                NotifyOfPropertyChange(() => File);
            }
        }

        public string File => System.IO.Path.GetFileName(Path);

        public int? Line
        {
            get { return _line; }
            set
            {
                if (value == _line) return;
                _line = value;
                NotifyOfPropertyChange(() => Line);
            }
        }

        public int? Column
        {
            get { return _column; }
            set
            {
                if (value == _column) return;
                _column = value;
                NotifyOfPropertyChange(() => Column);
            }
        }

        public System.Action OnClick { get; set; }

        #endregion

        // Initializations

        #region Initializations

        public ErrorListItem(ErrorListItemType type, int code, string description,
            string path = null, int? line = null, int? column = null)
        {
            _type = type;
            _code = code;
            _description = description;
            _path = path;
            _line = line;
            _column = column;
        }

        public ErrorListItem()
        {
        }

        #endregion
    }
}