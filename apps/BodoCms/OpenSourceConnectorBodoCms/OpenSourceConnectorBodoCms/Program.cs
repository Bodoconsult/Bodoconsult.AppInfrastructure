// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace OpenSourceConnectorBodoCms;

internal class Program
{
    private static int Main(string[] args)
    {
        var starter = new OpenSourceConnectorOpenSourceConnectorBodoCmsConsoleStarter([]);
        return starter.Start();
        //Status("OpenSourceConnectorToOpenSourceConnectorBodoCms: started...");

        //GlobalValues.LoadAppSettings();

        //var settings = GlobalValues.CurrentAppSettings;

        //var registerService = new RegisterFolderService()
        //{
        //    TargetPath = settings.TargetPath,
        //    FolderFilter = settings.FolderFilter,
        //    SourcePath = settings.SourcePath
        //};

        //registerService.GetAllProjectFolders();

        //var service = new ExportService(registerService.OpenSourceFolders, settings.TargetPath,
        //    registerService.Categories)
        //{
        //    StatusMessageDelegate= Status
        //};
        //service.CreateFolders();

        //service.CreateHtml();

        //service.CreateStartpage();

        //Status("OpenSourceConnectorToOpenSourceConnectorBodoCms: done!");

    }


    ///// <summary>
    ///// Status anzeigen
    ///// </summary>
    ///// <param name="msg">Anzuzeigende Nachricht</param>
    //static void Status(string msg)
    //{
    //    Console.WriteLine(msg);
    //}
}