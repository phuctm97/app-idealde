namespace Idealde.Framework.Commands
{
    public interface ICommandRouter
    {
        CommandHandlerWrapper GetHandler(CommandDefinition commandDefinition);
    }
}