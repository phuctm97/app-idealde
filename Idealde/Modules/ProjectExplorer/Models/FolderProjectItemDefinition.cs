using System;
using System.Collections.Generic;
using System.Windows.Input;
using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;

namespace Idealde.Modules.ProjectExplorer.Models
{
    public class FolderProjectItemDefinition: ProjectItemDefinition
    {
        public override IEnumerable<CommandDefinition> CommandDefinitions {get { yield break; } }
        public override ICommand ActiveCommand => null;

        public override string GetTooltip(object tag)
        {
            return "Folder";
        }
        public override Uri GetIcon(bool isOpen, object tag)
        {
            string iconSource = string.Empty;
            switch (isOpen)
            {
                case true:
                    iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/Run.png";
                    break;
                case false:
                    iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/Redo.png";
                    break;
            }
            return new Uri(iconSource, UriKind.Absolute);
        }

        public FolderProjectItemDefinition()
        {
        }

    }
}
