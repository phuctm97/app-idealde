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
        private string _outputFilePath;

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
            command.IsEnabled = CanCompileSingleFile(FilePath);
        }

        Task ICommandHandler<CompileSingleFileCommandDefinition>.Run(Command command)
        {
            return CompileSingleFile(FilePath);
        }

        void ICommandHandler<RunSingleFileCommandDefinition>.Update(Command command)
        {
            command.IsEnabled = CanRunSingleFile(FilePath, out _outputFilePath);
        }

        Task ICommandHandler<RunSingleFileCommandDefinition>.Run(Command command)
        {
            return RunSingleFile(_outputFilePath);
        }

        void ICommandHandler<CompileAndRunSingleFileCommandDefinition>.Update(Command command)
        {
            command.IsEnabled = CanCompileSingleFile(FilePath);
        }

        async Task ICommandHandler<CompileAndRunSingleFileCommandDefinition>.Run(Command command)
        {
            if (!await CompileSingleFile(FilePath)) return;

            if (!CanRunSingleFile(FilePath, out _outputFilePath)) return;

            await RunSingleFile(_outputFilePath);
        }

        private bool CanCompileSingleFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return false;

            var codeCompiler = IoC.Get<ICodeCompiler>();
            return !codeCompiler.IsBusy && codeCompiler.CanCompileSingleFile(Path.GetExtension(filePath));
        }

        private bool CanRunSingleFile(string filePath, out string outputFilePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                outputFilePath = string.Empty;
                return false;
            }

            outputFilePath = FilePath.Replace(Path.GetExtension(filePath), ".exe");
            if (File.Exists(_outputFilePath))
            {
                return true;
            }

            outputFilePath = FilePath.Replace(Path.GetExtension(filePath), ".bat");
            if (File.Exists(_outputFilePath))
            {
                return true;
            }

            return false;
        }

        private async Task<bool> CompileSingleFile(string filePath)
        {
            //dependencies
            var codeCompiler = IoC.Get<ICodeCompiler>();
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
                    output.AppendLine($"> {data}");
                }
                semaphore++;
            };

            CompilerExitedEventHandler compileExitedHandler = null;
            compileExitedHandler = delegate(IEnumerable<CompileError> errors, IEnumerable<CompileError> warnings)
            {
                //check for erros
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
                codeCompiler.OutputDataReceived -= outputReceivedHandler;
                codeCompiler.OnExited -= compileExitedHandler;
                completed = true;
            };
            codeCompiler.OutputDataReceived += outputReceivedHandler;
            codeCompiler.OnExited += compileExitedHandler;

            // compile
            codeCompiler.CompileSingleFile(FilePath, GetContent());

            // wait for compilation finish
            while (!completed) await Task.Delay(25);
            return result;
        }

        private Task RunSingleFile(string outputFilePath)
        {
            if (!File.Exists(outputFilePath)) return Task.FromResult(false);

            var output = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = outputFilePath,
                    UseShellExecute = true,
                    CreateNoWindow = false
                }
            };

            output.Start();
            return Task.FromResult(true);
        }

        #endregion
    }
}