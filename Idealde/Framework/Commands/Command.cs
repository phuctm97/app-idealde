#region Using Namespace

using System;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Commands
{
    public class Command : PropertyChangedBase
    {
        // Backing fields

        #region Backing fields

        private bool _isVisible;
        private bool _isEnabled;
        private bool _isChecked;
        private string _text;
        private string _tooltip;
        private Uri _iconSource;
        private object _tag;

        #endregion

        // Initializations

        #region Initializations

        public Command(CommandDefinition commandDefinition)
        {
            CommandDefinition = commandDefinition;
            Text = CommandDefinition.Text;
            Tooltip = CommandDefinition.Tooltip;
            IconSource = CommandDefinition.IconSource;
            IsEnabled = true;
            IsVisible = true;
        }

        #endregion

        // Bind properites

        #region Bind properties

        public CommandDefinition CommandDefinition { get; }

        // Command state visible
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

        // Command state enable
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

        // Command state checked
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

        // Command display name
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

        // Command display description
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

        // Command display icon
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

        // Command helper object
        public object Tag
        {
            get { return _tag; }
            set
            {
                if (Equals(value, _tag)) return;
                _tag = value;
                NotifyOfPropertyChange(() => Tag);
            }
        }

        #endregion
    }
}