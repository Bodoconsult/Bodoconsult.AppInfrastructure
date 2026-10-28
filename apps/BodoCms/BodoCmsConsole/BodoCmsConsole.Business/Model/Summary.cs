// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Text;

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Contains the summary for a file system folder
/// </summary>
public class Summary
{
    /// <summary>
    /// Folder path the summary is for
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// HTML content of the summary
    /// </summary>
    public StringBuilder Content { get; } = new();

    /// <summary>
    /// Counter
    /// </summary>
    public int Counter;

    /// <summary>
    /// Add the summaries of the folder's subfolders
    /// </summary>
    public bool IncludeSummariesForSubfolders;

    /// <summary>
    /// Title for the summary
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// File name for the summary file
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Relative url for the summary to css and images
    /// </summary>
    public string Relative { get; set; } = string.Empty;
}