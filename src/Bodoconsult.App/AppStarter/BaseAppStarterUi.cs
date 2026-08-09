// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.AppStarter;

/// <summary>
/// Base class for <see cref="IAppStarterUi"/> implementations used for Econsole apps and WinForms based apps
/// </summary>
public class BaseAppStarterUi : IAppStarterUi
{
    /// <summary>
    /// Console service for managing the console
    /// </summary>
    protected IConsoleService? ConsoleService;

    /// <summary>
    /// Default ctor
    /// </summary>
    public BaseAppStarterUi(IAppBuilder appBuilder)
    {
        AppBuilder = appBuilder;
    }

    /// <summary>
    /// The current app start process handler
    /// </summary>
    public IAppBuilder AppBuilder { get; }

    /// <summary>
    /// Is already another instance started?
    /// </summary>
    public bool IsAnotherInstance
    {

        get
        {
            if (!AppBuilder.AppGlobals.AppStartParameter.IsSingletonApp)
            {
                return false;
            }

            AppBuilder.AppGlobals.EventWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset, $"Global\\{AppBuilder.AppGlobals.AppStartParameter.AppName}", out var created);
            if (created)
            {
                return false;
            }

            //// Check if app is already started
            //var currentProcess = Process.GetCurrentProcess();
            //var runningProcess = (from process in Process.GetProcesses()
            //    where
            //        process.Id != currentProcess.Id &&
            //        process.ProcessName.Equals(
            //            currentProcess.ProcessName,
            //            StringComparison.Ordinal)
            //    select process).FirstOrDefault();

            //if (runningProcess is null)
            //{
            //    return false;
            //}

            ArgumentNullException.ThrowIfNull(ConsoleService);

            ConsoleService.ConsoleHandle = ConsoleService.CsGetConsoleWindow();
            ConsoleService.CsShowWindow(ConsoleService.ConsoleHandle, ConsoleService.ShowWindowShow);
            return true;

        }


    }

    /// <summary>
    /// Start the app
    /// </summary>
    public virtual void Start()
    {
        try
        {
            // Start app logic in a separate thread from UI
            var appThread = new Thread(() => AppBuilder.StartApplicationService(null))
            {
                Priority = ThreadPriority.AboveNormal,
                IsBackground = true
            };
            appThread.Start();
        }
        catch (Exception e)
        {
            AppBuilder.AppGlobals.Logger?.LogError($"{AppBuilder.AppGlobals.AppStartParameter.AppName} is closed due to error", e);
            HandleException(e);
            Environment.Exit(0);
        }
    }

    /// <summary>
    /// Wait while the application runs
    /// </summary>
    public virtual void Wait()
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Show a message and then terminate the app
    /// </summary>
    /// <param name="message">Message to show before app termination</param>
    /// <param name="appTitle">App title to set</param>
    public virtual void TerminateAppWithMessage(string message, string appTitle)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Central handling for exceptions
    /// </summary>
    /// <param name="e"></param>
    public virtual void HandleException(Exception e)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Current implementation of Dispose()
    /// </summary>
    /// <param name="disposing">Disposing required: true or false</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
                
        }
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);
        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Finalizer
    /// </summary>
    ~BaseAppStarterUi()
    {
        Dispose(false);
    }
}