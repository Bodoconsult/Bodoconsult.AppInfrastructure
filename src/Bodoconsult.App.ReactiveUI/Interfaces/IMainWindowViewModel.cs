// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Reflection;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for view models for the main app window using ReactiveUi
/// </summary>
public interface IRxMainWindowViewModel : IUiWindowViewModel, IUiMenuWindow
{
    /// <summary>
    /// II18N instance to use with MVVM / WPF / Xamarin / Avalonia
    /// </summary>
    /// <returns>Translated string</returns>
    II18N TranslationService { get; }

    /// <summary>
    /// Menu text for open menu in system tray bar
    /// </summary>
    string OpenMenuText { get; set; }

    /// <summary>
    /// Menu text for exit menu in system tray bar
    /// </summary>
    string ExitMenuText { get; set; }

    /// <summary>
    /// Open app from taskbar icon
    /// </summary>
    Task NotifyIconOpen();

    /// <summary>
    /// Exit app from taskbar icon
    /// </summary>
    Task NotifyIconExit();

    /// <summary>
    /// Current window state
    /// </summary>
    UiWindowState WindowState { get; set; }

    /// <summary>
    /// Show the main window in taskbar
    /// </summary>
    bool ShowInTaskbar { get; set; }

    /// <summary>
    /// Inner width of the main window
    /// </summary>
    double Width { get; set; }

    /// <summary>
    /// Inner height of the main window
    /// </summary>
    double Height { get; set; }

    /// <summary>
    /// Inner height of the main window
    /// </summary>
    double HeaderHeight { get; }

    /// <summary>
    /// Current app builder
    /// </summary>
    IAppBuilder? AppBuilder { get; }

    /// <summary>
    /// Message shown during console is waiting
    /// </summary>
    string MsgConsoleWait { get; set; }

    /// <summary>
    /// Message "how to shutdown server app"
    /// </summary>
    string MsgHowToShutdownServer { get; set; }

    /// <summary>
    /// Message on what port the app is listening
    /// </summary>
    string MsgServerIsListeningOnPort { get; set; }

    /// <summary>
    /// Message with the current process ID
    /// </summary>
    string MsgServerProcessId { get; set; }

    /// <summary>
    /// Message to exit the app
    /// </summary>
    string MsgExit { get; set; }

    /// <summary>
    /// Clear text name of the app to show in windows and message boxes
    /// </summary>
    string AppName { get; set; }

    /// <summary>
    /// Application exe file name
    /// </summary>
    string AppExe { get; set; }

    /// <summary>
    /// Current app version
    /// </summary>
    string AppVersion { get; set; }

    /// <summary>
    /// Clear text name of the app with version to show in windows and message boxes
    /// </summary>
    string FullAppName { get; set; }

    /// <summary>
    /// Log data as string to show on UI
    /// </summary>
    ReadOnlyObservableCollection<string> LogData { get; }

    /// <summary>
    /// Event level
    /// </summary>
    EventLevel LogEventLevel { get; set; }

    /// <summary>
    /// Load the current <see cref="IAppBuilder"/> instance to use
    /// </summary>
    /// <param name="appBuilder">Current <see cref="IAppBuilder"/> instance to use</param>
    void LoadAppBuilder(IAppBuilder appBuilder);

    /// <summary>
    /// Load the logo
    /// </summary>
    /// <param name="assembly">Assembly to load the logo from</param>
    /// <param name="ressourcePath">Ressource path</param>
    void LoadLogo(Assembly? assembly, string ressourcePath);

    /// <summary>
    /// Shutdown for app
    /// </summary>
    void ShutDown();

    /// <summary>
    /// Minimize the app to the tray icon
    /// </summary>
    bool MinimizeToTray { get; set; }

    /// <summary>
    /// Check if there are new log entries
    /// </summary>
    void CheckLogs();

    /// <summary>
    /// The logo to use for the user interface
    /// </summary>
    ReadOnlyMemory<byte>? Logo { get; }

    /// <summary>
    /// Background color of the header line
    /// </summary>
    TypoColor HeaderBackColor { get; set; }

    /// <summary>
    /// Background color of the form body
    /// </summary>
    TypoColor BodyBackColor { get; set; }

    /// <summary>
    /// Create the main window of the application
    /// </summary>
    /// <returns></returns>
    object CreateWindow();

    /// <summary>
    /// Start the event listener
    /// </summary>
    void StartEventListener();

    /// <summary>
    /// Navigate to start user controls for the regions
    /// </summary>
    void NavigateToStart();
}