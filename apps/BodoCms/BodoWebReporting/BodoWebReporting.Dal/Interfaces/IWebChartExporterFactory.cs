// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Interfaces;

/// <summary>
/// Interface for factories to create <see cref="WebChartExporter"/> instances
/// </summary>
public interface IWebChartExporterFactory
{
    /// <summary>
    /// Current datahandler
    /// </summary>
    IDataHandler DataHandler {get;} 
        
    /// <summary>
    /// Current bitmap service factory
    /// </summary>
        IBitmapServiceFactory BitmapServiceFactory { get; }

    /// <summary>
    /// Create an instance of <see cref="WebChartExporter"/>
    /// </summary>
    /// <param name="wc">WebChart data</param>
    /// <returns>Instance of <see cref="WebChartExporter"/></returns>
    WebChartExporter CreateInstance( WebChart wc);
}