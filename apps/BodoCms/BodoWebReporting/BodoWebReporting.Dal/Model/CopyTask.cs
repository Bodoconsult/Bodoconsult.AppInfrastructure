// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace BodoWebReporting.Dal.Model;

/// <summary>
/// Data required for a filecopy task
/// </summary>
public class CopyTask
{
    /// <summary>
    /// Source filename
    /// </summary>
    public string SourceFileName { get; set; } = string.Empty;

    /// <summary>
    /// Target filename
    /// </summary>
    public string TargetFileName { get; set; } = string.Empty;

    /// <summary>
    /// Include source filename
    /// </summary>
    public string IncludeSourceFileName { get; set; } = string.Empty;

    /// <summary>
    /// Include target filename
    /// </summary>
    public string IncludeTargetFileName { get; set; } = string.Empty;
}