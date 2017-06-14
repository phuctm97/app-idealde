using System;
using System.IO;
using System.Threading.Tasks;
using Idealde.Framework.Panes;
using ScintillaNET;

namespace Idealde.Modules.CodeEditor.ViewModels
{
    public class CodeEditorViewModel : PersistedDocument, ICodeEditor
    {
        private ICodeEditorView _view;
        private string _fileContent;
        private Lexer _fileLexer;

        private readonly ILanguageDefinitionManager _languageDefinitionManager;

        public CodeEditorViewModel(ILanguageDefinitionManager languageDefinitionManager)
        {
            _languageDefinitionManager = languageDefinitionManager;
            _fileContent = string.Empty;
        }

        protected override void OnViewLoaded(object view)
        {
            _view = (ICodeEditorView)view;
            if (_view == null) throw new InvalidCastException();

            _view.SetResourceDirectory("Resources");
            _view.SetContent(_fileContent);
            _view.SetLexer(_fileLexer);
            base.OnViewLoaded(view);
        }

        protected override Task DoNew()
        {
            _fileContent = string.Empty;
            _view?.SetContent(_fileContent);

            SetLanguage(_languageDefinitionManager.GetLanguage(Path.GetExtension(FileName)).Lexer);
            return Task.FromResult(true);
        }

        protected override Task DoLoad()
        {
            _fileContent = File.ReadAllText(FilePath);
            _view?.SetContent(_fileContent);

            SetLanguage(_languageDefinitionManager.GetLanguage(Path.GetExtension(FilePath)).Lexer);
            return Task.FromResult(true);
        }

        protected override Task DoSave()
        {
            string viewContent = _view.GetContent();
            File.WriteAllText(viewContent, FilePath);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Set language of document
        /// </summary>
        /// <param name="lexer">language (Cpp, c#, vb, ... )</param>
        public void SetLanguage(Lexer lexer)
        {
            _fileLexer = lexer;
            _view?.SetLexer(_fileLexer);
        }

        public string GetContent()
        {
            return _view.GetContent();
        }

        public void Goto(int row, int column)
        {
            _view.Goto(row, column);
        }
    }
}
