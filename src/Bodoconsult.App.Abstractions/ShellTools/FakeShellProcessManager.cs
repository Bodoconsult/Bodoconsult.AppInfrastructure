// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Abstractions.ShellTools;

/// <summary>
/// Fake implementation of <see cref="IShellProcessManager"/> for testing
/// </summary>
public class FakeShellProcessManager : IShellProcessManager
{
    /// <summary>
    /// List of all executed commands
    /// </summary>
    public List<string> Commands { get; } = [];

    /// <summary>
    /// Move a directory
    /// </summary>
    /// <param name="parameters">Parameter set with full source and target paths of the directory to move</param>
    public void MoveDirectory(MoveDirectoryShellProcessParameters parameters)
    {
        if (!Directory.Exists(parameters.SourcePath) || Directory.Exists(parameters.TargetPath))
        {
            throw new ArgumentException("Source path must exists and target path must not be null or empty");
        }

        var cmd = $"MOVE /y \"{parameters.SourcePath}\" \"{parameters.TargetPath}\"".Replace("\\\"", "\"", StringComparison.OrdinalIgnoreCase);
        Debug.Print(cmd);
        Commands.Add(cmd);
    }

    /// <summary>
    /// Remove a directory
    /// </summary>
    /// <param name="parameters">Parameter set with full path of the directory to remove</param>
    public void RemoveDirectory(RemoveDirectoryShellProcessParameters parameters)
    {
        if (!Directory.Exists(parameters.Path))
        {
            throw new ArgumentException("Path must not be null or empty");
        }

        var cmd = $"cmd.exe /C rd \"{parameters.Path}\" /S /Q";
        Debug.Print(cmd);
        Commands.Add(cmd);
    }

    /// <summary>
    /// Run robocopy
    /// </summary>
    /// <param name="parameters">Parameter set with full path of the directory to remove</param>
    public void RunRobocopy(RobocopyShellProcessParameters parameters)
    {
        if (string.IsNullOrEmpty(parameters.Args))
        {
            throw new ArgumentException("Args must not be null or empty");
        }

        var cmd = $"robocopy.exe {parameters.Args}";
        Debug.Print(cmd);
        Commands.Add(cmd);
    }

    /// <summary>
    /// Running a shell command in the shell
    /// </summary>
    /// <param name="parameters">Parameter set for running a shell command</param>
    public void RunShellCommand(ShellCommandShellProcessParameter parameters)
    {
        if (string.IsNullOrEmpty(parameters.Command))
        {
            throw new ArgumentException("Command must not be null or empty");
        }

        var cmd = $"cmd.exe /C {parameters.Command}";
        Debug.Print(cmd);
        Commands.Add(cmd);
    }

    /// <summary>
    /// Kill a task (TASKKILL)
    /// </summary>
    /// <param name="parameters">Parameter set with an ID of the process to kill</param>
    public void TaskKill(TaskKillShellProcessParameter parameters)
    {
        if (parameters.ProcessId<=0)
        {
            throw new ArgumentException("ProcessID must be greater or equalt to 0");
        }

        var cmd = $"taskkill /PID {parameters.ProcessId} /F";
        Debug.Print(cmd);
        Commands.Add(cmd);
    }
}