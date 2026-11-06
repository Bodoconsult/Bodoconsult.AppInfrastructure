// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Factories;

/// <summary>
/// Current implementation of <see cref="IWebTableExporterFactory"/>
/// </summary>
public class WebReportExporterFactory : IWebReportExporterFactory
{
    /// <summary>
    /// Defaul ctor
    /// </summary>
    /// <param name="dataHandler">Current datahandler</param>
    /// <param name="webChartExporterFactory">Current WebChart exporter factory</param>
    public WebReportExporterFactory(IDataHandler dataHandler, IWebChartExporterFactory webChartExporterFactory)
    {
        DataHandler = dataHandler;
        WebChartExporterFactory = webChartExporterFactory;
    }

    /// <summary>
    /// Current datahandler
    /// </summary>
    public IDataHandler DataHandler { get; }

    /// <summary>
    /// Current WebChart export factory
    /// </summary>
    public IWebChartExporterFactory WebChartExporterFactory { get; }

    /// <summary>
    /// Create an instance of <see cref="WebReportExporter"/>
    /// </summary>
    /// <param name="file">Reporting file data</param>
    /// <param name="id">Step ID. Default: 1</param>
    /// <returns>Instance of <see cref="WebReportExporter"/></returns>
    public WebReportExporter CreateInstance(ReportingFile file, int id = 1)
    {
        return new WebReportExporter(DataHandler, WebChartExporterFactory, file, id);
    }
}