#region Using Namespace

using System.IO;
using System.Windows.Input;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Modules.ProjectExplorer.Models;

#endregion

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class ProjectExplorerViewModel : Tool, IProjectExplorer
    {
        public override PaneLocation PreferredLocation => PaneLocation.Right;


        public ProjectExplorerViewModel()
        {
            DisplayName = "Solution Explorer";
            Items = new BindableCollection<ProjectItemBase>();
        }


        public IObservableCollection<ProjectItemBase> Items { get; }

    }
}