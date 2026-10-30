// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using BodoCmsConsole.Business.Interfaces;
using Bodoconsult.App.Abstractions.Interfaces;

namespace BodoCmsConsole.Business.Handlers;

/// <summary>
/// Current implementaton of <see cref="IBodoCmsHandler"/> as top level business logic class
/// </summary>
[SupportedOSPlatform("windows")]
public class BodoCmsHandler : IBodoCmsHandler
{
    private readonly IWebsiteBuilder _webBuilder;
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="webBuilder">Current web builder instance</param>
    /// <param name="appGlobals">Current app globals</param>
    public BodoCmsHandler(IWebsiteBuilder webBuilder, IAppGlobals appGlobals)
    {
        _webBuilder = webBuilder;
        _appGlobals = appGlobals;
    }

    //public StatusMessageDelegate? StatusMessage { get; set; }

    public void CreateHomepage()
    {
        var wb = _webBuilder;
        wb.StatusMessageDelegate = _appGlobals.StatusMessageDelegate ?? SendDefaultStateMessage;
        wb.Init();

        // Grundlegende Arbeiten
        wb.PrepareDirectories();
        wb.LoadTheme();
        wb.PreparePages();
        wb.CopyBaseFiles();


        // Bilder handeln
        wb.CheckNewHeaderFiles();

        wb.ClearImages(); // Do not turn off!!!!

        wb.FindImages();
        wb.CopyImages();
        wb.ClearThumbs();
        wb.PrepareImages();

        //_wb.CheckImagesAtTargetPath();
        wb.ClearOldPages();
        wb.OtherDirectories();

        //Console.ReadLine();
        wb.BuildPages();
        wb.Sitemap();

        wb.SendErrorMail();

        wb.Dispose();
    }

    private static void SendDefaultStateMessage(string msg)
    {
        Console.WriteLine(msg);
    }
}