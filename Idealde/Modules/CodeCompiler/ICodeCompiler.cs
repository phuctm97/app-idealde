using System;
using Idealde.Framework.Commands;
using Idealde.Modules.CodeCompiler.Commands;

namespace Idealde.Modules.CodeCompiler
{
    public interface ICodeCompiler
    {
        bool IsBusy { get; }

        void CompileSingleFile(string sourceFilePath, string outputFilePath = "");

        event EventHandler<string> OutputDataReceived;

        event EventHandler<string> ErrorDataReceived;
    }
}