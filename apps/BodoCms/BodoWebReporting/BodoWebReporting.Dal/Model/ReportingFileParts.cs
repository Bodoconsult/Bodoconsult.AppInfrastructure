// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Charting.Base.Interfaces;

namespace BodoWebReporting.Dal.Model;

public class ReportingFileParts
{
    /// <summary>
    /// Landscape orientation of the chart
    /// </summary>
    public bool Landscape{ get; set; }

    /// <summary>
    /// Path to the background image
    /// </summary>
    public string? BackgroundImagePath { get; set; }

    /// <summary>
    /// Path to the logo
    /// </summary>
    public string? LogoPath { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Ttile 1
    /// </summary>
    public string? Title1 { get; set; }

    /// <summary>
    /// Subject
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// SQL to run to get the data
    /// </summary>
    public string? Sql { get; set; }

    /// <summary>
    /// SQL to get additional info
    /// </summary>
    public string? AddInfoSql { get; set; }

    /// <summary>
    /// Report type
    /// </summary>
    public string? ReportType { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Show the part in the table of content
    /// </summary>
    public bool ShowInToc { get; set; }

    /// <summary>
    /// Semicolon separated list of columns to use for chart or null
    /// </summary>
    public string? ChartColumns { get; set; }

    /// <summary>
    /// Semicolon separated list of columns to use for data table or null
    /// </summary>
    public string? ChartColumnsDataTable { get; set; }

    /// <summary>
    /// Y unit name
    /// </summary>
    public string? ChartYUnit { get; set; }

    /// <summary>
    /// Length of the chart legend
    /// </summary>
    public short ChartLegendLength { get; set; }

    /// <summary>
    /// Chart type
    /// </summary>
    public ChartType ChartType { get; set; }
}