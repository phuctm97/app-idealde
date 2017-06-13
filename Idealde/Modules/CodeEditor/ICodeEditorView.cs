using System;
using ScintillaNET;

namespace Idealde.Modules.CodeEditor
{
    public interface ICodeEditorView
    {
        // CodeEditor view behaviors
        void SetResourceDirectory(string directory);
        void SetLexer(Lexer lexer);
        void SetContent(string text);
        void Goto(int row, int column);
        string GetContent();
    }
}
