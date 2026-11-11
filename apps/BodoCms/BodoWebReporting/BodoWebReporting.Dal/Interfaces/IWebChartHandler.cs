// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Interfaces;

/// <summary>
/// Interface for the creation process handler of <see cref="WebChart"/> instances
/// </summary>
public interface IWebChartHandler
{
    /// <summary>
    /// Export the charts
    /// </summary>
    /// <returns>Number of exported charts</returns>
    int ExportCharts();
}