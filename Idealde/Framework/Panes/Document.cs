#region Using Namespace

using System.Windows.Input;
using Idealde.Framework.Commands;
using Idealde.Framework.Services;

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

        // Behaviors
        public override void TryClose(bool? dialogResult = default(bool?))
        {
            CanClose(canClose =>
            {
                if (canClose) Close();
            });
        }

        private void Close()
        {
            var shell = Parent as IShell;
            shell?.Documents.Remove(this);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                TryClose();
            }
        }
    }
}