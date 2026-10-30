// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using OpenSourceConnectorBodoCms.Business.Model;

namespace OpenSourceConnectorBodoCms.Business.Interfaces;

/// <summary>
/// Interface for folder registration services
/// </summary>
public interface IRegisterFolderService
{
    /// <summary>
    /// Status message handler
    /// </summary>
    StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Source folder for open source projects
    /// </summary>
    string SourcePath { get; }

    /// <summary>
    /// Filter expression for the folder
    /// </summary>
    string FolderFilter { get; }

    /// <summary>
    /// Filter expression for the folder
    /// </summary>
    string FolderExcludeFilter { get; }

    /// <summary>
    /// Publish folder
    /// </summary>
    string TargetPath { get; }

    /// <summary>
    /// Current open source project folders
    /// </summary>
    List<FolderData> OpenSourceFolders { get; }

    /// <summary>
    /// All categories
    /// </summary>
    List<string> Categories { get; }

    /// <summary>
    /// Get all open source project folders
    /// </summary>
    void GetAllProjectFolders();

    /// <summary>
    /// Register a folder if relevante
    /// </summary>
    /// <param name="folder">Current folder</param>
    void RegisterFolder(DirectoryInfo folder);

    /// <summary>
    /// Get a clean repository URL
    /// </summary>
    /// <param name="url">Raw URL</param>
    /// <returns>Cleaned URL</returns>
    string GetRepositoryUrl(string url);
}