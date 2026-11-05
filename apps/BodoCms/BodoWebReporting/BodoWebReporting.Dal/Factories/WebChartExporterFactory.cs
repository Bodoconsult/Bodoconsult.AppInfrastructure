// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Factories;

/// <summary>
/// Current implementation of <see cref="IWebChartExporterFactory"/>
/// </summary>
public class WebChartExporterFactory : IWebChartExporterFactory
{
    /// <summary>
    /// Defaul ctor
    /// </summary>
    /// <param name="dataHandler">Current datahandler</param>
    /// <param name="bitmapServiceFactory">Current bitmap service factory</param>
    public WebChartExporterFactory(IDataHandler dataHandler, IBitmapServiceFactory bitmapServiceFactory)
    {
        DataHandler = dataHandler;
        BitmapServiceFactory = bitmapServiceFactory;
    }

    /// <summary>
    /// Current datahandler
    /// </summary>
    public IDataHandler DataHandler { get; }

    /// <summary>
    /// Current bitmap service factory
    /// </summary>
    public IBitmapServiceFactory BitmapServiceFactory { get; }

    /// <summary>
    /// Create an instance of <see cref="WebChartExporter"/>
    /// </summary>
    /// <param name="wc">WebChart data</param>
    /// <returns>Instance of <see cref="WebChartExporter"/></returns>
    public WebChartExporter CreateInstance(WebChart wc)
    {
        return new WebChartExporter(DataHandler, BitmapServiceFactory, wc);
    }
}