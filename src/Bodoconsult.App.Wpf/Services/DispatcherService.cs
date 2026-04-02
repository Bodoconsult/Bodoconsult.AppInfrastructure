// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Threading;

namespace Bodoconsult.App.Wpf.Services;

/// <summary>
/// Service class to start and quit an WPF application if there is no one started already
/// For usage of WPF features in non-WPF application
/// </summary>
[SupportedOSPlatform("windows")]
public static class DispatcherService
{


    /// <summary>
    /// Contains the current WPF application
    /// </summary>
    public static Application CurrentApplication { get; set; }

    /// <summary>
    /// Dispose the dispatcher to close the application finally (only needed in NON-WPF-Applications)
    /// </summary>
    public static void DisposeDispatcher()
    {
        try
        {
            CurrentApplication.Dispatcher.InvokeShutdown();
            Application.Current.Dispatcher.InvokeShutdown();
        }
        catch
        {
            // ignored
        }


        //for (var i = TempFiles.Count - 1; i >= 0; i--)
        //{
        //    var fileName = TempFiles[i];
        //    if (File.Exists(fileName)) File.Delete(fileName);
        //}
    }

    /// <summary>
    /// Open the dispatcher  (only needed in NON-WPF-Applications)
    /// </summary>
    public static void OpenDispatcher()
    {
        TempFiles = new List<string>();

        if (Application.Current != null)
        {
            return;
        }

        var thread = new Thread(CreateApp);
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
 
        Thread.Sleep(300);
    }

    private static void CreateApp()
    {
        if (Application.Current != null)
        {
            CurrentApplication = Application.Current;
            return;
        }

        CurrentApplication = new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
        CurrentApplication.Run();
    }

    /// <summary>
    /// Keeps paths to temporary files to be deleted on shutdown of the application
    /// </summary>
    public static IList<string> TempFiles { get; set; }



    /// <summary>
    /// Application.DoEvents for WPF
    /// </summary>
    public static void DoEvents()
    {
        var frame = new DispatcherFrame();
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
            new DispatcherOperationCallback(ExitFrame), frame);
        Dispatcher.PushFrame(frame);
    }

    private static object ExitFrame(object f)
    {
        ((DispatcherFrame)f).Continue = false;

        return null;
    }
}