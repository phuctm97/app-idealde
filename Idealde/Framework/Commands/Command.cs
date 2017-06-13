#region Using Namespace

using System;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Commands
{
    public class Command : PropertyChangedBase
    {
        private bool _isVisible;
        private bool _isEnabled;
        private bool _isChecked;
        private string _text;
        private string _tooltip;
        private Uri _iconSource;

        public Command(CommandDefinition commandDefinition)
        {
            CommandDefinition = commandDefinition;
            Text = commandDefinition.Text;
            Tooltip = commandDefinition.ToolTip;
            IconSource = commandDefinition.IconSource;
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value == _isEnabled) return;
                _isEnabled = value;
                NotifyOfPropertyChange(() => IsEnabled);
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value == _isChecked) return;
                _isChecked = value;
                NotifyOfPropertyChange(() => IsChecked);
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public string Tooltip
        {
            get { return _tooltip; }
            set
            {
                if (value == _tooltip) return;
                _tooltip = value;
                NotifyOfPropertyChange(() => Tooltip);
            }
        }

        public Uri IconSource
        {
            get { return _iconSource; }
            set
            {
                if (Equals(value, _iconSource)) return;
                _iconSource = value;
                NotifyOfPropertyChange(() => IconSource);
            }
        }

        public CommandDefinition CommandDefinition { get; }
    }
}