using Idealde.Framework.Panes;

namespace Idealde.Modules.CodeEditor
{
    public interface ICodeEditor: IPersistedDocument
    {
        void ChangeColor();
        string GetContent();
        void Goto(long row, long column);
    }
}
