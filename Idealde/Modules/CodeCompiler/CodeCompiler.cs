#region Using Namespace

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.CodeCompiler
{
    public class CodeCompiler : ICodeCompiler
    {
        private readonly string[] _regexSpecialCharacters;
        private readonly List<string> _canCompileFileTypes;
        private readonly List<CompileError> _compileErrors;
        private string _regexableSourceFilePath;

        public bool CanCompileSingleFile(string sourceFilePath)
        {
            return _canCompileFileTypes.Contains(Path.GetExtension(sourceFilePath));
        }

        public void CompileSingleFile(string sourceFilePath)
        {
            var sourceFileDirectory = Path.GetDirectoryName(sourceFilePath);
            _regexableSourceFilePath = GenerateRegexableString(sourceFilePath);

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
                        $"/c {Settings.Default.VCVarSallPath} && cl /EHsc {sourceFilePath} /Fo:{sourceFileDirectory}\\ /Fe:{sourceFileDirectory}\\"
                },
                EnableRaisingEvents = true
            };

            cl.OutputDataReceived += OnCompilerOutputDataReceived;
            cl.ErrorDataReceived += OnCompilerOutputDataReceived;
            cl.Exited += OnCompilerExited;

            IsBusy = true;
            _compileErrors.Clear();

            cl.Start();
            cl.BeginOutputReadLine();
            cl.BeginErrorReadLine();
        }

        private void OnCompilerExited(object sender, EventArgs e)
        {
            // release process
            var cl = (Process) sender;

            var exitCode = cl.ExitCode;

            cl.OutputDataReceived -= OnCompilerOutputDataReceived;
            cl.ErrorDataReceived -= OnCompilerOutputDataReceived;
            cl.Exited -= OnCompilerExited;
            cl.Dispose();

            IsBusy = false;
            OnExited?.Invoke(sender, _compileErrors);
        }

        private void OnCompilerOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e?.Data)) return;

            string pattern = $"{_regexableSourceFilePath}\\(([0-9]+)\\): error ([a-zA-Z0-9]+): (.+)";
            var errorMatch = Regex.Match(e.Data, pattern);

            if (errorMatch.Success)
            {
                var line = 0;
                var column = 0;
                var code = "N/A";
                var description = "N/A";

                if (errorMatch.Groups.Count > 1)
                {
                    int.TryParse(errorMatch.Groups[1].Value, out line);
                }
                if (errorMatch.Groups.Count > 2)
                {
                    code = errorMatch.Groups[2].Value;
                }
                if (errorMatch.Groups.Count > 3)
                {
                    description = errorMatch.Groups[3].Value;
                }
                _compileErrors.Add(new CompileError(line, column, code, description));
            }

            OutputDataReceived?.Invoke(sender, e.Data);
        }

        public event EventHandler<string> OutputDataReceived;

        public event EventHandler<IEnumerable<CompileError>> OnExited;

        public bool IsBusy { get; private set; }

        public CodeCompiler()
        {
            IsBusy = false;

            _canCompileFileTypes = new List<string>();

            _compileErrors = new List<CompileError>();

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
            var regexableString = source;
            foreach (var s in _regexSpecialCharacters)
            {
                regexableString = regexableString.Replace(s, $"\\{s}");
            }
            return regexableString;
        }
    }
}