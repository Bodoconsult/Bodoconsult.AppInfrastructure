// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Exporter;

namespace BodoWebReporting.Dal.Report;

[SupportedOSPlatform("windows")]
internal class ChartReportItem : BaseReportItem
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public ChartReportItem()
    {
        TypeOfObject = ReportItemType.Chart;
    }

    /// <summary>
    /// Data
    /// </summary>
    public WebChartExporter? Data { get; set; }

    /// <summary>
    /// Export the repor chart
    /// </summary>
    public void Export()
    {
        ArgumentNullException.ThrowIfNull(Data);

        Data.Typography.ChartStyle.PaperColor = TypoColors.Transparent;
        Data.Export();
    }

}