using System;
using System.Windows.Input;
using Idealde.Framework.Commands;

namespace Idealde.Framework.Panes
{
    public abstract class Document : LayoutItem, IDocument
    {
        private ICommand _closeCommand;

        public override ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => TryClose())); }
        }
    }
}