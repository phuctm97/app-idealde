#region Using Namespace

using System.Windows;
using System.Windows.Input;
using Idealde.Framework.Commands;

#endregion

namespace Idealde.Framework.Panes
{
    public abstract class Document : LayoutItem, IDocument
    {
        // Backing fields

        #region Backing fields

        protected ICommand _closeCommand;

        #endregion

        // Bind properties
        #region Bind properties
        public override ICommand CloseCommand
        {
            get { return _closeCommand ?? new RelayCommand(p => TryClose(), p => true); }
        } 
        #endregion

        // Initializations
        #region Initializations

        protected Document()
        {
            _closeCommand = null;
        } 
        #endregion
    }
}