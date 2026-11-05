// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Interfaces;

namespace BodoWebReporting.Dal.Handler;

public class WebTablesHandler: BaseObjectHandler, IWebTablesHandler
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="appGlobals">Current app globals</param>
    public WebTablesHandler(IDataHandler dataHandler, IAppGlobals appGlobals) : base(dataHandler, appGlobals)
    { }

    public int ExportTables()
    {
        var data = _dataHandler.GetWebTables(_appGlobals.TargetPath);

        var i = 0;

        foreach (var wt in data)
        {
            SetStatus($"WebTable {wt.Name}...");
            var wte = new WebTableExporter(_dataHandler, wt);
            wte.Export();
            wte.CopyToTarget();
            i++;
        }

        SetStatus("Exporting webtables is done...");

        return i;
    }
}