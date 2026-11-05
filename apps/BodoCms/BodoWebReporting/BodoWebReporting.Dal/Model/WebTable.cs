// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace BodoWebReporting.Dal.Model;

/// <summary>
/// WebTable representing an HTML table code segement to include in a webpage
/// </summary>
public class WebTable
{
    /// <summary>
    /// Name of the table
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Database ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Table type
    /// </summary>
    public string? TableType { get; set; }

    /// <summary>
    /// SQL to run to get data for chart
    /// </summary>
    public string? Sql { get; set; }

    /// <summary>
    /// Filename
    /// </summary>
    public string? FileName { get; set; }
}