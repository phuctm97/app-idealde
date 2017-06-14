namespace Idealde.Modules.CodeEditor
{
    public interface ILanguageDefinition
    {
        void Register(ScintillaNET.Lexer language, params string[] extensions);

        ScintillaNET.Lexer GetLanguage(string extension);
    }
}