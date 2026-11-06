// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Database.SqlClient;
using BodoWebReporting.Dal.App;
using BodoWebReporting.Dal.Handler;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;
using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using Bodoconsult.Drawing.SkiaSharp.Factories;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using BodoWebReporting.Dal.Factories;

namespace BodoWebReporting.Test;

[SupportedOSPlatform("windows")]
[TestFixture]
internal class WebReportHandlerTests
{
    private readonly IBodoWebReportingAppGlobals _appGlobals = Globals.Instance;

    public WebReportHandlerTests()
    {
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (dir == null) return;
        var file = Path.Combine(dir, @"Template\StyleImport.cs");

        // Styles laden
        StyleHandler.FileName = file;
        StyleHandler.Start();

        StyleHandler.RunCode();
    }

    [Test]
    public void Run_ValidSetup_Success()
    {
        ArgumentNullException.ThrowIfNull(_appGlobals.AppStartParameter.DefaultConnectionString);

        var db = new SqlClientConnManager(_appGlobals.AppStartParameter.DefaultConnectionString);

        var dataLayer = new DataHandler(_appGlobals, db);

        IBitmapServiceFactory bitmapServiceFactory = new BitmapServiceFactory();
        var chartFactory = new WebChartExporterFactory(dataLayer, bitmapServiceFactory);

        var factory = new WebReportExporterFactory(dataLayer, chartFactory);

        var rh = new WebReportHandler(dataLayer, factory, _appGlobals);
        rh.StartExport();
        rh.MoveToTarget();
    }
}