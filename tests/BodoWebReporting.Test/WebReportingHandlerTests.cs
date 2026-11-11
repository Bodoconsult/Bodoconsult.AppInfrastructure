// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Runtime.Versioning;
using Bodoconsult.Database.SqlClient;
using Bodoconsult.Drawing.SkiaSharp.Factories;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using BodoWebReporting.Dal.App;
using BodoWebReporting.Dal.DataHandling;
using BodoWebReporting.Dal.Factories;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.ReportHandling;
using NUnit.Framework;

namespace BodoWebReporting.Test;

[SupportedOSPlatform("windows")]
[TestFixture]
public class WebReportingHandlerTests
{
    private readonly IBodoWebReportingAppGlobals _appGlobals = Globals.Instance;
    private readonly WebReportingHandler _wrh;

    public WebReportingHandlerTests()
    {
        ArgumentNullException.ThrowIfNull(_appGlobals.AppStartParameter.DefaultConnectionString);

        var db = new SqlClientConnManager(_appGlobals.AppStartParameter.DefaultConnectionString);

        var dataLayer = new DataHandler(_appGlobals, db);

        IBitmapServiceFactory bitmapServiceFactory = new BitmapServiceFactory();
        IWebChartExporterFactory webChartExporterFactory = new WebChartExporterFactory(dataLayer, bitmapServiceFactory);
        IWebChartHandler webChartHandler = new WebChartHandler(dataLayer, _appGlobals, webChartExporterFactory);


        IWebTableExporterFactory webTableExporterFactory = new WebTableExporterFactory(dataLayer);
        IWebTableHandler webTablesHandler = new WebTableHandler(dataLayer, webTableExporterFactory, _appGlobals);

        IWebReportExporterFactory webReportExporterFactory = new WebReportExporterFactory(dataLayer, webChartExporterFactory);
        IWebReportHandler webReportHandler = new WebReportHandler(dataLayer, webReportExporterFactory, _appGlobals);
        _wrh = new WebReportingHandler(dataLayer, webChartHandler, webTablesHandler, webReportHandler, _appGlobals);
    }

    [Test]
    public void CreateWebTables_ValidSetup_Success()
    {
        // Arrange

        // Act
        var result = _wrh.CreateWebTables();

        // Assert
        Assert.That(result > 0);
    }

    [Test]
    public void CreateWebCharts_ValidSetup_Success()
    {
        // Arrange 

        // Act  
        var result = _wrh.CreateWebCharts();

        // Assert
        Assert.That(result > 0);
    }

    [Test]
    public void CreateWebReports_ValidSetup_Success()
    {
        // Arrange 

        // Act  
        var result = _wrh.CreateWebReports();

        // Assert
        Assert.That(result > 0);
    }
}