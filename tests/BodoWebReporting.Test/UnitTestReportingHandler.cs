// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using BodoWebReporting.Dal;
using BodoWebReporting.Dal.Handler;
using BodoWebReporting.Dal.Model;
using NUnit.Framework;

namespace BodoWebReporting.Test;

[SupportedOSPlatform("windows")]
[TestFixture]
public class UnitTestWebReportHandler
{

    public UnitTestWebReportHandler()
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
    public void TestRun_Success()
    {

        var appSettings = GlobalValues.CurrentAppSettings;


        var dataLayer = new DataHandler(appSettings.DatabaseConnection,
            appSettings.ErrorMailReceiver);


        var rh = new WebReportHandler(dataLayer)
        {
            TargetPath = appSettings.TargetPath,
            Target = ReportingTargets.PdfFile,
        };
        rh.StatusChange += Status;

        rh.OpenDatabase();

        rh.StartExport();
        rh.MoveToTarget();

    }

    private static void Status(string msg)
    {
        Debug.Print(msg);
    }
}