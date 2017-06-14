using System.Threading.Tasks;

namespace Idealde.Framework.Commands
{
    public interface ICommandHandler
    {
       
    }

    public interface ICommandHandler<TCommandDefinition> : ICommandHandler
        where TCommandDefinition : CommandDefinition
    {
        void Update(Command command);

        Task Run(Command command);
    }
}