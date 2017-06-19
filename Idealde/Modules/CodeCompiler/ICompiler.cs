using System;
using Idealde.Framework.Projects;

namespace Idealde.Modules.CodeCompiler
{
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