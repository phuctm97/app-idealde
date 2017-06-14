#region Using Namespace

using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Services;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.CodeEditor.Commands
{
    public abstract class NewCodeFileCommandHandler : ICommandHandler
    {
        private int _fileCounter;
        private readonly string _fileExtension;

        protected NewCodeFileCommandHandler(string fileExtension)
        {
            _fileExtension = fileExtension;
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
        }
    }

    public class NewCppHeaderCommandHandler : NewCodeFileCommandHandler,
        ICommandHandler<NewCppHeaderCommandDefinition>
    {
        public NewCppHeaderCommandHandler()
            : base(".h")
        {
        }
    }


    public class NewCppSourceCommandHandler : NewCodeFileCommandHandler,
        ICommandHandler<NewCppSourceCommandDefinition>
    {
        public NewCppSourceCommandHandler()
            : base(".cpp")
        {
        }
    }
}