// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using OpenSourceConnectorBodoCms.Business.Interfaces;

namespace OpenSourceConnectorBodoCms.Business.Services;

/// <summary>
/// Top level busienss logic class based on <see cref="IOpenSourcePackagesHandler"/>
/// </summary>
public class OpenSourcePackagesHandler : IOpenSourcePackagesHandler
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="registerFolderService">Current folder registration service instance</param>
    /// <param name="exportService">Current export service</param>
    public OpenSourcePackagesHandler(IRegisterFolderService registerFolderService, IExportService exportService)
    {
        RegisterFolderService = registerFolderService;
        ExportService = exportService;
    }

    /// <summary>
    /// Current folder registration service instance
    /// </summary>
    public IRegisterFolderService RegisterFolderService { get; }

    /// <summary>
    /// Current export service
    /// </summary>
    public IExportService ExportService { get; }

    /// <summary>
    /// Register all project folders with Nuget packages
    /// </summary>
    public void GetAllProjectFolders()
    {
        RegisterFolderService.StatusMessageDelegate.Invoke("OpenSourceConnectorToBodoCms: started...");

        RegisterFolderService.StatusMessageDelegate.Invoke("OpenSourceConnectorToBodoCms: register folders");

        RegisterFolderService.GetAllProjectFolders();
    }

    /// <summary>
    /// Export the package meta data to BodoCms
    /// </summary>
    public void ExportPackageInfoToBodoCms()
    {
        RegisterFolderService.StatusMessageDelegate.Invoke("OpenSourceConnectorToBodoCms: export folder info");

        ExportService.PrepareData();
        ExportService.CreateFolders();
        ExportService.CreateHtml();
        ExportService.CreateStartpage();

        RegisterFolderService.StatusMessageDelegate.Invoke("OpenSourceConnectorToBodoCms: done!");
    }
}