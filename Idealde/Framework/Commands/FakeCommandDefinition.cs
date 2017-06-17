using Idealde.Framework.Commands;

namespace Idealde.Framework.Commands
{
    public class FakeCommandDefinition : CommandDefinition
    {
        public FakeCommandDefinition(string name)
        {
            Name = name;
        }

        public override string Name { get; }
    }
}