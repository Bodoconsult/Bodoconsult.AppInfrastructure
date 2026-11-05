// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Charting.Base.Interfaces;

namespace BodoWebReporting.Dal.Model;

/// <summary>
/// Webchart representing a chart exported as image to the filesystem
/// </summary>
public class WebChart
{
    /// <summary>
    /// Filename
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// SQL to run to get data for chart
    /// </summary>
    public string? Sql { get; set; }

    /// <summary>
    /// Y unit name
    /// </summary>
    public string? YUnit { get; set; }

    /// <summary>
    /// Semicolon separated list of columns to use for chart or null
    /// </summary>
    public string? Columns { get; set; }

    /// <summary>
    /// Semicolon separated list of columns to use for data table or null
    /// </summary>
    public string? ColumnsDataTable { get; set; }

    /// <summary>
    /// Chart type
    /// </summary>
    public ChartType ChartType { get; set; }

    /// <summary>
    /// Database ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Width in pixels
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Height in pixels
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Export the datatable too?
    /// </summary>
    public bool ExportDataTable { get; set; }

    /// <summary>
    /// Copyright to print on chart
    /// </summary>
    public string? Copyright { get; set; }

    /// <summary>
    /// Copy tasks
    /// </summary>
    public List<CopyTask> CopyTasks { get; } = new();
}