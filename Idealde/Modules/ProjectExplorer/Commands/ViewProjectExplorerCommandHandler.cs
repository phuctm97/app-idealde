using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Services;

namespace Idealde.Modules.ProjectExplorer.Commands
{
    public class ViewProjectExplorerCommandHandler :
        ICommandHandler<ViewProjectExplorerCommandDefinition>
    {
        public void Update(Command command)
        {
        }

        public Task Run(Command command)
        {
            var shell = IoC.Get<IShell>();
            shell.ShowTool<IProjectExplorer>();

            return Task.FromResult(true);
        }
    }
}