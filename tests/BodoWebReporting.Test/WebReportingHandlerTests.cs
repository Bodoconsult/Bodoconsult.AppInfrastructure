// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Database.SqlClient;
using Bodoconsult.Drawing.SkiaSharp.Factories;
using BodoWebReporting.Dal;
using BodoWebReporting.Dal.App;
using BodoWebReporting.Dal.Factories;
using BodoWebReporting.Dal.Handler;
using BodoWebReporting.Dal.Interfaces;
using NUnit.Framework;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace BodoWebReporting.Test;

[SupportedOSPlatform("windows")]
[TestFixture]
public class WebReportingHandlerTests
{
    private readonly IBodoWebReportingAppGlobals _appGlobals = Globals.Instance;
    private readonly WebReportingHandler _wrh;

    private readonly BitmapServiceFactory _bsf = new BitmapServiceFactory();

    public WebReportingHandlerTests()
    {
        ArgumentNullException.ThrowIfNull(_appGlobals.AppStartParameter.DefaultConnectionString);

        var db = new SqlClientConnManager(_appGlobals.AppStartParameter.DefaultConnectionString);

        var dataLayer = new DataHandler(_appGlobals, db);

        IWebChartExporterFactory webChartExporterFactory = new WebChartExporterFactory(dataLayer, _bsf);
        IWebChartHandler webChartHandler = new WebChartHandler(dataLayer, _appGlobals, webChartExporterFactory);

        IWebTablesHandler webTablesHandler = new WebTablesHandler(dataLayer, _appGlobals); ;
        IWebReportHandler webReportHandler = new WebReportHandler(dataLayer, _appGlobals); ;
        _wrh = new WebReportingHandler(dataLayer, webChartHandler, webTablesHandler, webReportHandler, _appGlobals);
    }

    [Test]
    public void TestCreateWebTables_Success()
    {   
        var result = _wrh.CreateWebTables();
        Assert.That(result>0);
    }

    [Test]
    public void TestCreateWebCharts_Success()
    {
        // Webcharts
        var result = _wrh.CreateWebCharts();
        Assert.That(result > 0);
    }

    [Test]
    public void TestCreateWebReports_Success()
    {
        // Web reports
        var result = _wrh.CreateWebReports();
        Assert.That(result > 0);
    }
}