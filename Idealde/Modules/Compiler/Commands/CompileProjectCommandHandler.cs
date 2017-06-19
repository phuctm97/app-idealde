using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Projects;
using Idealde.Framework.Services;
using Idealde.Modules.ErrorList;
using Idealde.Modules.Output;
using Idealde.Modules.ProjectExplorer;
using Idealde.Modules.StatusBar;
using Idealde.Properties;

namespace Idealde.Modules.Compiler.Commands
{
    public class CompileProjectCommandHandler
        : ICommandHandler<CompileProjectCommandDefinition>
    {
        public void Update(Command command)
        {
            var projectExplorer = IoC.Get<IProjectExplorer>();
            if (projectExplorer?.CurrentProjectInfo?.Provider?.Compiler != null)
            {
                command.IsEnabled = true;
            }
            else
            {
                command.IsEnabled = false;
            }
        }

        public async Task Run(Command command)
        {
            var projectExplorer = IoC.Get<IProjectExplorer>();
            var compiler = projectExplorer.CurrentProjectInfo.Provider.Compiler;
            if (compiler == null) return;

            await Compile(compiler, projectExplorer.CurrentProjectInfo);
        }

        private async Task<bool> Compile(ICompiler compiler, ProjectInfoBase project)
        {
            //dependencies
            var shell = IoC.Get<IShell>();
            var output = IoC.Get<IOutput>();
            var errorList = IoC.Get<IErrorList>();
            var statusBar = IoC.Get<IStatusBar>();

            //semaphore and mutex for multi-processing
            var semaphore = 1;
            var completed = false;

            //final result
            var result = false;

            //show output
            shell.ShowTool(output);

            //reset output
            output.Clear();
            output.AppendLine($"----- {Resources.CompileProjectStartOutput}");

            // reset error list
            errorList.Clear();

            // reset status bar first item
            if (statusBar.Items.Count == 0)
            {
                statusBar.AddItem(Resources.CompileSingleFileStartOutput,
                    new System.Windows.GridLength(1, System.Windows.GridUnitType.Auto));
            }
            else
            {
                statusBar.Items[0].Message = Resources.CompileSingleFileStartOutput;
            }

            //handle compile events
            EventHandler<string> outputReceivedHandler = delegate (object s, string data)
            {
                while (semaphore <= 0)
                {
                }

                semaphore--;
                if (!string.IsNullOrWhiteSpace(data))
                {
                    output.AppendLine($"> {data}");
                }
                semaphore++;
            };

            CompilerExitedEventHandler compileExitedHandler = null;
            compileExitedHandler = delegate (IEnumerable<CompileError> errors, IEnumerable<CompileError> warnings)
            {
                //check for errors
                var compileErrors = errors as IList<CompileError> ?? errors.ToList();
                var compileWarnings = warnings as IList<CompileError> ?? warnings.ToList();

                //show error(s)
                foreach (var error in compileErrors)
                {
                    errorList.AddItem(ErrorListItemType.Error, error.Code, error.Description, error.Path, error.Line,
                        error.Column);
                }
                //show warning(s)
                foreach (var warning in compileWarnings)
                {
                    errorList.AddItem(ErrorListItemType.Warning, warning.Code, warning.Description, warning.Path,
                        warning.Line,
                        warning.Column);
                }
                if (compileErrors.Any() || compileWarnings.Any())
                {
                    shell.ShowTool(errorList);
                }

                //result
                if (!compileErrors.Any())
                {
                    //no error
                    result = true;
                    output.AppendLine($"----- {Resources.CompileProjectFinishSuccessfullyOutput}");
                    statusBar.Items[0].Message = Resources.CompileProjectFinishSuccessfullyOutput;
                }
                else
                {
                    //has error(s)
                    result = false;
                    output.AppendLine($"----- {Resources.CompileProjectFailedOutput}");
                    statusBar.Items[0].Message = Resources.CompileProjectFailedOutput;
                }

                // release subcribed delegate
                compiler.OutputDataReceived -= outputReceivedHandler;
                compiler.OnExited -= compileExitedHandler;
                completed = true;
            };
            compiler.OutputDataReceived += outputReceivedHandler;
            compiler.OnExited += compileExitedHandler;

            // compile
            compiler.Compile(project);

            // wait for compilation finish
            while (!completed) await Task.Delay(25);
            return result;
        }

    }
}