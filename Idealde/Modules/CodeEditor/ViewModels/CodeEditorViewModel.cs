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

        public CodeEditorViewModel()
        {
     
        }

        protected override void OnViewLoaded(object view)
        {
            _view = (ICodeEditorView)view;
            if (_view == null) throw new InvalidCastException();

            _view.SetResourceDirectory("Resources");
            _view.SetLexer(Lexer.Cpp);
            base.OnViewLoaded(view);
        }

        protected override Task DoNew()
        {
            _view.SetContent(string.Empty);
            return Task.FromResult(true);
        }

        protected override Task DoLoad()
        {
            string fileContent = File.ReadAllText(FilePath);
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
            _view.SetLexer(lexer);
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
