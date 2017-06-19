#region Using Namespace

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Modules.ProjectExplorer;

#endregion

namespace Idealde.Modules.CodeCompiler.Commands
{
    public class RunProjectCommandHandler :
        ICommandHandler<RunProjectCommandDefinition>
    {
        public void Update(Command command)
        {
            command.IsEnabled = false;

            var projectExplorer = IoC.Get<IProjectExplorer>();
            if (projectExplorer?.CurrentProjectInfo?.Provider != null)
            {
                var provider = projectExplorer.CurrentProjectInfo.Provider;

                var outputFileName =
                    $"{provider.GetBinPath(projectExplorer.CurrentProjectInfo)}\\{projectExplorer.CurrentProjectInfo.ProjectName}.exe";
                if (File.Exists(outputFileName))
                {
                    command.IsEnabled = true;
                    command.Tag = outputFileName;
                }

                outputFileName = Path.ChangeExtension(outputFileName, ".bat");
                if (File.Exists(outputFileName))
                {
                    command.IsEnabled = true;
                    command.Tag = outputFileName;
                }
            }
        }

        public Task Run(Command command)
        {
            var outputFileName = command.Tag as string ?? string.Empty;
            if (!File.Exists(outputFileName)) return Task.FromResult(false);

            var bin = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = outputFileName,
                    UseShellExecute = true,
                    CreateNoWindow = false
                }
            };

            bin.Start();
            return Task.FromResult(true);
        }
    }
}