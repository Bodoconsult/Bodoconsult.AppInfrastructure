// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.General;
using Bodoconsult.App.Zip;
using IGeneralAppManagementService = Bodoconsult.App.Interfaces.IGeneralAppManagementService;

namespace Bodoconsult.App;

/// <summary>
/// Basic implementation of general app services like database backup
/// </summary>
public class GeneralAppManagementService : IGeneralAppManagementService
{
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    public GeneralAppManagementService(IAppGlobals appGlobals)
    {
        _appGlobals = appGlobals;
    }

    /// <summary>
    /// Create a dump file with all logs in ProgrammData\SmdTower
    /// </summary>
    public void CreateLogDump()
    {

        // Create log dump now
        var path = _appGlobals.AppStartParameter.DataPath;

        if (path is null)
        {
            return;
        }

        var zip = Path.Combine(path, "LogDump.zip");

        const string extensions = "*.log,*.lst,*.txt";

        ZipHelper.CreateZipArchive(zip, path, extensions);
    }

    /// <summary>
    /// Create a normal mini dump of the app
    /// </summary>
    public void CreateMiniDump()
    {
        var path = _appGlobals.AppStartParameter.DataPath;

        if (path is null)
        {
            return;
        }

        var miniDumpPath = Path.Combine(path, "MiniDump.dmp");

        MiniDump.MiniDumpToFile(miniDumpPath);
    }

    /// <summary>
    /// Create a normal mini dump of the app
    /// </summary>
    public void CreateMiniDump(MiniDump.MiniDumpTypeEnum dumpType)
    {
        var path = _appGlobals.AppStartParameter.DataPath;

        if (path is null)
        {
            return;
        }

        var miniDumpPath = Path.Combine(path, "MiniDump.dmp");

        MiniDump.MiniDumpToFile(miniDumpPath, dumpType);
    }
}