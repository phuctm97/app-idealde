using System.Threading.Tasks;

namespace Idealde.Framework.Commands
{
    public interface ICommandHandler
    {
        void Update(Command command);

        Task Run(Command command);
    }

    public interface ICommandHandler<TCommandDefinition> : ICommandHandler
        where TCommandDefinition : CommandDefinition
    {
        
    }
}