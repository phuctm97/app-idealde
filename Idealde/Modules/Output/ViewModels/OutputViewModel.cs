#region Using Namespace

using System;
using System.Text;
using Caliburn.Micro;
using Idealde.Framework.Panes;

#endregion

namespace Idealde.Modules.Output.ViewModels
{
    public sealed class OutputViewModel : Tool, IOutput
    {
        // Backing fields
        #region Backing fields
        private readonly StringBuilder _stringBuilder;
        private IOutputView _view;
        #endregion

        // Bind properties
        #region Bind properties

        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

        #endregion

        // Initializations 
        #region Initializations
        public OutputViewModel()
        {
            _stringBuilder = new StringBuilder();

            DisplayName = "Output";
        }

        protected override void OnViewLoaded(object view)
        {
            _view = (IOutputView)view;
            if (_view == null) throw new InvalidCastException();

            _view.SetText(_stringBuilder.ToString());
            _view.ScrollToEnd();

            base.OnViewLoaded(view);
        }

        #endregion

        // Output behaviors
        #region Output behaviors
        public void Append(string text)
        {
            _stringBuilder.Append(text);
            OnTextChanged();
        }

        public void AppendLine(string text)
        {
            Append(text);
            BreakLine();
        }

        public void BreakLine()
        {
            Append(Environment.NewLine);
        }

        public void Clear()
        {
            if (_view != null)
            {
                Execute.OnUIThread(() => _view.Clear());
            }

            _stringBuilder.Clear();
        }

        private void OnTextChanged()
        {
            if (_view != null)
            {
                Execute.OnUIThread(() => _view.SetText(_stringBuilder.ToString()));
            }
        } 
        #endregion
    }
}