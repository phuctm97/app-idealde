#region Using Namespace

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.CodeCompiler
{
    public class CodeCompiler : ICodeCompiler
    {
        private readonly List<string> _canCompileFileTypes;

        public bool CanCompileSingleFile(string sourceFilePath)
        {
            return _canCompileFileTypes.Contains(Path.GetExtension(sourceFilePath));
        }

        public void CompileSingleFile(string sourceFilePath)
        {
            string sourceFileDirectory = Path.GetDirectoryName(sourceFilePath);

            var cl = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = $"/c {Settings.Default.VCVarSallPath} && cl /EHsc {sourceFilePath} /Fo:{sourceFileDirectory}\\ /Fe:{sourceFileDirectory}\\"
                },
                EnableRaisingEvents = true
            };

            cl.OutputDataReceived += OnCompilerOutputDataReceived;
            cl.ErrorDataReceived += OnCompilerErrorDataReceived;
            cl.Exited += OnCompilerExited;

            IsBusy = true;
            cl.Start();
            cl.BeginOutputReadLine();
            cl.BeginErrorReadLine();
        }

        private void OnCompilerExited(object sender, EventArgs e)
        {
            // release process
            var cl = (Process) sender;
            cl.OutputDataReceived -= OnCompilerOutputDataReceived;
            cl.ErrorDataReceived -= OnCompilerErrorDataReceived;
            cl.Exited -= OnCompilerExited;
            cl.Dispose();

            IsBusy = false;
            OnExited?.Invoke(sender, e);
        }

        private void OnCompilerErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputDataReceived?.Invoke(sender, e.Data);
        }

        private void OnCompilerOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ErrorDataReceived?.Invoke(sender, e.Data);
        }

        public event EventHandler<string> OutputDataReceived;

        public event EventHandler<string> ErrorDataReceived;

        public event EventHandler OnExited;

        public bool IsBusy { get; private set; }

        public CodeCompiler()
        {
            IsBusy = false;

            _canCompileFileTypes = new List<string>();

            Initialize();
        }

        private void Initialize()
        {
            _canCompileFileTypes.AddRange(new[]
            {
                ".cpp"
            });
        }
    }
}