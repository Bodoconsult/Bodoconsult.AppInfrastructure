// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Reflection;
using System.Runtime.Versioning;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Handler;

[SupportedOSPlatform("windows")]
public class WebReportingHandler
{

    public DataHandler DataLayer { get; set; }

    public string CurrentDir { get; set; }
    public StatusHandler StatusChange { get; set; }
    public string PreRunSql { get; set; }
    public string TargetPath { get; set; }


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataLayer"></param>
    public WebReportingHandler(DataHandler dataLayer)
    {

        DataLayer = dataLayer;

        CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (CurrentDir == null)
        {
            return;
        }
        var file = Path.Combine(CurrentDir, @"Template\StyleImport.cs");

        // Styles laden
        StyleHandler.FileName = file;
        StyleHandler.Start();
    }

    /// <summary>
    /// Create web tables
    /// </summary>
    public int CreateWebTables()
    {
        var wt = new WebTablesHandler(DataLayer)
        {
            TargetPath = TargetPath,
            PreRunSql = PreRunSql,
            StatusChange = StatusChange,
        };
        wt.OpenDatabase();
        return wt.ExportTables();
    }

    /// <summary>
    /// Create web charts
    /// </summary>
    public int CreateWebCharts()
    {
        // Webcharts
        var wc = new WebChartHandler(DataLayer)
        {
            TargetPath = TargetPath,
            StatusChange = StatusChange,
        };

        wc.OpenDatabase();
        return wc.ExportCharts();
    }


    public int CreateWebReports()
    {

           

        var rh = new WebReportHandler(DataLayer)
        {
            TargetPath = TargetPath,
            Target = ReportingTargets.PdfFile,
            StatusChange = StatusChange,
        };

        rh.OpenDatabase();
        var result = rh.StartExport();
        rh.MoveToTarget();

        return result;
    }
}