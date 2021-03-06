﻿#region Using Namespace

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Idealde.Framework.Projects;
using Idealde.Modules.ProjectExplorer.Providers;

#endregion

namespace Idealde.Modules.Compiler
{
    public class CppCompiler : ICompiler
    {
        // Backing fields
        private readonly List<CompileError> _compileErrors;
        private readonly List<CompileError> _compileWarnings;

        // Events
        public event EventHandler<string> OutputDataReceived;
        public event CompilerExitedEventHandler OnExited;

        // Initializations
        public CppCompiler()
        {
            IsBusy = false;

            _compileErrors = new List<CompileError>();

            _compileWarnings = new List<CompileError>();
        }

        public bool IsBusy { get; private set; }

        public bool CanCompile(ProjectInfoBase project)
        {
            if (!(project is CppProjectInfo)) return false;
            if (!(project?.Provider is CppProjectProvider)) return false;
            return true;
        }

        public bool CanCompile(string file)
        {
            return string.Equals(Path.GetExtension(file), ".cpp", StringComparison.OrdinalIgnoreCase);
        }

        public void Compile(ProjectInfoBase project)
        {
            // get all files
            var files = new List<string>();
            foreach (var projectFile in project.Files)
            {
                var extension = Path.GetExtension(projectFile.RealPath);
                if (string.Equals(extension, ".c", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(extension, ".cxx", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(extension, ".cc", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(extension, ".cpp", StringComparison.OrdinalIgnoreCase))
                {
                    files.Add(projectFile.RealPath);
                }
            }

            // generate build command
            var buildCommand = "cl /EHsc";
            buildCommand += " " + string.Join(" ", files.Select(p => $"\"{p}\""));

            var cppProject = project as CppProjectInfo;
            var cppProvider = project.Provider as CppProjectProvider;
            if (cppProject == null) return;

            // libs
            buildCommand += " " + string.Join(" ", cppProject.PrebuiltLibraries.Select(p => $"\"{p}\""));
            // TODO: libpath

            // includes
            buildCommand += " " + string.Join(" ", cppProject.IncludeDirectories.Select(p => $"/I\"{p}\""));

            // output
            buildCommand += " " + $"/Fe:\"{cppProvider?.GetBinPath(cppProject)}\\{cppProject?.ProjectName}.exe\"";

            // reset data
            IsBusy = true;
            _compileErrors.Clear();
            _compileWarnings.Clear();

            // generate cl
            var cl = GenerateCl(buildCommand);
            
            // start cl
            StartCl(cl, cppProvider?.GetCompileDirectory(cppProject));
        }

        public void Compile(string file, string outputPath)
        {
            // generate build command
            var buildCommand = $"cl /EHsc \"{file}\"";

            //reset data
            IsBusy = true;
            _compileErrors.Clear();
            _compileWarnings.Clear();

            // generate cl
            var cl = GenerateCl(buildCommand);

            // start cl
            StartCl(cl, outputPath);
        }

        private Process GenerateCl(string buildCommand)
        {
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
                    Arguments = $"/c \"\"{Properties.Settings.Default.VCVarSallPath}\" & {buildCommand}\""
                },
                EnableRaisingEvents = true
            };
            cl.OutputDataReceived += OnCompilerOutputDataReceived;
            cl.ErrorDataReceived += OnCompilerOutputDataReceived;
            cl.Exited += OnCompilerExited;

            return cl;
        }

        private void StartCl(Process cl, string workingDirectory)
        {
            cl.StartInfo.WorkingDirectory = workingDirectory;
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
            string errorPattern = $"(.*?)\\(([0-9]+)\\) *: *error ([a-zA-Z0-9]+): (.+)";
            var errorMatch = Regex.Match(output, errorPattern);

            if (errorMatch.Success)
            {
                var path = string.Empty;
                var line = 0;
                var column = -1;
                var code = "N/A";
                var description = "N/A";

                if (errorMatch.Groups.Count > 1)
                {
                    path = errorMatch.Groups[1].Value;
                }
                if (errorMatch.Groups.Count > 2)
                {
                    int.TryParse(errorMatch.Groups[2].Value, out line);
                }
                if (errorMatch.Groups.Count > 3)
                {
                    code = errorMatch.Groups[3].Value;
                }
                if (errorMatch.Groups.Count > 4)
                {
                    description = errorMatch.Groups[4].Value;
                }
                _compileErrors.Add(new CompileError(line, column, code, description, path));
            }
        }

        private void MatchWarnings(string output)
        {
            string warningPattern = $"(.*?)\\(([0-9]+)\\) *: *warning ([a-zA-Z0-9]+): (.+)";
            var warningMatch = Regex.Match(output, warningPattern);

            if (warningMatch.Success)
            {
                var path = string.Empty;
                var line = 0;
                var column = -1;
                var code = "N/A";
                var description = "N/A";

                if (warningMatch.Groups.Count > 1)
                {
                    path = warningMatch.Groups[1].Value;
                }
                if (warningMatch.Groups.Count > 2)
                {
                    int.TryParse(warningMatch.Groups[2].Value, out line);
                }
                if (warningMatch.Groups.Count > 3)
                {
                    code = warningMatch.Groups[3].Value;
                }
                if (warningMatch.Groups.Count > 4)
                {
                    description = warningMatch.Groups[4].Value;
                }
                _compileWarnings.Add(new CompileError(line, column, code, description, path));
            }
        }
    }
}