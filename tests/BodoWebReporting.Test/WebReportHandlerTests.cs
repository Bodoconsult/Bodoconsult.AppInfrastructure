// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Database.SqlClient;
using BodoWebReporting.Dal.App;
using BodoWebReporting.Dal.Interfaces;
using NUnit.Framework;
using System.Reflection;
using System.Runtime.Versioning;
using Bodoconsult.App.Helpers;
using Bodoconsult.Drawing.SkiaSharp.Factories;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using Bodoconsult.Pdf.Interfaces;
using BodoWebReporting.Dal.DataHandling;
using BodoWebReporting.Dal.Factories;
using BodoWebReporting.Dal.ReportHandling;

namespace BodoWebReporting.Test;

[SupportedOSPlatform("windows")]
[TestFixture]
internal class WebReportHandlerTests
{
    private readonly IBodoWebReportingAppGlobals _appGlobals = Globals.Instance;

    public WebReportHandlerTests()
    {
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        ArgumentNullException.ThrowIfNull(dir);
    }

    [Test]
    public void Run_ValidSetup_Success()
    {
        ArgumentNullException.ThrowIfNull(_appGlobals.AppStartParameter.DefaultConnectionString);

        var db = new SqlClientConnManager(_appGlobals.AppStartParameter.DefaultConnectionString);

        var dataLayer = new DataHandler(_appGlobals, db);

        IBitmapServiceFactory bitmapServiceFactory = new BitmapServiceFactory();
        var chartFactory = new WebChartExporterFactory(dataLayer, bitmapServiceFactory);
        IStyleSetFactory styleSetFactory = new BodoWebReportingStyleSetFactory();

        var factory = new WebReportExporterFactory(dataLayer, chartFactory, styleSetFactory);

        var rh = new WebReportHandler(dataLayer, factory, _appGlobals);
        rh.StartExport();
        rh.WaitForExportsDone();
        rh.CopyToTarget();
    }

    [Test]
    public void StartExportReportFile_ValidReportName_ReportCreated()
    {
        ArgumentNullException.ThrowIfNull(_appGlobals.AppStartParameter.DefaultConnectionString);

        var db = new SqlClientConnManager(_appGlobals.AppStartParameter.DefaultConnectionString);

        const string reportName = "Andrea_Gesamtbericht.pdf"; // BC_LiquiForecast.pdf Andrea_Gesamtbericht.pdf
        var targetPath = Path.Combine(_appGlobals.TargetPath, reportName);

        FileSystemHelper.DeleteFile(targetPath);

        var dataLayer = new DataHandler(_appGlobals, db);

        IBitmapServiceFactory bitmapServiceFactory = new BitmapServiceFactory();
        var chartFactory = new WebChartExporterFactory(dataLayer, bitmapServiceFactory);
        IStyleSetFactory styleSetFactory = new BodoWebReportingStyleSetFactory();

        var factory = new WebReportExporterFactory(dataLayer, chartFactory, styleSetFactory);

        var rh = new WebReportHandler(dataLayer, factory, _appGlobals);

        var rf = dataLayer.GetReportingFile(_appGlobals.TargetPath, reportName);
        ArgumentNullException.ThrowIfNull(rf);

        rh.StartExportReportFile(rf, 0);

        rh.WaitForExportsDone();

        FileSystemHelper.RunInDebugMode(targetPath);
    }
}