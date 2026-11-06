// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Interfaces;

/// <summary>
/// Interface for factories to create <see cref="WebReportExporter"/> instances
/// </summary>
public interface IWebReportExporterFactory
{
    /// <summary>
    /// Current data handler
    /// </summary>
    IDataHandler DataHandler { get; }

    /// <summary>
    /// Current WebChart export factory
    /// </summary>
    IWebChartExporterFactory WebChartExporterFactory { get; }

    /// <summary>
    /// Create an instance of <see cref="WebReportExporter"/>
    /// </summary>
    /// <param name="file">Reporting file data</param>
    /// <param name="id">Step ID. Default: 1</param>
    /// <returns>Instance of <see cref="WebReportExporter"/></returns>
    WebReportExporter CreateInstance(ReportingFile file, int id = 1);
}