// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Interfaces;

namespace BodoWebReporting.Dal.ReportHandling;

/// <summary>
/// Current implementation of <see cref="IWebTableHandler"/>
/// </summary>
public class WebTableHandler: BaseObjectHandler, IWebTableHandler
{
    private readonly IWebTableExporterFactory _webTableExporterFactory;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="webTableExporterFactory">Current WebTableExporter factory</param>
    /// <param name="appGlobals">Current app globals</param>
    public WebTableHandler(IDataHandler dataHandler, IWebTableExporterFactory webTableExporterFactory,
        IAppGlobals appGlobals) : base(dataHandler, appGlobals)
    {
        _webTableExporterFactory = webTableExporterFactory;
    }

    /// <summary>
    /// Export the tables
    /// </summary>
    /// <returns>Number of exported tables</returns>
    public int ExportTables()
    {
        var data = DataHandler.GetWebTables(AppGlobals.TargetPath);

        var i = 0;

        foreach (var wt in data)
        {
            SetStatus($"WebTable {wt.Name}...");
            var wte = _webTableExporterFactory.CreateInstance(wt);
            wte.Export();
            wte.CopyToTarget();
            i++;
        }

        SetStatus("Exporting webtables is done...");

        return i;
    }
}