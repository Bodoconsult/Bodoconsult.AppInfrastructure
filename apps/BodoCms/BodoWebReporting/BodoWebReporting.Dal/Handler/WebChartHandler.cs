// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Interfaces;

namespace BodoWebReporting.Dal.Handler;

[SupportedOSPlatform("windows")]
public class WebChartHandler: BaseObjectHandler, IWebChartHandler
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="appGlobals">Current app globals</param>
    public WebChartHandler(IDataHandler dataHandler, IAppGlobals appGlobals): base(dataHandler, appGlobals)
    { }

    public int ExportCharts()
    {
        var i = 0;
        var data = _dataHandler.GetWebCharts(_appGlobals.TargetPath);

        foreach (var wc in data)
        {
            SetStatus($"Exportiere Chart {wc.Title}");
            try
            {
                var wce = new WebChartExporter(_dataHandler, wc);
                wce.Export();
                wce.ExportDataTable();
                wce.CopyToTarget();

                i++;
            }
            catch (Exception ex)
            {
                _dataHandler.ErrorHandling($"BodoWebReporting:WebChartHandler:ExportCharts:ID{wc.Id}",
                    $"{ex.Message}");
            }
        }

        SetStatus("Exporting charts is done...");
        return i;
    }

    /// <summary>
    /// Statusanzeige
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    private void SetStatus(string msg)
    {
        StatusMessageDelegate.Invoke(msg);
    }
}