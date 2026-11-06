// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using BodoWebReporting.Dal.Exporter;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Interfaces;

/// <summary>
/// Interface for factories to create <see cref="WebTableExporter"/> instances
/// </summary>
public interface IWebTableExporterFactory
{
    /// <summary>
    /// Current data handler
    /// </summary>
    IDataHandler DataHandler { get; }


    /// <summary>
    /// Create an instance of <see cref="WebTableExporter"/>
    /// </summary>
    /// <param name="wc">WebChart data</param>
    /// <returns>Instance of <see cref="WebTableExporter"/></returns>
    WebTableExporter CreateInstance(WebTable wc);
}