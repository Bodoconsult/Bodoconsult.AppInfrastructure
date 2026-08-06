// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.AppStarter;

/// <summary>
/// Implementation of <see cref="IAppStarterUi"/> for console app
/// </summary>
[SupportedOSPlatform("windows")]
public class WinConsoleAppStarterUi : BaseAppStarterUi
{

    private CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    /// Message shown during console is waiting
    /// </summary>
    public string MsgConsoleWait { get; set; } = string.Empty;

    /// <summary>
    /// Message "how to shitdon server app"
    /// </summary>
    public string MsgHowToShutdownServer { get; set; } = string.Empty;

    /// <summary>
    /// Default ctor
    /// </summary>
    public WinConsoleAppStarterUi(IAppBuilder appBuilder): base(appBuilder)
    {
        ConsoleService = new WinConsoleService();

        //// App is a WinForms app, therefore the console is normally hidden.
        //// We access the hidden console here and make it visible 
        //ConsoleService.CsAllocConsole();

        //ConsoleService.ConsoleHandle = ConsoleService.CsGetConsoleWindow();
        //ConsoleService.CsShowWindow(ConsoleService.ConsoleHandle, ConsoleService.ShowWindowShow);
    }


    /// <summary>
    /// Wait while the application runs
    /// </summary>
    public override void Wait()
    {
        try
        {
            var msg = MsgConsoleWait;

            AppBuilder.AppGlobals.Logger?.LogInformation(msg);
            Debug.Print(msg);
            Debug.Print(MsgHowToShutdownServer);

            _cancellationTokenSource = new CancellationTokenSource();

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                AsyncHelper.Delay(1000);
            }
        }
        catch (Exception e)
        {
            HandleException(e);
        }
    }


    /// <summary>
    /// Show a message and then terminate the app
    /// </summary>
    /// <param name="message">Message to show before app termination</param>
    /// <param name="appTitle">App title to set</param>
    public override void TerminateAppWithMessage(string message, string appTitle)
    {
        _cancellationTokenSource?.Cancel();

        Debug.Print(message);
        Console.WriteLine(message);
        AsyncHelper.Delay(5000);

    }

    /// <summary>
    /// Central handling for exceptions
    /// </summary>
    /// <param name="e"></param>
    public override void HandleException(Exception e)
    {
        //if (e == null)
        //{
        //    return;
        //}

        try
        {
            var msg = UiMessages.HandleException(e);

            // ToDo: correct handling of line breaks
            Debug.Print(msg);
        }
        catch (Exception exception)
        {
            Debug.Print(exception.Message);
        }

        AsyncHelper.Delay(5000);
        Environment.Exit(0);
    }
}