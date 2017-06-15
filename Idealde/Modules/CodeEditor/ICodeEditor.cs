using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Modules.CodeCompiler.Commands;
using ScintillaNET;

namespace Idealde.Modules.CodeEditor
{
    public interface ICodeEditor: IPersistedDocument,
        ICommandHandler<CompileSingleFileCommandDefinition>,
        ICommandHandler<RunSingleFileCommandDefinition>,
        ICommandHandler<CompileAndRunSingleFileCommandDefinition>
    {
        void SetLanguage(Lexer lexer);
        void Goto(int row, int column);
        string GetContent();
    }
}
