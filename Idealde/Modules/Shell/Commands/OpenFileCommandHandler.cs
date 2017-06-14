#region Using Namespace

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;
using Idealde.Properties;
using Microsoft.Win32;

#endregion

namespace Idealde.Modules.Shell.Commands
{
    public class OpenFileCommandHandler : ICommandHandler<OpenFileCommandDefinition>
    {
        private readonly IEnumerable<IEditorProvider> _editorProviders;

        public OpenFileCommandHandler()
        {
            _editorProviders = IoC.GetAll<IEditorProvider>();
        }

        public void Update(Command command)
        {
        }

        public async Task Run(Command command)
        {
            var dialog = new OpenFileDialog
            {
                Title = Resources.OpenFileDialogTitle,
                Filter = "All Supported Files|" + string.Join(";", _editorProviders
                             .SelectMany(x => x.FileTypes).Select(x => "*" + x.Extension))
            };

            foreach (var editorProvider in _editorProviders)
            {
                dialog.Filter += "|" + editorProvider.Name + "|" +
                                 string.Join(";", editorProvider.FileTypes.Select(x => "*" + x.Extension));
            }

            if (dialog.ShowDialog() == true)
            {
                IoC.Get<IShell>().OpenDocument(await CreateEditor(dialog.FileName));
            }
        }

        private async Task<IDocument> CreateEditor(string filePath)
        {
            var provider = _editorProviders.FirstOrDefault(p => p.CanRead(filePath));
            if (provider == null)
                return null;

            var editor = provider.Create();
            await provider.Open(editor, filePath);

            return editor;
        }
    }
}