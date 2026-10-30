// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.Runtime.Versioning;
using BodoCmsConsole.Business.Builders;
using BodoCmsConsole.Business.Handlers;
using BodoCmsConsole.Business.Test.App;
using Bodoconsult.Database.SqlClient;
using NUnit.Framework;

namespace BodoCmsConsole.Business.Test;

[SupportedOSPlatform("windows")]
[TestFixture]
public class WebBuilderTests
{
    private WebsiteBuilder? _wb;
    private DatabaseHandler? _dh;
    private readonly Globals _appGlobals = Globals.Instance;

    [SetUp]
    public void Setup()
    {
        ArgumentNullException.ThrowIfNull(_appGlobals.Logger);
        ArgumentException.ThrowIfNullOrEmpty(_appGlobals.AppStartParameter.DefaultConnectionString);
        var db = new SqlClientConnManager(_appGlobals.AppStartParameter.DefaultConnectionString);
        _dh = new DatabaseHandler(db, _appGlobals.Logger);

        _wb = new WebsiteBuilder(_dh, _appGlobals);
        _wb.StatusMessageDelegate += SetMessage;
        _wb?.Init();

        // Grundlegende Arbeiten
        _wb?.PrepareDirectories();
        _wb?.LoadTheme();
        _wb?.PreparePages();
        _wb?.CopyBaseFiles();


        //// Bilder handeln
        //_wb.CheckNewHeaderFiles();
        //_wb.ClearImages();
        //_wb.FindImages();
        //_wb.CopyImages();
        //_wb.ClearThumbs();
        //_wb.PrepareImages();

        ////_wb.CheckImagesAtTargetPath();
        //_wb.ClearOldPages();
        //_wb.OtherDirectories();

        ////Console.ReadLine();
        //_wb.BuildPages();
        //_wb.Sitemap();
            
    }

    private void SetMessage(string msg)
    {
        Debug.Print(msg);
    }


    [TearDown]
    public void Cleanup()
    {
        _wb?.Dispose();
    }

    [Test]
    public void TestFindImages()
    {
        var count = _wb?.FindImages();

        Assert.That(count, Is.GreaterThan(0));
    }

    [Test]
    public void TestOtherDirectories()
    {
        var count = _wb?.FindImages();

        Assert.That(count, Is.GreaterThan(0));

        _wb?.ClearOldPages();
        count = _wb?.OtherDirectories();

        Assert.That(count, Is.GreaterThan(0));
    }
}