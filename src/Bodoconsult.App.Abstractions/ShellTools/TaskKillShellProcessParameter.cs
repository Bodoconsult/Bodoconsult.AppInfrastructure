// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.App.Abstractions.ShellTools;

/// <summary>
/// Parameter set for killing an OS process
/// </summary>
public class TaskKillShellProcessParameter: BaseShellProcessParameters
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public TaskKillShellProcessParameter()
    {
        DoNotCheckExitCode = true;
    }
    /// <summary>
    /// ID of the process to kill
    /// </summary>
    public int ProcessId { get; set; }
}