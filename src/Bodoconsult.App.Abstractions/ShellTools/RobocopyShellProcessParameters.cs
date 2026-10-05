// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.App.Abstractions.ShellTools;

/// <summary>
/// Parameter set to run robocopy
/// </summary>
public class RobocopyShellProcessParameters : BaseShellProcessParameters
{
    /// <summary>
    /// Arguments string added to the robocopy command
    /// </summary>
    public string? Args { get; set; }
}