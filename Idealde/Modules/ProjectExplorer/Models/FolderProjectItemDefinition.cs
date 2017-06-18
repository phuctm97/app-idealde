#region Using Namespace

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;

#endregion

namespace Idealde.Modules.ProjectExplorer.Models
{
    public class FolderProjectItemDefinition : ProjectItemDefinition
    {
        // Dependencies
        private readonly ICommandService _commandService;


        public override IEnumerable<CommandDefinition> CommandDefinitions
        {
            get { yield break; }
        }

        public override ICommand ActiveCommand => null;

        public override string GetTooltip(object tag)
        {
            var name = tag?.ToString() ?? string.Empty;
            return $"Folder {name}";
        }

        public override Uri GetIcon(bool isOpen, object tag)
        {
            var iconSource = isOpen
                ? "pack://application:,,,/Idealde;Component/Resources/Images/FolderOpen.png"
                : "pack://application:,,,/Idealde;Component/Resources/Images/FolderClosed.png";
            return new Uri(iconSource, UriKind.Absolute);
        }

        public FolderProjectItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
        }
    }
}