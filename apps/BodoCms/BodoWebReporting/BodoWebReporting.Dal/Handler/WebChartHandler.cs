// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Runtime.Versioning;
using BodoWebReporting.Dal.Exporter;

namespace BodoWebReporting.Dal.Handler;

[SupportedOSPlatform("windows")]
public class WebChartHandler
{
    private readonly DataHandler _dataHandler;

    public WebChartHandler(DataHandler dataHandler)
    {
        _dataHandler = dataHandler;
    }

    public StatusHandler StatusChange;


    public string TargetPath; // { get; set; }

    public int ExportCharts()
    {
        var i = 0;
        var data = _dataHandler.GetWebCharts(TargetPath);

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


    //public string ErrorMailer; // { get; set; }


    public void OpenDatabase()
    {
        SetStatus("Check target directory...");

        if (!Directory.Exists(TargetPath))
        {
            Directory.CreateDirectory(TargetPath);
        }
    }

    /// <summary>
    /// Statusanzeige
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    private void SetStatus(string msg)
    {
        var x = StatusChange;
        x?.Invoke(msg);
    }
}