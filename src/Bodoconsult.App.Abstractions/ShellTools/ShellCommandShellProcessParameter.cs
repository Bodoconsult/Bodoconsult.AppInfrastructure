// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.App.Abstractions.ShellTools;

/// <summary>
/// Parameters for running a shell command like RD, MOVE etc.
/// </summary>
public class ShellCommandShellProcessParameter: BaseShellProcessParameters
{
    /// <summary>
    /// Shell command to run
    /// </summary>
    public string? Command { get; set; }
}