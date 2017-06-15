using System;
using Idealde.Framework.Commands;
using Idealde.Modules.CodeCompiler.Commands;

namespace Idealde.Modules.CodeCompiler
{
    public interface ICodeCompiler
    {
        bool IsBusy { get; }

        bool CanCompileSingleFile(string sourceFilePath);

        void CompileSingleFile(string sourceFilePath);

        event EventHandler<string> OutputDataReceived;

        event EventHandler<string> ErrorDataReceived;

        event EventHandler OnExited;
    }
}