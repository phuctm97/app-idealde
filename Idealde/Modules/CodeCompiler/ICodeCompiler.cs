﻿#region Using Namespace

using System;
using System.Collections.Generic;

#endregion

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

    public delegate void CompilerExitedEventHandler(IEnumerable<CompileError> errors, IEnumerable<CompileError> warnings
    );

    public interface ICodeCompiler
    {
        bool IsBusy { get; }

        bool CanCompileSingleFile(string sourceFilePath);

        void CompileSingleFile(string sourceFilePath);

        event EventHandler<string> OutputDataReceived;

        event CompilerExitedEventHandler OnExited;
    }
}