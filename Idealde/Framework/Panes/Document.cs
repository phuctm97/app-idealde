#region Using Namespace

using System;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;

#endregion

namespace Idealde.Framework.Panes
{
    public abstract class Document : Screen, IDocument
    {
        // Backing fields
        private ICommand _closeCommand;
        private bool _isSelected;

        // Bind properties
        public string ContentId { get; }

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => TryClose())); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }

        // Initializations
        protected Document()
        {
            _isSelected = false;

            ContentId = Guid.NewGuid().ToString();
        }

    }
}