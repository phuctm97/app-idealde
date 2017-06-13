namespace Idealde.Framework.Commands
{
    public interface ICommandRouter
    {
        ICommandHandler GetHandler(CommandDefinition commandDefinition);
    }
}