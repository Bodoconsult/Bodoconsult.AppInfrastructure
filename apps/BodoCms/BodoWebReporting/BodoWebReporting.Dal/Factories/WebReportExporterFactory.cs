// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.Interfaces;
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
    /// <param name="styleSetFactory">Current styleset factory</param>
    public WebReportExporterFactory(IDataHandler dataHandler, IWebChartExporterFactory webChartExporterFactory, IStyleSetFactory styleSetFactory)
    {
        DataHandler = dataHandler;
        WebChartExporterFactory = webChartExporterFactory;
        StyleSetFactory = styleSetFactory;
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
    /// Current styleset factory
    /// </summary>
    public IStyleSetFactory StyleSetFactory { get; }

    /// <summary>
    /// Create an instance of <see cref="WebReportExporter"/>
    /// </summary>
    /// <param name="file">Reporting file data</param>
    /// <param name="id">Step ID. Default: 1</param>
    /// <returns>Instance of <see cref="WebReportExporter"/></returns>
    public WebReportExporter CreateInstance(ReportingFile file, int id = 1)
    {
        return new WebReportExporter(DataHandler, WebChartExporterFactory, StyleSetFactory, file, id);
    }
}