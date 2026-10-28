// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// Folder data for creating a webpage from the folder
/// </summary>
public class FolderData
{
    /// <summary>
    /// Full name of the folder
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Full path of the folder
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the folder
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Folder is a subdomain on webserver
    /// </summary>
    public bool Subdomain { get; set; }

    /// <summary>
    /// Remove leading numbers in the header
    /// </summary>
    public bool HeaderRemoveNumbers { get; set; }

    /// <summary>
    /// Add to sitemap
    /// </summary>
    public bool Sitemap { get; set; }

    /// <summary>
    /// Create a summary
    /// </summary>
    public bool Summary { get; set; }

    /// <summary>
    /// Keywords for the folder
    /// </summary>
    public string Keywords { get; set; } = string.Empty;

    /// <summary>
    /// Description for the folder
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// ID of the advertising set to use for the folder
    /// </summary>
    public int AdSetId { get; set; }

    /// <summary>
    /// Path to a header image for the folder
    /// </summary>
    public string HeaderImage { get; set; } = string.Empty;

    /// <summary>
    /// Web link to a header image for the folder
    /// </summary>
    public string HeaderImageLink { get; set; } = string.Empty;
}