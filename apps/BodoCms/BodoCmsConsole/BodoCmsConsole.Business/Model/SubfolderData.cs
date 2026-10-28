// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Sub folder data
/// </summary>
public class SubfolderData
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
}