// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Factories;

/// <summary>
/// Current implementation of <see cref="IWebTableExporterFactory"/>
/// </summary>
public class WebTableExporterFactory : IWebTableExporterFactory
{
    /// <summary>
    /// Defaul ctor
    /// </summary>
    /// <param name="dataHandler">Current datahandler</param>
    public WebTableExporterFactory(IDataHandler dataHandler)
    {
        DataHandler = dataHandler;
    }

    /// <summary>
    /// Current datahandler
    /// </summary>
    public IDataHandler DataHandler { get; }

    /// <summary>
    /// Create an instance of <see cref="WebTableExporter"/>
    /// </summary>
    /// <param name="wt">WebTable data</param>
    /// <returns>Instance of <see cref="WebTableExporter"/></returns>
    public WebTableExporter CreateInstance(WebTable wt)
    {
        return new WebTableExporter(DataHandler, wt);
    }
}