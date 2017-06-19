using System;
using System.Collections.Generic;
using Idealde.Framework.Projects;

namespace Idealde.Modules.CodeCompiler
{
    public class CompileError
    {
        public CompileError(int line, int column, string code, string description, string path)
        {
            Line = line;
            Column = column;
            Code = code;
            Description = description;
            Path = path;
        }

        public string Path { get; }
        public int Line { get; }
        public int Column { get; }
        public string Code { get; }
        public string Description { get; }
    }

    public delegate void CompilerExitedEventHandler(IEnumerable<CompileError> errors, IEnumerable<CompileError> warnings
    );

    public interface ICompiler
    {
        bool IsBusy { get; }

        bool CanCompile(ProjectInfoBase project);

        bool CanCompile(string file);

        void Compile(ProjectInfoBase project);

        void Compile(string file, string outputPath);

        event EventHandler<string> OutputDataReceived;

        event CompilerExitedEventHandler OnExited;
    }
}