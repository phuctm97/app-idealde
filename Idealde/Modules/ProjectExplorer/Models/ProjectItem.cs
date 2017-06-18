#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;

#endregion

namespace Idealde.Modules.ProjectExplorer.Models
{
    public class ProjectItem : ProjectItemBase
    {
        private readonly ICommandService _commandService;

        public ProjectItemDefinition ProjectItemDefintion { get; }

        public override object Tag { get; set; }

        public override Uri IconSource => ProjectItemDefintion.GetIcon(IsOpen,Tag);

        public override string Tooltip => ProjectItemDefintion.GetTooltip(Tag);

        public override ICommand ActiveCommand => ProjectItemDefintion.ActiveCommand;

        public override IEnumerable<Command> OptionCommands
        {
            get
            {
                foreach (var commandDefinition in ProjectItemDefintion.CommandDefinitions)
                {
                    yield return _commandService.GetCommand(commandDefinition);
                }
            }
        }

        public ProjectItem(ProjectItemDefinition projectItemDefintion)
        {
            _commandService = IoC.Get<ICommandService>();

            ProjectItemDefintion = projectItemDefintion;
        }

        public ProjectItem(Type projectItemDefinitionType)
        {
            _commandService = IoC.Get<ICommandService>();

            var projectController = IoC.Get<IProjectManager>();
            ProjectItemDefintion = projectController.GetProjectItemDefinition(projectItemDefinitionType);
        }

    }

    public class ProjectItem<TDefinition> : ProjectItem
        where TDefinition : ProjectItemDefinition
    {
        public ProjectItem() : base(typeof(TDefinition))
        {
        }
    }
}