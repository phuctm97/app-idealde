#region Using Namespace

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;
using Idealde.Modules.CodeCompiler;
using Idealde.Modules.CodeCompiler.Commands;
using Idealde.Modules.ErrorList;
using Idealde.Modules.Output;
using Idealde.Modules.StatusBar;
using Idealde.Properties;
using ScintillaNET;
using Command = Idealde.Framework.Commands.Command;

#endregion

namespace Idealde.Modules.CodeEditor.ViewModels
{
    public class CodeEditorViewModel : PersistedDocument, ICodeEditor
    {
        // Dependencies

        #region Dependencies

        private readonly ILanguageDefinitionManager _languageDefinitionManager;

        #endregion

        // Backing fields

        #region Backing fields

        private ICodeEditorView _view;
        private string _fileContent;
        private Lexer _fileLexer;

        #endregion

        // Initializations

        #region Initializations

        public CodeEditorViewModel(ILanguageDefinitionManager languageDefinitionManager)
        {
            _languageDefinitionManager = languageDefinitionManager;
            _fileContent = string.Empty;
        }

        protected override void OnViewLoaded(object view)
        {
            _view = (ICodeEditorView) view;
            if (_view == null) throw new InvalidCastException();
            _view.IsDirtyChanged += OnDirtyChanged;
            _view.SetResourceDirectory("Resources");
            _view.SetContent(_fileContent);
            _view.SetLexer(_fileLexer);
            base.OnViewLoaded(view);
        }

        #endregion

        // Persisted behaviors

        #region Persisted behaviors

        private void OnDirtyChanged(bool q)
        {
            IsDirty = q;
        }

        protected override Task DoNew()
        {
            _fileContent = string.Empty;
            _view?.SetContent(_fileContent);

            SetLanguage(_languageDefinitionManager.GetLanguage(Path.GetExtension(FileName)).Lexer);
            return Task.FromResult(true);
        }

        protected override Task DoLoad()
        {
            _fileContent = File.ReadAllText(FilePath);
            _view?.SetContent(_fileContent);

            SetLanguage(_languageDefinitionManager.GetLanguage(Path.GetExtension(FilePath)).Lexer);
            IsDirty = false;
            return Task.FromResult(true);
        }

        protected override async Task DoSave()
        {
            var viewContent = _view.GetContent();
            if (!File.Exists(FilePath))
            {
                using (var sw = File.CreateText(FilePath))
                {
                    await sw.WriteLineAsync(viewContent);
                }
            }
            else
            {
                var stream = new FileStream(FilePath, FileMode.Truncate, FileAccess.Write);
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    await writer.WriteLineAsync(viewContent);
                }
                stream.Close();
            }
        }

        #endregion

        // Code editor features

        #region Features

        public void SetLanguage(Lexer lexer)
        {
            _fileLexer = lexer;
            _view?.SetLexer(_fileLexer);
        }

        public string GetContent()
        {
            return _view.GetContent();
        }

        public void Goto(int row, int column)
        {
            _view.Goto(row, column);
            _view.EditorFocus();
        }

        #endregion

        // Compile single file command handler

        #region Compile and run single file command handler

        void ICommandHandler<CompileSingleFileCommandDefinition>.Update(Command command)
        {
            ICompiler compiler;
            if (CanCompileSingleFile(FilePath, out compiler))
            {
                command.IsEnabled = true;
                command.Tag = compiler;
            }
            else
            {
                command.IsEnabled = false;
            }
        }

        async Task ICommandHandler<CompileSingleFileCommandDefinition>.Run(Command command)
        {
            // copy current content to temp file
            var fileManager = IoC.Get<IFileManager>();
            var tempFilePath = fileManager.GetTempFilePath(FilePath);
            await fileManager.Write(tempFilePath, GetContent());

            // compile
            var compiler = command.Tag as ICompiler;
            await CompileSingleFile(compiler, tempFilePath);
        }

        void ICommandHandler<RunSingleFileCommandDefinition>.Update(Command command)
        {
            string outputFileName;
            if (CanRunSingleFile(FilePath, out outputFileName))
            {
                command.IsEnabled = true;
                command.Tag = outputFileName;
            }
            else
            {
                command.IsEnabled = false;
            }
        }

        Task ICommandHandler<RunSingleFileCommandDefinition>.Run(Command command)
        {
            return RunSingleFile(command.Tag as string);
        }

