﻿#region Using Namespace

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;

#endregion

namespace Idealde.Modules.ProjectExplorer.Models
{
    public abstract class ProjectItemBase : PropertyChangedBase
    {
        // Backing fields

        #region Backing fields

        private string _text;
        private ProjectItemState _state;

        #endregion

        // Bind properties

        #region Bind properties

        public abstract Uri IconSource { get; }

        public abstract string Tooltip { get; }

        public abstract ICommand ActiveCommand { get; }

        public abstract IEnumerable<Command> OptionCommands { get; }

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

        public ProjectItemState State
        {
            get { return _state; }
            set
            {
                if (value == _state) return;
                _state = value;
                NotifyOfPropertyChange(() => State);
            }
        }

        #endregion

        // Bind models

        #region Bind models

        public IObservableCollection<ProjectItemBase> Children { get; }

        #endregion

        protected ProjectItemBase()
        {
            Children = new BindableCollection<ProjectItemBase>();
        }
    }
}