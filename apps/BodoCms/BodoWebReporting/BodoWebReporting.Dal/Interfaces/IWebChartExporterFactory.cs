// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Data;
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
    /// Current data handler
    /// </summary>
    IDataHandler DataHandler { get; }

    /// <summary>
    /// Current bitmap service factory
    /// </summary>
    IBitmapServiceFactory BitmapServiceFactory { get; }

    /// <summary>
    /// Create an instance of <see cref="WebChartExporter"/>
    /// </summary>
    /// <param name="wc">WebChart data</param>
    /// <returns>Instance of <see cref="WebChartExporter"/></returns>
    WebChartExporter CreateInstance(WebChart wc);

    /// <summary>
    /// Create an instance of <see cref="WebChartExporter"/>
    /// </summary>
    /// <param name="dt">DataTable with chart data</param>
    /// <param name="wc">WebChart data</param>
    /// <returns>Instance of <see cref="WebChartExporter"/></returns>
    WebChartExporter CreateInstance(DataTable dt, WebChart wc);
}