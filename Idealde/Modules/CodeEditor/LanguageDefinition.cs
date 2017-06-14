#region Using Namespace

using System.Collections.Generic;
using ScintillaNET;

#endregion

namespace Idealde.Modules.CodeEditor
{
    public class LanguageDefinition : ILanguageDefinition
    {
        private readonly Dictionary<string, Lexer> _fileExtensionToLanguageLookup;

        public LanguageDefinition()
        {
            _fileExtensionToLanguageLookup = new Dictionary<string, Lexer>();
            Register(Lexer.Cpp, ".c", ".cpp", ".h", ".hpp", ".cc");
        }

        public void Register(Lexer language, params string[] extensions)
        {
            foreach (var extension in extensions)
            {
                if (!_fileExtensionToLanguageLookup.ContainsKey(extension))
                {
                    _fileExtensionToLanguageLookup.Add(extension, language);
                }
            }
        }

        public Lexer GetLanguage(string extension)
        {
            Lexer lexer;
            if (!_fileExtensionToLanguageLookup.TryGetValue(extension, out lexer))
            {
                lexer = Lexer.Null;
            }

            return lexer;
        }
    }
}