using System;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.ProjectExplorer.Commands
{
    public class AddFolderToProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Add.Folder";

        public override string Name => CommandName;

        public override string Tooltip => Resources.FileNewFolderCommandTooltip;

        public override string Text => Resources.FileNewFolderCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/FolderClosed.png");
    }

    public class AddNewCppHeaderToProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Add.CppHeader";

        public override string Name => CommandName;

        public override string Tooltip => Resources.AddNewCppHeaderToProjectCommandTooltip;

        public override string Text => Resources.AddNewCppHeaderToProjectCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/CppFile.png");
    }

    public class AddNewCppSourceToProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Add.CppSource";

        public override string Name => CommandName;

        public override string Tooltip => Resources.AddNewCppSourceToProjectCommandTooltip;

        public override string Text => Resources.AddNewCppSourceToProjectCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/CppFile.png");
    }

    public class AddExistingFileToProjectCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Project.Add.ExistingFile";

        public override string Name => CommandName;

        public override string Tooltip => Resources.AddExistingFileCommandTooltip;

        public override string Text => Resources.AddExistingFileCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/File.png");
    }
}