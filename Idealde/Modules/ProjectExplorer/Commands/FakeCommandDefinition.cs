using Idealde.Framework.Commands;

namespace Idealde.Modules.ProjectExplorer.Commands
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