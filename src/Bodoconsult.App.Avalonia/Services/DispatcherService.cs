// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Avalonia;
using Avalonia.Controls;

namespace Bodoconsult.App.Avalonia.Services;

/// <summary>
/// Service class to start and quit an Avalonia application if there is no one started already
/// For usage of Avalonia features in non-Avalonia application
/// </summary>
public static class DispatcherService
{

    private static CancellationTokenSource _cancellationTokenSource = new();

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

            _cancellationTokenSource.Cancel();

            //var lf = CurrentApplication.ApplicationLifetime;

            //if (lf is IClassicDesktopStyleApplicationLifetime desktop)
            //{
            //    desktop.Shutdown();
            //}
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
        //thread.SetApartmentState(ApartmentState.STA);
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

        CurrentApplication = new Application();
        CurrentApplication.Run(_cancellationTokenSource.Token);

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
        //DispatcherFrame frame = new DispatcherFrame();
        //Dispatcher.UIThread.Invoke(DispatcherPriority.Background,
        //    new DispatcherOperationCallback(ExitFrame), frame);
        //Dispatcher.PushFrame(frame);
    }

    //private static object ExitFrame(object f)
    //{
    //    ((DispatcherFrame)f).Continue = false;

    //    return null;
    //}
}