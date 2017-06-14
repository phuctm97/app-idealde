using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Services;

namespace Idealde.Modules.ErrorList.Commands
{
    public class ViewErrorListCommandHandler : ICommandHandler<ViewErrorListCommandDefinition>
    {
        public ViewErrorListCommandHandler()
        {
        }

        public void Update(Command command)
        {
        }

        public Task Run(Command command)
        {
            IoC.Get<IShell>().ShowTool<IErrorList>();
            return Task.FromResult(true);
        }
    }
}