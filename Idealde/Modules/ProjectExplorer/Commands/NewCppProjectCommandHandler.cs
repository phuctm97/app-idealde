#region Using Namespace

using System.IO;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Projects;
using Idealde.Modules.ProjectExplorer.ViewModels;

#endregion

namespace Idealde.ProjectExplorers.Shell.Commands
{
    public class NewCppProjectCommandHandler
        : ICommandHandler<NewCppProjectCommandDefinition>
    {
        public void Update(Command command)
        {
        }

        public Task Run(Command command)
        {
            var windowManager = IoC.Get<IWindowManager>();

            // show new dialog
            var dialog = IoC.Get<NewProjectSettingsViewModel>();
            var result = windowManager.ShowDialog(dialog) ?? false;
            if (!result) return Task.FromResult(false);

            var rootDirectory = dialog.ProjectRootDirectory.Trim();
            var projectName = dialog.ProjectName.Trim();

            // create project directory
            var projectDirectory = Path.Combine(rootDirectory, projectName);
            if (!Directory.Exists(projectDirectory))
            {
                Directory.CreateDirectory(projectDirectory);
            }

            // create project output directory
            foreach (var childDirectory in new[] {"Output", "Output\\obj", "Output\\bin"})
            {
                var d = $"{projectDirectory}\\{childDirectory}";
                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                }
            }

            // create project info file
            var emptyProjectInfo = new ProjectInfo();
            var projectManager = IoC.Get<IProjectManager>();

            var projectInfoFilePath = $"{projectDirectory}\\{projectName}";
            if (!projectInfoFilePath.EndsWith(".vcproj", System.StringComparison.OrdinalIgnoreCase))
            {
                projectInfoFilePath += ".vcproj";
            }
            projectManager.Save(emptyProjectInfo, projectInfoFilePath);

            return Task.FromResult(true);
        }
    }
}