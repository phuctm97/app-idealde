using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Services;
using Idealde.Modules.CodeEditor;

namespace Idealde.Modules.ProjectExplorer.Models
{
    public class FileProjectItemDefinition: ProjectItemDefinition
    {
        public override IEnumerable<CommandDefinition> CommandDefinitions { get { yield break; } }
        public override ICommand ActiveCommand { get; }

        public override string GetTooltip(object tag)
        {
            string path = tag as string;
            if (path == null) return string.Empty;

            var extension = Path.GetExtension(path).ToLower();

            return extension + " file";
        }
        public override Uri GetIcon(bool isOpen, object tag)
        {
            string path = tag as string;

            // default file icon
            string iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/File.png";

            if (path == null) return new Uri(iconSource, UriKind.Absolute);

            var extension = Path.GetExtension(path).ToLower();

            switch (extension)
            {
                case "cpp":
                    iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/CppFile.png";
                    break;
                case "cs":
                    iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/CsFile.png";
                    break;
                case "config":
                    iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/ConfigFile.png";
                    break;
                case "xml":
                    iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/XmlFile.png";
                    break;
                case "txt":
                    iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/TxtFile.png";
                    break;
                case "c":
                    iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/CFile.png";
                    break;
                case "h":
                    iconSource = "pack://application:,,,/Idealde;Component/Resources/Images/HeaderFile.png";
                    break;
            }

            return new Uri(iconSource, UriKind.Absolute);
        }

        public FileProjectItemDefinition()
        {
            ActiveCommand = new RelayCommand(Active, CanActive);
        }

        private bool CanActive(object projectItem)
        {
            return true;
        }

        private void Active(object projectItem)
        {
            var editor = IoC.Get<ICodeEditor>();
            var shell = IoC.Get<IShell>();

            var filePath = ((ProjectItemBase)projectItem).Tag as string;
            if (!File.Exists(filePath))
            { 
                MessageBox.Show("File not exists in current project");
                return;
            }

            editor.Load(filePath);

            foreach (var document in shell.Documents)
            {
                var persistedDocument = document as IPersistedDocument;
                if (persistedDocument == null) continue;

                var editorFilePath = editor.FilePath.TrimEnd('\\').ToLower();
                var persistedDocumentFilePath = persistedDocument.FilePath.TrimEnd('\\').ToLower();

                if (String.CompareOrdinal(editorFilePath, persistedDocumentFilePath) != 0) continue;
                shell.OpenDocument(persistedDocument);
                return;
            }

            shell.OpenDocument(editor);
        }

    }
}
