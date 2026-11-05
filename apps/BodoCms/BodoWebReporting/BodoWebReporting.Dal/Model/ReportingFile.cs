// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace BodoWebReporting.Dal.Model;

/// <summary>
/// Basic data for a report
/// </summary>
public class ReportingFile
{
    /// <summary>
    /// Filename
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    public string Author { get; set; } = "Author";

    /// <summary>
    /// Report shortname
    /// </summary>
    public string? Shortname { get; set; }

    /// <summary>
    /// Reporting parts
    /// </summary>
    public List<ReportingFileParts> ReportingFileParts { get; } = new();

    /// <summary>
    /// Copy tasks
    /// </summary>
    public List<CopyTask> CopyTasks { get; set; } = new();

    /// <summary>
    /// Path to the logo
    /// </summary>
    public string? LogoPath { get; set; }
}