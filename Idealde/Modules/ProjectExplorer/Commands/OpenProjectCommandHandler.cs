#region Using Namespace

using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.ProjectExplorer.Models;
using Idealde.Framework.Services;
using Idealde.Properties;
using Microsoft.Win32;

#endregion

namespace Idealde.Modules.ProjectExplorer.Commands
{
    public class OpenProjectCommandHandler :
        ICommandHandler<OpenProjectCommandDefinition>
    {
        public void Update(Command command)
        {
        }

        public Task Run(Command command)
        {
            var projectManager = IoC.Get<IProjectManager>();

            var dialog = new OpenFileDialog
            {
                Title = Resources.OpenFileDialogTitle,
                Filter = "All Supported Files|" + string.Join(";", projectManager.ProjectTypes
                             .Select(t => $"*{t.Extension}"))
            };

            foreach (var projectType in projectManager.ProjectTypes)
            {
                dialog.Filter += "|" + projectType.Name + "|" + projectType.Extension;
            }

            if (dialog.ShowDialog() == true)
            {
                var projectExplorer = IoC.Get<IProjectExplorer>();
                projectExplorer.LoadProject(dialog.FileName);

                var shell = IoC.Get<IShell>();
                shell.ShowTool(projectExplorer);
            }

            return Task.FromResult(true);
        }
    }
}