using System;
using ScintillaNET;

namespace Idealde.Modules.CodeEditor
{
    public delegate void DirtyChangingEventHandler(bool q);
    public interface ICodeEditorView
    {
        event DirtyChangingEventHandler IsDirtyChanged;

        // CodeEditor view behaviors
        void SetResourceDirectory(string directory);
        void SetLexer(Lexer lexer);
        void SetContent(string text);
        void Goto(int row, int column);
        void EditorFocus();
        string GetContent();

        void Undo();
        void Redo();
    }
}
