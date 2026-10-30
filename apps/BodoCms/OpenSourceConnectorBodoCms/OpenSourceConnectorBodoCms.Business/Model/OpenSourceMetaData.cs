// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace OpenSourceConnectorBodoCms.Business.Model;

/// <summary>
/// Open source meta data to place in the root folder of the project
/// </summary>
public class OpenSourceMetaData
{
    /// <summary>
    /// Name of the Nuget package
    /// </summary>
    public string NugetPackage { get; set; } = string.Empty;

    /// <summary>
    /// Package category
    /// </summary>
    public string Category { get; set; } = "General";
}