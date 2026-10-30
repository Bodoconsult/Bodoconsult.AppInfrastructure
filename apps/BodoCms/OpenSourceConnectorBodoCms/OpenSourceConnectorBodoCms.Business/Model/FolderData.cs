// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace OpenSourceConnectorBodoCms.Business.Model;

/// <summary>
/// Folder data found for a open source project
/// </summary>
public class FolderData
{
    /// <summary>
    /// Name of the folder
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Full path to the folder
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Url of the solution in GitHub
    /// </summary>
    public string GitUrl { get; set; } = string.Empty;

    /// <summary>
    /// Url of the Nuget package
    /// </summary>
    public List<string> NugetUrls { get; } = new();

    /// <summary>
    /// Markdown of the readme file
    /// </summary>
    public string ReadMe { get; set; } = string.Empty;

    /// <summary>
    /// Markdown of the licence file
    /// </summary>
    public string Licence { get; set; } = string.Empty;

    /// <summary>
    /// Name of the target folder: replace . with _
    /// </summary>
    public string TargetName => Name.Replace(".", "_");

    /// <summary>
    /// List of meta data loaded from file in the project folder
    /// </summary>
    public List<OpenSourceMetaData> MetaData { get; } = new();
}