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
    public class CppProjectItemDefinition : ProjectItemDefinition
    {
        private readonly ICommandService _commandService;

        public CppProjectItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public override IEnumerable<CommandDefinition> CommandDefinitions
        {
            get
            {
                yield return new FakeCommandDefinition("|Add");
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
            return "C++ Project";
        }

        public override Uri GetIcon(bool isOpen, object tag)
        {
            return new Uri("pack://application:,,,/Idealde;component/Resources/Images/CppProject.png");
        }
    }
}