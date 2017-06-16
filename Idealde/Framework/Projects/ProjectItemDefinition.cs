#region Using Namespace

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Idealde.Framework.Commands;

#endregion

namespace Idealde.Framework.ProjectExplorer.Models
{
    public enum ProjectItemState
    {
        Opened,
        Closed
    }

    public abstract class ProjectItemDefinition
    {
        public abstract IEnumerable<CommandDefinition> CommandDefinitions { get; }

        public abstract ICommand ActiveCommand { get; }

        public abstract string Tooltip { get; }

        public abstract Uri GetIcon(ProjectItemState state);
    }
}