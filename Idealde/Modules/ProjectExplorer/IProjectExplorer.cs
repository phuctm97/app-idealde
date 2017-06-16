using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Modules.ProjectExplorer.Models;

namespace Idealde.Modules.ProjectExplorer
{
    public interface IProjectExplorer: ITool
    {
        IObservableCollection<ProjectItemBase> Items { get; }
    }
}