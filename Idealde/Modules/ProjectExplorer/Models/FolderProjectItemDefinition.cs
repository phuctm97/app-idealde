#region Using Namespace

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Modules.ProjectExplorer.Commands;

#endregion

namespace Idealde.Modules.ProjectExplorer.Models
{
    public class FolderProjectItemDefinition : ProjectItemDefinition
    {
        // Dependencies
        private readonly ICommandService _commandService;


        public override IEnumerable<CommandDefinition> CommandDefinitions
        {
            get
            {
                yield return new FakeCommandDefinition("|Add");
                yield return new FakeCommandDefinition("");
                yield return new FakeCommandDefinition("|");
                yield return _commandService.GetCommandDefinition(typeof(RemoveFileCommandDefinition));
                yield return new FakeCommandDefinition("");
                yield return new FakeCommandDefinition("|");
                yield return new FakeCommandDefinition("|Properties");
                yield return new FakeCommandDefinition("|Add");
                yield return _commandService.GetCommandDefinition(typeof(AddFolderToProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return _commandService.GetCommandDefinition(typeof(AddNewCppHeaderToProjectCommandDefinition));
                yield return _commandService.GetCommandDefinition(typeof(AddNewCppSourceToProjectCommandDefinition));
                yield return new FakeCommandDefinition("|");
                yield return _commandService.GetCommandDefinition(typeof(AddExistingFileToProjectCommandDefinition));
            }
        }

        public override ICommand ActiveCommand => null;

        public override string GetTooltip(object tag)
        {
            return string.Empty;
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