#region Using Namespace

using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Projects;
using Idealde.Framework.Services;
using Idealde.Modules.ProjectExplorer;
using Idealde.Modules.ProjectExplorer.Providers;
using Idealde.Modules.ProjectExplorer.ViewModels;
using Idealde.Properties;

#endregion

namespace Idealde.ProjectExplorers.Shell.Commands
{
    public class NewCppProjectCommandHandler
        : ICommandHandler<NewCppProjectCommandDefinition>
    {
        public void Update(Command command)
        {
        }

        public async Task Run(Command command)
        {
            var windowManager = IoC.Get<IWindowManager>();

            // show new dialog
            var dialog = IoC.Get<NewProjectSettingsViewModel>();
            var result = windowManager.ShowDialog(dialog) ?? false;
            if (!result) return;

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
            var emptyProjectInfo = new CppProjectInfo(provider) {ProjectName = projectName};

            projectInfoFilePath = await provider.Save(emptyProjectInfo, projectInfoFilePath);
            if (string.IsNullOrEmpty(projectInfoFilePath))
            {
                MessageBox.Show(Resources.CreateProjectFailedText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // load new project
            var projectExplorer = IoC.Get<IProjectExplorer>();
            projectExplorer.LoadProject(projectInfoFilePath, provider);

            var shell = IoC.Get<IShell>();
            shell.ShowTool(projectExplorer);
        }
    }
}