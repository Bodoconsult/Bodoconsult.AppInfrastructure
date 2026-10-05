// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.ShellTools;

namespace Bodoconsult.App.Windows.ShellTools;

/// <summary>
/// Implementation of <see cref="IShellProcessManager"/> to run commands in Windows shell
/// </summary>
public class WinShellProcessManager : IShellProcessManager
{
    /// <summary>
    /// Move a directory
    /// </summary>
    /// <param name="parameters">Parameter set with full source and target paths of the directory to move</param>
    public void MoveDirectory(MoveDirectoryShellProcessParameters parameters)
    {
        if (!Directory.Exists(parameters.SourcePath) || Directory.Exists(parameters.TargetPath))
        {
            parameters.HandleExceptionDelegate?.Invoke(new ArgumentException("Source path must exists and target path must not be null or empty"));
            return;
        }

        var cmd = $"MOVE /y \"{parameters.SourcePath}\" \"{parameters.TargetPath}\"".Replace("\\\"", "\"", StringComparison.OrdinalIgnoreCase);
        RunInShellWait(cmd, parameters);
    }

    /// <summary>
    /// Remove a directory
    /// </summary>
    /// <param name="parameters">Parameter set with full path of the directory to remove</param>
    public void RemoveDirectory(RemoveDirectoryShellProcessParameters parameters)
    {
        if (!Directory.Exists(parameters.Path))
        {
            parameters.HandleExceptionDelegate?.Invoke(new ArgumentException("Path must not be null or empty"));
            return;
        }

        var cmd = $"""rd "{parameters.Path}" /S /Q""";
        RunInShellWait(cmd, parameters);
    }

    /// <summary>
    /// Run robocopy
    /// </summary>
    /// <param name="parameters">Parameter set with full path of the directory to remove</param>
    public void RunRobocopy(RobocopyShellProcessParameters parameters)
    {
        if (!Directory.Exists(parameters.Args))
        {
            parameters.HandleExceptionDelegate?.Invoke(new ArgumentException("Args must not be null or empty"));
            return;
        }

        ShellAndWait("robocopy.exe", parameters.Args, parameters);
    }

    /// <summary>
    /// Running a shell command in the shell
    /// </summary>
    /// <param name="parameters">Parameter set for running a shell command</param>
    public void RunShellCommand(ShellCommandShellProcessParameter parameters)
    {
        if (string.IsNullOrEmpty(parameters.Command))
        {
            parameters.HandleExceptionDelegate?.Invoke(new ArgumentException("Command must not be null or empty"));
            return;
        }

        RunInShellWait(parameters.Command, parameters);
    }

    /// <summary>
    /// Kill a task (TASKKILL)
    /// </summary>
    /// <param name="parameters">Parameter set with an ID of the process to kill</param>
    public void TaskKill(TaskKillShellProcessParameter parameters)
    {
        if (parameters.ProcessId<=0)
        {
            parameters.HandleExceptionDelegate?.Invoke(new ArgumentException("Process ID must be greater than 0"));
            return;
        }
        // taskkill /PID 1234 /F
        TaskKillInternal(parameters.ProcessId, parameters);
    }

    private static void TaskKillInternal(int processId, BaseShellProcessParameters parameters)
    {
        parameters.DoNotCheckExitCode = true;
        var cmd = $"taskkill /PID {processId} /F";
        RunInShellWait(cmd, parameters);
    }

    /// <summary>
    /// Run command in shell
    /// </summary>
    /// <param name="strShellCmd">Shell command to run</param>
    /// <param name="parameters">Parameters to use for state and exception handling</param>
    private static void RunInShellWait(string strShellCmd, BaseShellProcessParameters parameters)
    {
        if (string.IsNullOrEmpty(strShellCmd))
        {
            return;
        }

        var psi = new ProcessStartInfo("cmd.exe ", $@"/C {strShellCmd}")
        {
            WorkingDirectory = parameters.WorkingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WindowStyle = ProcessWindowStyle.Hidden, 
            CreateNoWindow = true
        };

        var process = new Process
        {
            StartInfo = psi
        };

        //ArgumentNullException.ThrowIfNull(process);

        process.OutputDataReceived += (_, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }
            parameters.StatusMessageDelegate?.Invoke(e.Data);
        };
        process.ErrorDataReceived += (_, e) =>
        {
            parameters.HandleExceptionDelegate?.Invoke(new Exception(e.Data));
        };

        process.Start();
        process.BeginOutputReadLine();
        var processId = process.Id;

        process.WaitForExit();

        if (process.ExitCode > 0 && !parameters.DoNotCheckExitCode)
        {
            var msg = $"WinShellProcessManager:Error>>cmd.exe /C {strShellCmd}>>ExitCode {process.ExitCode}";
            parameters.HandleExceptionDelegate?.Invoke(new Exception(msg));
            parameters.StatusMessageDelegate?.Invoke(msg);
        }
        process.Close();

        if (parameters.DoNotCheckExitCode)
        {
            return;
        }

        TaskKillInternal(processId, parameters);
    }

    private static void ShellAndWait(string exe, string args, BaseShellProcessParameters parameters)
    {
        var psi = new ProcessStartInfo(exe, args)
        {
            WorkingDirectory = parameters.WorkingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true
        };

        try
        {
            parameters.StatusMessageDelegate?.Invoke($"Run {exe} {args}");

            RunProcess(exe, args, psi, parameters);
        }
        catch (Exception ex)
        {
            parameters.HandleExceptionDelegate?.Invoke(ex);
        }
    }

    private static void RunProcess(string exe, string args, ProcessStartInfo psi, BaseShellProcessParameters parameters)
    {
        var p = new Process
        {
            StartInfo = psi,
            PriorityClass = ProcessPriorityClass.AboveNormal
        };

        p.OutputDataReceived += (_, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }
            parameters.StatusMessageDelegate?.Invoke(e.Data);
        };
        p.ErrorDataReceived += (_, e) =>
        {
            parameters.HandleExceptionDelegate?.Invoke(new Exception(e.Data));
        };

        p.BeginOutputReadLine();

        p.Start();

        var processId = p.Id;

        p.WaitForExit();

        if (p.ExitCode > 0 && !parameters.DoNotCheckExitCode)
        {
            var msg = $"WinShellProcessManager:Error>>{exe} {args}>>ExitCode {p.ExitCode}";
            parameters.HandleExceptionDelegate?.Invoke(new Exception(msg));
            parameters.StatusMessageDelegate?.Invoke(msg);
        }
        p.Close();

        if (parameters.DoNotCheckExitCode)
        {
            return;
        }

        TaskKillInternal(processId, parameters);
    }
}