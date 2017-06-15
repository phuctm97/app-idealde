using System;
using System.Collections.Generic;
using Idealde.Framework.Commands;
using Idealde.Modules.CodeCompiler.Commands;

namespace Idealde.Modules.CodeCompiler
{
    public class CompileError
    {
        public CompileError(int line, int column, string code, string description)
        {
            Line = line;
            Column = column;
            Code = code;
            Description = description;
        }

        public int Line { get; }
        public int Column { get; }
        public string Code { get; }
        public string Description { get; }
    }

    public interface ICodeCompiler
    {
        bool IsBusy { get; }

        bool CanCompileSingleFile(string sourceFilePath);

        void CompileSingleFile(string sourceFilePath);

        event EventHandler<string> OutputDataReceived;

        event EventHandler<IEnumerable<CompileError>> OnExited;
    }
}