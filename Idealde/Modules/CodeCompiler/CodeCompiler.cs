#region Using Namespace

using System;
using System.Diagnostics;
using Idealde.Properties;

#endregion

namespace Idealde.Modules.CodeCompiler
{
    public class CodeCompiler : ICodeCompiler
    {
        public void CompileSingleFile(string sourceFilePath, string outputFilePath)
        {
            var cl = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = $"/c {Settings.Default.VCVarSallPath} && cl /EHsc {sourceFilePath}",
                }
            };

            cl.EnableRaisingEvents = true;
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
            var cl = (Process)sender;
            cl.OutputDataReceived -= OnCompilerOutputDataReceived;
            cl.ErrorDataReceived -= OnCompilerErrorDataReceived;
            cl.Exited -= OnCompilerExited;
            IsBusy = false;
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

        public bool IsBusy { get; private set; }

        public CodeCompiler()
        {
            IsBusy = false;
        }
    }
}