        void ICommandHandler<CompileAndRunSingleFileCommandDefinition>.Update(Command command)
        {
            ICompiler compiler;
            if (CanCompileSingleFile(FilePath, out compiler))
            {
                command.IsEnabled = true;
                command.Tag = compiler;
            }
            else
            {
                command.IsEnabled = false;
            }
        }

        async Task ICommandHandler<CompileAndRunSingleFileCommandDefinition>.Run(Command command)
        {
            // copy current content to temp file
            var fileManager = IoC.Get<IFileManager>();
            var tempFilePath = fileManager.GetTempFilePath(FilePath);
            await fileManager.Write(tempFilePath, GetContent());

            // compile
            var compiler = command.Tag as ICompiler;
            if (!await CompileSingleFile(compiler, tempFilePath)) return;
            
            // run
            string outputFilePath;
            if (!CanRunSingleFile(FilePath, out outputFilePath)) return;

            await RunSingleFile(outputFilePath);
        }

        private bool CanCompileSingleFile(string filePath, out ICompiler outCompiler)
        {
            outCompiler = null;

            if (string.IsNullOrWhiteSpace(filePath)) { return false;}

            var compilers = IoC.GetAll<ICompiler>();
            foreach (var compiler in compilers)
            {
                if (!compiler.IsBusy && compiler.CanCompile(filePath))
                {
                    outCompiler = compiler;
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> CompileSingleFile(ICompiler compiler, string filePath)
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
            output.AppendLine($"----- {Resources.CompileSingleFileStartOutput}");

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
            EventHandler<string> outputReceivedHandler = delegate(object s, string data)
            {
                while (semaphore <= 0)
                {
                }

                semaphore--;
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var data2 = ConvertTempPathToFilePath(filePath, FilePath, data);
                    output.AppendLine($"> {data2}");
                }
                semaphore++;
            };

            CompilerExitedEventHandler compileExitedHandler = null;
            compileExitedHandler = delegate(IEnumerable<CompileError> errors, IEnumerable<CompileError> warnings)
            {
                //check for errors
                var compileErrors = errors as IList<CompileError> ?? errors.ToList();
                var compileWarnings = warnings as IList<CompileError> ?? warnings.ToList();

                //show error(s)
                foreach (var error in compileErrors)
                {
                    var path = ConvertTempPathToFilePath(filePath, FilePath, error.Path);

                    errorList.AddItem(ErrorListItemType.Error, error.Code, error.Description, path, error.Line,
                        error.Column);
                }
                //show warning(s)
                foreach (var warning in compileWarnings)
                {
                    var path = ConvertTempPathToFilePath(filePath, FilePath, warning.Path);

                    errorList.AddItem(ErrorListItemType.Warning, warning.Code, warning.Description, path,
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
                    output.AppendLine($"----- {Resources.CompileSingleFileFinishSuccessfullyOutput}");
                    statusBar.Items[0].Message = Resources.CompileSingleFileFinishSuccessfullyOutput;
                }
                else
                {
                    //has error(s)
                    result = false;
                    output.AppendLine($"----- {Resources.CompileSingleFileFailedOutput}");
                    statusBar.Items[0].Message = Resources.CompileSingleFileFailedOutput;
                }

                // release subcribed delegate
                compiler.OutputDataReceived -= outputReceivedHandler;
                compiler.OnExited -= compileExitedHandler;
                completed = true;
            };
            compiler.OutputDataReceived += outputReceivedHandler;
            compiler.OnExited += compileExitedHandler;

            // compile
            compiler.Compile(filePath, Path.GetDirectoryName(FilePath));

            // wait for compilation finish
            while (!completed) await Task.Delay(25);
            return result;
        }

        private bool CanRunSingleFile(string filePath, out string outputFileName)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                outputFileName = string.Empty;
                return false;
            }

            outputFileName = Path.ChangeExtension(filePath, ".exe");
            if (File.Exists(outputFileName))
            {
                return true;
            }

            outputFileName = Path.ChangeExtension(filePath, ".bat");
            if (File.Exists(outputFileName))
            {
                return true;
            }

            return false;
        }

        private Task RunSingleFile(string outputFileName)
        {
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

        private string ConvertTempPathToFilePath(string tempPath, string filePath, string source)
        {
            do
            {
                var i = source.IndexOf(tempPath, StringComparison.OrdinalIgnoreCase);
                if (i == -1) break;

                source = source.Remove(i, tempPath.Length);
                source = source.Insert(i, filePath);
            } while (true);

            return source;
        }

        #endregion
    }
}