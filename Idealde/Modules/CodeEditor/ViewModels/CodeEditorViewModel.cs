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
using ScintillaNET;
using Command = Idealde.Framework.Commands.Command;

#endregion

namespace Idealde.Modules.CodeEditor.ViewModels
{
    public class CodeEditorViewModel : PersistedDocument, ICodeEditor
    {
        // Backing fields

        #region Backing fields

        private ICodeEditorView _view;
        private string _fileContent;
        private Lexer _fileLexer;
        private readonly ILanguageDefinitionManager _languageDefinitionManager;
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
            var codeCompiler = IoC.Get<ICodeCompiler>();
            return !codeCompiler.IsBusy && codeCompiler.CanCompileSingleFile(FilePath);
        }

        private bool CanRunSingleFile(string filePath, out string outputFilePath)
        {
            outputFilePath = FilePath.Replace(Path.GetExtension(FilePath), ".exe");
            if (File.Exists(_outputFilePath))
            {
                return true;
            }

            outputFilePath = FilePath.Replace(Path.GetExtension(FilePath), ".bat");
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

            //semaphore and mutex for multi-processing
            var semaphore = 1;
            var completed = false;

            //final result
            var result = false;

            //show output
            shell.ShowTool(output);

            //reset output
            output.Clear();
            output.AppendLine($"----- Compile single file starts");
            output.AppendLine($"----- File: {FilePath}");

            // reset error list
            errorList.Clear();

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

            EventHandler<IEnumerable<CompileError>> compileExitedHandler = null;
            compileExitedHandler = delegate(object s, IEnumerable<CompileError> errors)
            {
                //check for erros
                var compileErrors = errors as IList<CompileError> ?? errors.ToList();
                if (!compileErrors.Any())
                {
                    //no error
                    result = true;
                    output.AppendLine("----- Compile single file successfully");
                }
                else
                {
                    //has error(s)
                    result = false;
                    output.AppendLine("----- Compile single file failed");

                    //show error(s)
                    foreach (var error in compileErrors)
                    {
                        errorList.AddItem(ErrorListItemType.Error, error.Code, error.Description, filePath, error.Line,
                            error.Column);
                    }
                    shell.ShowTool(errorList);
                }

                // release subcribed delegate
                codeCompiler.OutputDataReceived -= outputReceivedHandler;
                codeCompiler.OnExited -= compileExitedHandler;
                completed = true;
            };
            codeCompiler.OutputDataReceived += outputReceivedHandler;
            codeCompiler.OnExited += compileExitedHandler;

            // compile
            codeCompiler.CompileSingleFile(FilePath);

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