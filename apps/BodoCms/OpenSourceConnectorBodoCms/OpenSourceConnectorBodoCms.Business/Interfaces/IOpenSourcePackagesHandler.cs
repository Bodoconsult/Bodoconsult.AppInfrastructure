// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace OpenSourceConnectorBodoCms.Business.Interfaces;

/// <summary>
/// Interface for the top level business logic class of the app
/// </summary>
public interface IOpenSourcePackagesHandler
{
    /// <summary>
    /// Current folder registration service instance
    /// </summary>
    IRegisterFolderService RegisterFolderService { get; }

    /// <summary>
    /// Current export service
    /// </summary>
    IExportService ExportService { get; }

    /// <summary>
    /// Register all project folders with Nuget packages
    /// </summary>
    void GetAllProjectFolders();

    /// <summary>
    /// Export the package meta data to BodoCms
    /// </summary>
    void ExportPackageInfoToBodoCms();

}