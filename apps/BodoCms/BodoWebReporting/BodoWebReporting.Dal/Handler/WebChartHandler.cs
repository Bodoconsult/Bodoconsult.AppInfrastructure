// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Interfaces;

namespace BodoWebReporting.Dal.Handler;

[SupportedOSPlatform("windows")]
public class WebChartHandler: BaseObjectHandler, IWebChartHandler
{
    private readonly IWebChartExporterFactory _webChartExporterFactory;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="appGlobals">Current app globals</param>
    /// <param name="webChartExporterFactory">Current WebChartExporter factory</param>
    public WebChartHandler(IDataHandler dataHandler, IAppGlobals appGlobals,
        IWebChartExporterFactory webChartExporterFactory) : base(dataHandler, appGlobals)
    {
        _webChartExporterFactory = webChartExporterFactory;
    }

    public int ExportCharts()
    {
        var i = 0;
        var data = DataHandler.GetWebCharts(AppGlobals.TargetPath);

        foreach (var wc in data)
        {
            SetStatus($"Exportiere Chart {wc.Title}");
            try
            {
                var wce = _webChartExporterFactory.CreateInstance(wc);
                wce.Export();
                wce.ExportDataTable();
                wce.CopyToTarget();

                i++;
            }
            catch (Exception ex)
            {
                DataHandler.ErrorHandling($"BodoWebReporting:WebChartHandler:ExportCharts:ID{wc.Id}",
                    $"{ex.Message}");
            }
        }

        SetStatus("Exporting charts is done...");
        return i;
    }
}