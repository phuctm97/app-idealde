using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.Projects;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Models;

namespace Idealde.Modules.ProjectExplorer
{
    public interface IProjectExplorer : ITool
    {
        ProjectInfo CurrentProjectInfo { get; set; }

        IObservableCollection<ProjectItemBase> ProjectItems { get; }

        void LoadProject(string path);

        void CloseCurrentProject();
    }
}