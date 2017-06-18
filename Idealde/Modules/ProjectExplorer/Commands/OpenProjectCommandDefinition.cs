using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.ProjectExplorer.Commands
{
    public class OpenProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.OpenProject";

        public override string Name => CommandName;

        public override string Tooltip => Resources.FileOpenProjectCommandTooltip;

        public override string Text => Resources.FileOpenProjectCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/OpenProject.bmp");
    }
}