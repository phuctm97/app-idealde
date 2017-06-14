#region Using Namespace

using System;
using System.Collections.Generic;
using ScintillaNET;

#endregion

namespace Idealde.Modules.CodeEditor
{
    public class LanguageDefinitionManager : ILanguageDefinitionManager
    {
        public Dictionary<string, LanguageDefinition> LanguageDefinitions { get; }

        public LanguageDefinitionManager()
        {
            LanguageDefinitions = new Dictionary<string, LanguageDefinition>();

            Initialize();
        }

        private void Initialize()
        {
            Register(new LanguageDefinition("Cpp", Lexer.Cpp), ".c", ".cpp", ".h", ".hpp", ".cc");
        }

        public void Register(LanguageDefinition language, params string[] extensions)
        {
            foreach (var extension in extensions)
            {
                if (!LanguageDefinitions.ContainsKey(extension))
                {
                    LanguageDefinitions.Add(extension, language);
                }
            }
        }

        public LanguageDefinition GetLanguage(string extension)
        {
            LanguageDefinition languageDefinition;
            if (!LanguageDefinitions.TryGetValue(extension, out languageDefinition))
            {
                languageDefinition = new LanguageDefinition("Null", Lexer.Null);
            }

            return languageDefinition;
        }

    }
}