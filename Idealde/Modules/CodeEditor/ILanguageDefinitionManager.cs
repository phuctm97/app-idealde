#region Using Namespace

using System.Collections.Generic;
using ScintillaNET;

#endregion

namespace Idealde.Modules.CodeEditor
{
    public class LanguageDefinition
    {
        public LanguageDefinition(string name, Lexer lexer)
        {
            Name = name;
            Lexer = lexer;
        }

        public string Name { get; }

        public Lexer Lexer { get; }
    }

    public interface ILanguageDefinitionManager
    {
        Dictionary<string, LanguageDefinition> LanguageDefinitions { get; }

        void Register(LanguageDefinition language, params string[] extensions);

        LanguageDefinition GetLanguage(string extension);
    }
}