#region Using Namespace

using System.IO;
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
            var providers = IoC.GetAll<IProjectProvider>().ToList();

            var dialog = new OpenFileDialog
            {
                Title = Resources.OpenFileDialogTitle,
                Multiselect = false,
                Filter = "All Supported Files|" + string.Join(";", providers.SelectMany(p => p.ProjectTypes)
                             .Select(t => $"*{t.Extension}"))
            };

            // generate filters
            foreach (var provider in providers)
            {
                dialog.Filter += "|" + provider.Name + "|" +
                                 string.Join(";", provider.ProjectTypes.Select(t => $"*{t.Extension}"));
            }

            if (dialog.ShowDialog() == true)
            {
                // find provider
                var extension = Path.GetExtension(dialog.FileName);
                var projectExplorer = IoC.Get<IProjectExplorer>();
                var projectProvider =
                    providers.First(
                        p =>
                            p.ProjectTypes.Any(
                                t =>
                                    string.Equals(t.Extension, extension,
                                        System.StringComparison.InvariantCultureIgnoreCase)));

                projectExplorer.LoadProject(dialog.FileName, projectProvider);

                var shell = IoC.Get<IShell>();
                shell.ShowTool(projectExplorer);
            }

            return Task.FromResult(true);
        }
    }
}