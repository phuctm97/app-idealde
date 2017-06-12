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

        // Bind properties
        public string ContentId { get; }

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => TryClose())); }
        }

        // Initializations
        protected Document()
        {
            ContentId = Guid.NewGuid().ToString();
        }
    }
}