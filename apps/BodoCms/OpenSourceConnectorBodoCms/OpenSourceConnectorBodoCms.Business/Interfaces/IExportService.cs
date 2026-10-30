// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using OpenSourceConnectorBodoCms.Business.Model;

namespace OpenSourceConnectorBodoCms.Business.Interfaces;

/// <summary>
/// Metadata export service to BodoCms
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Folders to export
    /// </summary>
    IList<FolderData> Folders { get; }

    /// <summary>
    /// Status message delelgate
    /// </summary>
    StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Prepare the data for the export
    /// </summary>
    void PrepareData();

    /// <summary>
    /// Create folders
    /// </summary>
    void CreateFolders();

    /// <summary>
    /// Create HTML
    /// </summary>
    void CreateHtml();

    /// <summary>
    /// Create start page
    /// </summary>
    void CreateStartpage();
}