using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Services;

namespace Idealde.Modules.Output.Commands
{
    public class ViewOutputCommandHandler : ICommandHandler<ViewOutputCommandDefinition>
    {
        public void Update(Command command)
        {
        }

        public Task Run(Command command)
        {
            IoC.Get<IShell>().ShowTool<IOutput>();
            return Task.FromResult(true);
        }
    }
}