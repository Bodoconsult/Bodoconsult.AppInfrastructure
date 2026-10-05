// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.App.Abstractions.ShellTools;

/// <summary>
/// Parameter set to use for moving a directory
/// </summary>
public class MoveDirectoryShellProcessParameters : BaseShellProcessParameters
{
    /// <summary>
    /// Full path of the source directory to move from
    /// </summary>
    public string? SourcePath { get; set; }

    /// <summary>
    /// Full path of the target directory to move to
    /// </summary>
    public string? TargetPath { get; set; }
}