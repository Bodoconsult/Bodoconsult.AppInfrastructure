// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// File data for subfolder files
/// </summary>
public class FileData
{
    /// <summary>
    /// Full folder path
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Folder name in the OS
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Name of the folder to display
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// File extension
    /// </summary>
    public string Extension { get; set; } = string.Empty;
}