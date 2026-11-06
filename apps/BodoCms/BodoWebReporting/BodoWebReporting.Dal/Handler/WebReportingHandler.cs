// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Reflection;
using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Handler;

[SupportedOSPlatform("windows")]
public class WebReportingHandler : IWebReportingHandler
{
    private readonly IWebChartHandler _webChartHandler;
    private readonly IWebTableHandler _webTablesHandler;
    private readonly IWebReportHandler _webReportHandler;
    private readonly IDataHandler _dataHandler;
    private readonly IBodoWebReportingAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    public WebReportingHandler(IDataHandler dataHandler, IWebChartHandler webChartHandler,  IWebTableHandler webTablesHandler, IWebReportHandler webReportHandler, IAppGlobals appGlobals)
    {
        _dataHandler = dataHandler;
        _webChartHandler = webChartHandler;
        _webTablesHandler = webTablesHandler;
        _webReportHandler = webReportHandler;

        if (appGlobals is not IBodoWebReportingAppGlobals globals)
        {
            throw new ArgumentException("appGlobals is not IBodoWebReportingAppGlobals");
        }

        _appGlobals = globals;

        ArgumentNullException.ThrowIfNull(appGlobals.AppStartParameter.AppPath); 
        CurrentDir = appGlobals.AppStartParameter.AppPath;
        var file = Path.Combine(CurrentDir, @"Template\StyleImport.cs");

        // Styles laden
        StyleHandler.FileName = file;
        StyleHandler.Start();
        _webReportHandler = webReportHandler;
    }

    /// <summary>
    /// Current directory
    /// </summary>
    public string CurrentDir { get; }

    /// <summary>
    /// Check the target directory
    /// </summary>
    public void CheckTargetPath()
    {
        _appGlobals.StatusMessageDelegate?.Invoke("Check target directory...");

        if (!Directory.Exists(_appGlobals.TargetPath))
        {
            Directory.CreateDirectory(_appGlobals.TargetPath);
        }
    }

    /// <summary>
    /// Create web tables
    /// </summary>
    /// <returns>O on success else error code</returns>
    public int CreateWebTables()
    {
        _appGlobals.StatusMessageDelegate?.Invoke("Create WebTables...");
        var  result = _webTablesHandler.ExportTables();
        return result;
    }

    /// <summary>
    /// Create web charts
    /// </summary>
    public int CreateWebCharts()
    {
        _appGlobals.StatusMessageDelegate?.Invoke("Create WebCharts...");
        return _webChartHandler.ExportCharts();
    }

    /// <summary>
    /// Create web reports
    /// </summary>
    /// <returns>O on success else error code</returns>
    public int CreateWebReports()
    {
        _appGlobals.StatusMessageDelegate?.Invoke("Create WebReports...");
        _webReportHandler.Target = ReportingTargets.PdfFile;
        var result = _webReportHandler.StartExport();
        _webReportHandler.MoveToTarget();
        return result;
    }

    /// <summary>
    /// Run the post run SQL statement
    /// </summary>
    public void RunPostRunSql()
    {
        if (string.IsNullOrEmpty(_appGlobals.PostRunSql))
        {
            return;
        }

        _appGlobals.StatusMessageDelegate?.Invoke("Execute post run app SQL...");
        try
        {
            _dataHandler.Exec(_appGlobals.PostRunSql);
        }
        catch
        {
            // Do nothing
        }
    }

    /// <summary>
    /// Run the pre run SQL statement
    /// </summary>
    public void RunPreRunSql()
    {
        if (string.IsNullOrEmpty(_appGlobals.PreRunSql))
        {
            return;
        }

        _appGlobals.StatusMessageDelegate?.Invoke("Execute pre run app SQL...");
        try
        {
            _dataHandler.Exec(_appGlobals.PreRunSql);
        }
        catch
        {
            // Do nothing
        }
    }
}