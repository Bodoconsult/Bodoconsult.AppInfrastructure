// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace JobExecuter.Dal.Model;

/// <summary>
/// Folder information for folder compliance reporting
/// </summary>
/// <remarks>Show all permissions on file system folders for users and groups.</remarks>
public class ComplianceFolder
{
    /// <summary>
    /// Full path to the folder
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Include the direct subfolders of the folder
    /// </summary>
    public bool IncludeSubFolders { get; set; }

    /// <summary>
    /// Show the permissions of predefined Windows accounts. Default: false
    /// </summary>
    public bool ShowPermissionsOfPredefinedAccounts { get; set; }
}