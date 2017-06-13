using System.Threading.Tasks;
using Idealde.Framework.Commands;
using Idealde.Framework.Services;

namespace Idealde.Modules.Shell.Commands
{
    public class ExitCommandHandler : ICommandHandler<ExitCommandDefinition>
    {
        private readonly IShell _shell;

        public ExitCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public void Update(Command command)
        {}

        public Task Run(Command command)
        {
            _shell.Close();
            return Task.FromResult(true);
        }
    }
}