using Idealde.Framework.Panes;
using ScintillaNET;

namespace Idealde.Modules.CodeEditor
{
    public interface ICodeEditor: IPersistedDocument
    {
        void SetLanguage(Lexer lexer);
        void Goto(int row, int column);
        string GetContent();
    }
}
