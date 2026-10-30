// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using OpenSourceConnectorBodoCms.Business.Model;

namespace OpenSourceConnectorBodoCms.Business.Interfaces;

/// <summary>
/// OpenSourceConnectorBodoCms specific enhancements for <see cref="IAppGlobals"/>
/// </summary>
public interface IOpenSourceConnectorBodoCmsAppGlobals: IAppGlobals
{
    /// <summary>
    /// Source folder for open source projects
    /// </summary>
    string SourcePath { get; set; }

    /// <summary>
    /// Filter expression for the folder
    /// </summary>
    string FolderFilter { get; set; }

    /// <summary>
    /// Filter expression for excluding folders
    /// </summary>
    string FolderExcludeFilter { get; set; }

    /// <summary>
    /// Publish folder
    /// </summary>
    string TargetPath { get; set; }

    /// <summary>
    /// Current open source project folders
    /// </summary>
    List<FolderData> OpenSourceFolders { get; }

    /// <summary>
    /// All categories
    /// </summary>
    List<string> Categories { get; }
}