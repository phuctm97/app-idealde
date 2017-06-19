#region Using Namespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Services;
using Idealde.Modules.CodeEditor;
using Idealde.Modules.ProjectExplorer.Commands;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.ProjectExplorer.Models
{
    public class FileProjectItemDefinition : ProjectItemDefinition
    {
        private readonly ICommandService _commandService;

        public override IEnumerable<CommandDefinition> CommandDefinitions
        {
            get { yield return _commandService.GetCommandDefinition(typeof(RemoveFileCommandDefinition)); }
        }

        public override ICommand ActiveCommand { get; }

        public override string GetTooltip(object tag)
        {
            var path = tag as string ?? string.Empty;
            if (!File.Exists(path))
            {
                return Resources.FileNotExistText;
            }

            var extension = Path.GetExtension(path).ToLower();
            return extension + " file";
        }

        public override Uri GetIcon(bool isOpen, object tag)
        {
            var path = tag as string;

            // default file icon

            string iconSource;
            if (!File.Exists(path))
            {
                iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/BrokenlinktoFile.png";
            }
            else
            {
                var extension = path == null ? string.Empty : Path.GetExtension(path).ToLower();
                switch (extension)
                {
                    case ".cpp":
                    case ".c":
                    case ".cxx":
                    case ".h":
                    case ".hpp":
                        iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/CppFile.png";
                        break;
                    default:
                        iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/File.png";
                        break;
                }
            }

            return new Uri(iconSource, UriKind.Absolute);
        }

        public FileProjectItemDefinition(ICommandService commandService)
        {
            _commandService = commandService;
            ActiveCommand = new RelayCommand(Active, CanActive);
        }

        private bool CanActive(object projectItem)
        {
            var filePath = ((ProjectItemBase)projectItem)?.Tag as string;
            if (!File.Exists(filePath))
            {
                return false;
            }
            return true;
        }

        private void Active(object projectItem)
        {
            var shell = IoC.Get<IShell>();

            var filePath = ((ProjectItemBase) projectItem)?.Tag as string;
            if (string.IsNullOrEmpty(filePath)) return;

            if (!File.Exists(filePath))
            {
                MessageBox.Show(Resources.FileNotExistText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var document in shell.Documents)
            {
                var persistedDocument = document as IPersistedDocument;
                if (persistedDocument == null) continue;

                var editorFilePath = filePath.Trim().TrimEnd('\\').ToLower();
                var persistedDocumentFilePath = persistedDocument.FilePath.Trim().TrimEnd('\\').ToLower();

                if (string.CompareOrdinal(editorFilePath, persistedDocumentFilePath) != 0) continue;
                shell.OpenDocument(persistedDocument);
                return;
            }

            var editor = IoC.Get<ICodeEditor>();
            editor.Load(filePath);
            shell.OpenDocument(editor);
        }
    }
}