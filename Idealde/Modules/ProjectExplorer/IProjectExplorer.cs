using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;
using Idealde.Modules.MainMenu.Models;
using Idealde.Modules.ProjectExplorer.Models;

namespace Idealde.Modules.ProjectExplorer
{
    public interface IProjectExplorer : ITool
    {
        ProjectInfoBase CurrentProjectInfo { get; set; }

        IObservableCollection<ProjectItemBase> ProjectItems { get; }

        void LoadProject(string path, IProjectProvider provider);

        void CloseCurrentProject();
    }
}