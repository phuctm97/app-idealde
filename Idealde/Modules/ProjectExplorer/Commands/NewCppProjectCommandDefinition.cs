using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.ProjectExplorers.Shell.Commands
{
    public class NewCppProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.New.CppProject";

        public override string Name => CommandName;

        public override string Tooltip => Resources.FileNewCppProjectCommandTooltip;

        public override string Text => Resources.FileNewCppProjectCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/NewProject.bmp");
    }
}