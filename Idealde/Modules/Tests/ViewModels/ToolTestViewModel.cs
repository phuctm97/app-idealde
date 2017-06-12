using Idealde.Framework.Panes;

namespace Idealde.Modules.Tests.ViewModels
{
    public class ToolTestViewModel : Tool
    {
        public override PaneLocation PreferredLocation { get; }

        public ToolTestViewModel(PaneLocation preferredLocation)
        {
            PreferredLocation = preferredLocation;
        }
    }
}