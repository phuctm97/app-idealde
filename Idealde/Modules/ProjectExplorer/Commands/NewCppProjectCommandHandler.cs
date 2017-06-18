#region Using Namespace

using System.IO;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Projects;
using Idealde.Modules.ProjectExplorer.Providers;
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

            // create project directory
            var rootDirectory = dialog.ProjectRootDirectory.Trim();
            var projectName = dialog.ProjectName.Trim();
            var projectDirectory = Path.Combine(rootDirectory, projectName);
            if (!Directory.Exists(projectDirectory))
            {
                Directory.CreateDirectory(projectDirectory);
            }

            var projectInfoFilePath = $"{projectDirectory}\\{projectName}";

            // create empty project info file
            var provider = IoC.Get<CppProjectProvider>();
            var emptyProjectInfo = new CppProjectInfo(provider);
            emptyProjectInfo.ProjectName = projectName;

            provider.Save(emptyProjectInfo, projectInfoFilePath);

            return Task.FromResult(true);
        }
    }
}