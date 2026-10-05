// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.ShellTools;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for shell based operations manager
/// </summary>
public interface IShellProcessManager
{
    /// <summary>
    /// Move a directory (MOVE)
    /// </summary>
    /// <param name="parameters">Parameter set with full source and target paths of the directory to move</param>
    void MoveDirectory(MoveDirectoryShellProcessParameters parameters);

    /// <summary>
    /// Remove a directory (RD)
    /// </summary>
    /// <param name="parameters">Parameter set with full path of the directory to remove</param>
    void RemoveDirectory(RemoveDirectoryShellProcessParameters parameters);

    /// <summary>
    /// Run robocopy (ROBOCOPY.EXE)
    /// </summary>
    /// <param name="parameters">Parameter set with full path of the directory to remove</param>
    void RunRobocopy(RobocopyShellProcessParameters parameters);

    /// <summary>
    /// Running a shell command in the shell
    /// </summary>
    /// <param name="parameters">Parameter set for running a shell command</param>
    void RunShellCommand(ShellCommandShellProcessParameter parameters);

    /// <summary>
    /// Kill a task (TASKKILL)
    /// </summary>
    /// <param name="parameters">Parameter set with an ID of the process to kill</param>
    void TaskKill(TaskKillShellProcessParameter parameters);
}