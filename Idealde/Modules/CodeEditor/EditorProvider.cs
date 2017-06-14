#region Using Namespace

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;

#endregion

namespace Idealde.Modules.CodeEditor
{
    public class EditorProvider : IEditorProvider
    {
        private readonly ILanguageDefinitionManager _languageDefinitionManager;

        public EditorProvider(ILanguageDefinitionManager languageDefinitionManager)
        {
            _languageDefinitionManager = languageDefinitionManager;
        }

        public const string ProviderName = "Code Files";

        public string Name => ProviderName;

        public IEnumerable<EditorFileType> FileTypes
        {
            get
            {
                return _languageDefinitionManager.LanguageDefinitions.Select(
                    p => new EditorFileType(p.Value.Name, p.Key));
            }
        }

        public bool CanRead(string path)
        {
            var extension = Path.GetExtension(path);
            return extension != null && _languageDefinitionManager.LanguageDefinitions.ContainsKey(extension);
        }

        public IDocument Create()
        {
            return IoC.Get<ICodeEditor>();
        }

        public async Task New(IDocument document, string fileName)
        {
            var editor = document as ICodeEditor;
            if (editor == null) return;

            await editor.New(fileName);
        }

        public async Task Open(IDocument document, string filePath)
        {
            var editor = document as ICodeEditor;
            if (editor == null) return;

            await editor.Load(filePath);
        }
    }
}