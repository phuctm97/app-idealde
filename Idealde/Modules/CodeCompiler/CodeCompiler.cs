#region Using Namespace

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Idealde.Framework.Services;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.CodeCompiler
{
    public class CodeCompiler : ICodeCompiler
    {
        // Dependencies
        private readonly IFileManager _fileManager;

        // Backing fields
        private readonly string[] _regexSpecialCharacters;
        private readonly List<string> _canCompileFileTypes;
        private readonly List<CompileError> _compileErrors;
        private readonly List<CompileError> _compileWarnings;
        private string _regexableSourceFilePath;

        public bool CanCompileSingleFile(string extension)
        {
            return _canCompileFileTypes.Contains(extension);
        }

        public async void CompileSingleFile(string sourceFilePath, string fileContent)
        {
            //write new content to temp file
            var tempFilePath = _fileManager.GetTempFilePath(sourceFilePath);
            await _fileManager.Write(tempFilePath, fileContent);

            //generate regexable source file path for error/warning detect
            _regexableSourceFilePath = GenerateRegexableString(tempFilePath);
            
            //generate output directory
            var sourceFileDirectory = Path.GetDirectoryName(sourceFilePath);

            //config cl
            var cl = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments =
                        $"/c {Settings.Default.VCVarSallPath} && cl /EHsc {tempFilePath} /Fo:{sourceFileDirectory}\\ /Fe:{sourceFileDirectory}\\"
                },
                EnableRaisingEvents = true
            };
            cl.OutputDataReceived += OnCompilerOutputDataReceived;
            cl.ErrorDataReceived += OnCompilerOutputDataReceived;
            cl.Exited += OnCompilerExited;

            //reset data
            IsBusy = true;
            _compileErrors.Clear();
            _compileWarnings.Clear();

            // start cl
            cl.Start();
            cl.BeginOutputReadLine();
            cl.BeginErrorReadLine();
        }

        private void OnCompilerExited(object sender, EventArgs e)
        {
            // release process
            var cl = (Process) sender;
            if (cl.ExitCode == 0)
            {
                _compileErrors.Clear();
            }

            cl.OutputDataReceived -= OnCompilerOutputDataReceived;
            cl.ErrorDataReceived -= OnCompilerOutputDataReceived;
            cl.Exited -= OnCompilerExited;
            cl.Dispose();

            IsBusy = false;
            OnExited?.Invoke(_compileErrors, _compileWarnings);
        }

        private void OnCompilerOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e?.Data)) return;

            MatchErrors(e.Data);

            MatchWarnings(e.Data);

            OutputDataReceived?.Invoke(sender, e.Data);
        }

        private void MatchErrors(string output)
        {
            string errorPattern = $"{_regexableSourceFilePath}\\(([0-9]+)\\): error ([a-zA-Z0-9]+): (.+)";
            var errorMatch = Regex.Match(output.ToLower(), errorPattern);

            if (errorMatch.Success)
            {
                var line = 0;
                var column = -1;
                var code = "N/A";
                var description = "N/A";

                if (errorMatch.Groups.Count > 1)
                {
                    int.TryParse(errorMatch.Groups[1].Value, out line);
                }
                if (errorMatch.Groups.Count > 2)
                {
                    code = output.Substring(errorMatch.Groups[2].Index,
                        errorMatch.Groups[2].Length);
                }
                if (errorMatch.Groups.Count > 3)
                {
                    description = output.Substring(errorMatch.Groups[3].Index,
                        errorMatch.Groups[3].Length);
                }
                _compileErrors.Add(new CompileError(line, column, code, description));
            }
        }

        private void MatchWarnings(string output)
        {
            string warningPattern = $"{_regexableSourceFilePath}\\(([0-9]+)\\) : warning ([a-zA-Z0-9]+): (.+)";
            var warningMatch = Regex.Match(output.ToLower(), warningPattern);

            if (warningMatch.Success)
            {
                var line = 0;
                var column = -1;
                var code = "N/A";
                var description = "N/A";

                if (warningMatch.Groups.Count > 1)
                {
                    int.TryParse(warningMatch.Groups[1].Value, out line);
                }
                if (warningMatch.Groups.Count > 2)
                {
                    code = output.Substring(warningMatch.Groups[2].Index,
                        warningMatch.Groups[2].Length);
                }
                if (warningMatch.Groups.Count > 3)
                {
                    description = output.Substring(warningMatch.Groups[3].Index,
                        warningMatch.Groups[3].Length);
                }
                _compileWarnings.Add(new CompileError(line, column, code, description));
            }
        }

        public event EventHandler<string> OutputDataReceived;

        public event CompilerExitedEventHandler OnExited;

        public bool IsBusy { get; private set; }

        public CodeCompiler(IFileManager fileManager)
        {
            _fileManager = fileManager;

            IsBusy = false;

            _canCompileFileTypes = new List<string>();

            _compileErrors = new List<CompileError>();

            _compileWarnings = new List<CompileError>();

            _regexSpecialCharacters = new[]
            {
                "\\",
                ".", "$", "^", "{", "[", "(", "|", ")", "]", "}", "*", "+", "?"
            };

            Initialize();
        }

        private void Initialize()
        {
            _canCompileFileTypes.AddRange(new[]
            {
                ".cpp"
            });

            _regexableSourceFilePath = string.Empty;
        }

        private string GenerateRegexableString(string source)
        {
            var regexableString = source.ToLower();
            foreach (var s in _regexSpecialCharacters)
            {
                regexableString = regexableString.Replace(s, $"\\{s}");
            }
            return regexableString;
        }
    }
}