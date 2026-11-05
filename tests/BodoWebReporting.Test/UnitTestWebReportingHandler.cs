// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Runtime.Versioning;
using BodoWebReporting.Dal;
using BodoWebReporting.Dal.Handler;
using NUnit.Framework;

namespace BodoWebReporting.Test;

[SupportedOSPlatform("windows")]
[TestFixture]
public class UnitTestWebReportingHandler
{

    private readonly WebReportingHandler _wrh;

    public UnitTestWebReportingHandler()
    {
        var appSettings = GlobalValues.CurrentAppSettings;


        var dataLayer = new DataHandler(appSettings.DatabaseConnection,
            appSettings.ErrorMailReceiver);


        _wrh = new WebReportingHandler(dataLayer)
        {
            TargetPath = appSettings.TargetPath,
            PreRunSql = appSettings.PreRunSql,
            StatusChange = Status
        };

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


    private static void Status(string msg)
    {
        Debug.Print(msg);
    }
}