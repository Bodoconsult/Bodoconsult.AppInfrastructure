// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.App.Abstractions.ShellTools;

/// <summary>
/// Parameter set to use for removing a directory
/// </summary>
public class RemoveDirectoryShellProcessParameters : BaseShellProcessParameters
{
    /// <summary>
    /// Full path of the directory to remove
    /// </summary>
    public string? Path { get; set; }
}