using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idealde.Framework.Commands
{
    public class CommandService : ICommandService
    {
        public CommandDefinition GetCommandDefinition(Type commandDefinitionType)
        {
            throw new NotImplementedException();
        }

        public Command GetCommand(CommandDefinition commandDefinition)
        {
            throw new NotImplementedException();
        }
    }
}
