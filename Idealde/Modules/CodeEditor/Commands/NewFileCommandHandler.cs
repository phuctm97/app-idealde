#region Using Namespace

using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Services;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.CodeEditor.Commands
{
    public abstract class NewFileCommandHandler : ICommandHandler
    {
        private int _fileCounter;
        private readonly string _fileExtension;
        private readonly ILanguageDefinition _languageDefinition;

        protected NewFileCommandHandler(string fileExtension, ILanguageDefinition languageDefinition)
        {
            _fileExtension = fileExtension;
            _languageDefinition = languageDefinition;
            _fileCounter = 1;
        }

        public void Update(Command command)
        {
        }

        public async Task Run(Command command)
        {
            var editor = IoC.Get<ICodeEditor>();
            var shell = IoC.Get<IShell>();

            shell.OpenDocument(editor);
            await editor.New($"{Resources.UntitledFileName}{_fileCounter++}{_fileExtension}");
            editor.SetLanguage(_languageDefinition.GetLanguage(_fileExtension));
        }
    }

    public class NewCppHeaderCommandHandler : NewFileCommandHandler,
        ICommandHandler<NewCppHeaderCommandDefinition>
    {
        public NewCppHeaderCommandHandler(ILanguageDefinition languageDefinition)
            : base(".h", languageDefinition)
        {
        }
    }


    public class NewCppSourceCommandHandler : NewFileCommandHandler,
        ICommandHandler<NewCppSourceCommandDefinition>
    {
        public NewCppSourceCommandHandler(ILanguageDefinition languageDefinition)
            : base(".cpp", languageDefinition)
        {
        }
    }
}