// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Logging;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using Bodoconsult.App.ReactiveUI.Ui;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Reflection;
using DynamicData;

namespace Bodoconsult.App.ReactiveUI.ViewModels;

/// <summary>
/// ViewModel for MainWindow window
/// </summary>
public partial class MainWindowViewModel : ReactiveObject, IRxMainWindowViewModel
{
    private bool _showInTaskbar;
    private UiWindowState _windowState;

    private Timer? _logDataTimer;

    private const int MaxNumberOfLogEntries = 100;

    private readonly IAppEventListener? _listener;

    private ObservableCollectionExtended<string> LogDataSource { get; } = [];

    private EventLevel _logEventLevel;
    private double _width = 100;
    private double _height = 100;
    private ObservableAsPropertyHelper<double> _headerHeight = ObservableAsPropertyHelper<double>.Default();
    private IAppBuilder? _appBuilder;

    private string _msgConsoleWait = string.Empty;
    private string _msgHowToShutdownServer = string.Empty;
    private string _msgExit = "Exit the app?";
    private string _msgServerIsListeningOnPort = string.Empty;
    private string _msgServerProcessId = string.Empty;
    private string? _appExe;

    private ReadOnlyMemory<byte>? _logo;
    private TypoColor _headerBackColor = TypoColors.Coral;
    private TypoColor _bodyBackColor = TypoColors.LightGray;


    private bool _minimizeToTray;

    /// <summary>
    /// Ctor providing an <see cref="AppEventListener"/> instance
    /// </summary>
    /// <param name="listener">Current EventSource listener: neede to bring logging entries to UI</param>
    /// <param name="translationService">Translation service. Use DummyI18N in case of no translations needed</param>
    /// <param name="regionManager">Current region manager</param>
    public MainWindowViewModel(IAppEventListener listener, II18N translationService, IRegionManager regionManager)
    {
        TranslationService = translationService;
        _listener = listener;
        RegionManager = regionManager;
        WindowState = UiWindowState.Maximized;
        ShowInTaskbar = true;
        OpenMenuText = "Open";
        ExitMenuText = "Exit";

        TranslationService = translationService;

        // Use the ToObservableChangeSet operator to convert
        // the observable collection to IObservable<IChangeSet<T>>
        // which describes the changes. Then, use any DD operators
        // to transform the collection. 
        LogDataSource.ToObservableChangeSet()
            .Transform(value => value)
            // No need to use the .ObserveOn() operator here, as
            // ObservableCollectionExtended is single-threaded.
            .Bind(out _logDataInternal)
            .Subscribe();

        //this.WhenAnyValue(x => x._flowDoc)
        //    .Select(x=> x.Value)
        //    .ToProperty(this, x => x.LogData, out _flowDoc);

        //this.WhenAnyValue(x => x._headerHeight)
        //    .Select(x => x.Value * 0.15)
        //    .ToProperty(this, x => x.HeaderHeight, out _headerHeight);
    }

    /// <summary>
    /// II18N instance to use with MVVM / WPF / Xamarin / Avalonia
    /// </summary>
    /// <returns>Translated string</returns>
    public II18N TranslationService { get; }

    /// <summary>
    /// Menu text for open menu in system tray bar
    /// </summary>
    [Reactive] public partial string OpenMenuText { get; set; }

    /// <summary>
    /// Menu text for exit menu in system tray bar
    /// </summary>
    [Reactive] public partial string ExitMenuText { get; set; }

    /// <summary>
    /// Instance name of the window. If null or string.Empty the window instance name is derived from the window type name (loading the window as a singleton instance)
    /// </summary>
    [Reactive] public partial string? InstanceName { get; set; }

    /// <summary>
    /// Current region manager
    /// </summary>
    public IRegionManager RegionManager { get; }

    /// <summary>
    /// Region 1
    /// </summary>
    [Reactive]
    public partial UiRegion? Region1 { get; set; }

    /// <summary>
    /// Region 2
    /// </summary>
    [Reactive]
    public partial UiRegion? Region2 { get; set; }

    /// <summary>
    /// Region 3
    /// </summary>
    [Reactive]
    public partial UiRegion? Region3 { get; set; }

    /// <summary>
    /// Open app from taskbar icon
    /// </summary>
    public Task NotifyIconOpen()
    {
        WindowState = UiWindowState.Normal;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Exit app from taskbar icon
    /// </summary>
    public async Task NotifyIconExit()
    {
        await Task.Run(ShutDown);
    }

    /// <summary>
    /// Current window state
    /// </summary>
    public UiWindowState WindowState
    {
        get => _windowState;
        set
        {
            ShowInTaskbar = true;
            this.RaiseAndSetIfChanged(ref _windowState, value);
            ShowInTaskbar = value != UiWindowState.Minimized;
        }
    }

    /// <summary>
    /// Show the main window in taskbar
    /// </summary>
    public bool ShowInTaskbar
    {
        get => _showInTaskbar;
        set => this.RaiseAndSetIfChanged(ref _showInTaskbar, value);
    }

    /// <summary>
    /// Inner width of the main window
    /// </summary>
    public double Width
    {
        get => _width;
        set => this.RaiseAndSetIfChanged(ref _width, value);
    }

    /// <summary>
    /// Inner height of the main window
    /// </summary>
    public double Height
    {
        get => _height;
        set
        {
            this.RaiseAndSetIfChanged(ref _height, value);
            _headerHeight = ObservableAsPropertyHelper<double>.Default(value);
        }
    }

    /// <summary>
    /// Inner height of the main window
    /// </summary>
    public double HeaderHeight => _headerHeight.Value;


    /// <summary>
    /// Current app start process handler
    /// </summary>
    public IAppBuilder? AppBuilder
    {
        get => _appBuilder;
        private set => this.RaiseAndSetIfChanged(ref _appBuilder, value);
    }

    /// <summary>
    /// Message shown during console is waiting
    /// </summary>
    public string MsgConsoleWait
    {
        get => _msgConsoleWait;
        set => this.RaiseAndSetIfChanged(ref _msgConsoleWait, value);
    }

    /// <summary>
    /// Message "how to shitdon server app"
    /// </summary>
    public string MsgHowToShutdownServer
    {
        get => _msgHowToShutdownServer;
        set => this.RaiseAndSetIfChanged(ref _msgHowToShutdownServer, value);
    }

    /// <summary>
    /// Message on what port the app is listening
    /// </summary>
    public string MsgServerIsListeningOnPort
    {
        get => _msgServerIsListeningOnPort;
        set => this.RaiseAndSetIfChanged(ref _msgServerIsListeningOnPort, value);
    }

    /// <summary>
    /// Message with the current process ID
    /// </summary>
    public string MsgServerProcessId
    {
        get => _msgServerProcessId;
        set => this.RaiseAndSetIfChanged(ref _msgServerProcessId, value);
    }

    /// <summary>
    /// Message to exit the app
    /// </summary>
    public string MsgExit
    {
        get => _msgExit;
        set => this.RaiseAndSetIfChanged(ref _msgExit, value);
    }


    /// <summary>
    /// Clear text name of the app to show in windows and message boxes
    /// </summary>
    public string AppName
    {
        get => AppBuilder?.AppGlobals.AppStartParameter.AppName ?? string.Empty;
        set
        {
            //if (value == AppBuilder.AppGlobals.AppStartParameter.AppName)
            //{
            //    return;
            //}
            //_appVersion = value;
            //OnPropertyChanged();
        }
    }

    /// <summary>
    /// Application exe file name
    /// </summary>
    public string AppExe
    {
        get => _appExe ?? string.Empty;
        //set
        //{
        //    if (value == _appExe)
        //    {
        //        return;
        //    }
        //    _appExe = value;
        //    OnPropertyChanged();
        //}
        set => this.RaiseAndSetIfChanged(ref _appExe, value);
    }


    /// <summary>
    /// Clear text name of the app with version to show in windows and message boxes
    /// </summary>
    public string FullAppName
    {
        get =>
            $"{AppBuilder?.AppGlobals.AppStartParameter.AppName} {AppBuilder?.AppGlobals.AppStartParameter.AppVersion}";
        set
        {
            //if (value == AppBuilder.AppGlobals.AppStartParameter.AppName)
            //{
            //    return;
            //}
            ////_appVersion = value;
            //OnPropertyChanged();
        }
    }

    /// <summary>
    /// Current app version
    /// </summary>
    public string AppVersion
    {
        get => AppBuilder?.AppGlobals.AppStartParameter.AppVersion ?? string.Empty;
        set
        {
            //if (value == AppBuilder.AppGlobals.AppStartParameter.AppVersion)
            //{
            //    return;
            //}
            ////_appVersion = value;
            //OnPropertyChanged();
        }
    }

    /// <summary>
    /// Load the current <see cref="IAppBuilder"/> instance to use
    /// </summary>
    /// <param name="appBuilder">Current <see cref="IAppBuilder"/> instance to use</param>
    public void LoadAppBuilder(IAppBuilder appBuilder)
    {
        AppBuilder = appBuilder;

        MsgServerIsListeningOnPort = AppBuilder.AppGlobals.AppStartParameter.Port == 0 ? string.Empty : $"{UiMessages.MsgServerIsListeningOnPort} {AppBuilder.AppGlobals.AppStartParameter.Port}";
        MsgHowToShutdownServer = UiMessages.MsgHowToShutdownServer;
        MsgServerProcessId = $"{UiMessages.MsgServerProcessId} {Environment.ProcessId}";
    }

    /// <summary>
    /// Load the logo
    /// </summary>
    /// <param name="assembly">Assembly to load the logo from</param>
    /// <param name="ressourcePath">Ressource path</param>
    public void LoadLogo(Assembly assembly, string ressourcePath)
    {
        //try
        //{

        //if (assembly == null)
        //{
        //    return;
        //}

        var logoStream = assembly.GetManifestResourceStream(ressourcePath);

        if (logoStream == null)
        {
            return;
        }

        logoStream.Position = 0;

        using (var memoryStream = new MemoryStream())
        {
            logoStream.CopyTo(memoryStream);
            Logo = memoryStream.ToArray();
        }

        var rm = ReadOnlyMemory<byte>.Empty;

        Logo = rm;
        logoStream.Close();
        logoStream.Dispose();

        //}
        //catch
        //{
        //    // Do nothing
        //}
    }

    /// <summary>
    /// Shutdown for app
    /// </summary>
    public void ShutDown()
    {
        AppBuilder?.StopApplication();

        if (_logDataTimer != null)
        {
            _logDataTimer.Change(
                Timeout.Infinite,
                Timeout.Infinite);
            _logDataTimer.Dispose();
        }

        Environment.Exit(0);
    }

    ///// <summary>
    ///// Show a notification
    ///// </summary>
    ///// <param name="notification">Notification to show</param>
    //public void ShowNotification(NotificationData notification)
    //{
    //    Notification = notification;
    //}

    /// <summary>
    /// Minimize the app to the tray icon
    /// </summary>
    public bool MinimizeToTray
    {
        get => _minimizeToTray;
        set => this.RaiseAndSetIfChanged(ref _minimizeToTray, value);
    }

    /// <summary>
    /// Check if there are new log entries
    /// </summary>
    public void CheckLogs()
    {

        if (_listener == null)
        {
            return;
        }

        var count = _listener.Messages.Count;

        if (count == 0)
        {
            return;
        }

        // Keep maximum log data length equal to MaxNumberOfLogEntries
        if (LogDataSource.Count > 0 && LogDataSource.Count + count > MaxNumberOfLogEntries)
        {
            for (var i = LogDataSource.Count - MaxNumberOfLogEntries - 2; i >= 0; i--)
            {
                LogDataSource.Remove(LogDataSource[i]);
            }
        }

        // Add the received messages to log data
        for (var i = 0; i < count; i++)
        {
            var logMsg = GeneralHelper.DequeueFromQueue(_listener.Messages);

            if (LogDataSource.Count > MaxNumberOfLogEntries)
            {
                continue;
            }

            LogDataSource.Add(logMsg);
        }

        // If there are to many entries
        for (var i = LogDataSource.Count - MaxNumberOfLogEntries - 2; i >= 0; i--)
        {
            LogDataSource.Remove(LogDataSource[i]);
        }
    }

    private readonly ReadOnlyObservableCollection<string> _logDataInternal;

    /// <summary>
    /// Log data as string to show on UI
    /// </summary>
    public ReadOnlyObservableCollection<string> LogData => _logDataInternal;

    /// <summary>
    /// Event level
    /// </summary>
    public EventLevel LogEventLevel
    {
        get => _logEventLevel;
        set
        {
            if (value == _logEventLevel || _listener == null)
            {
                return;
            }
            _logEventLevel = value;
            _listener.EventLevel = _logEventLevel;
            this.RaiseAndSetIfChanged(ref _logEventLevel, value);
        }
    }

    /// <summary>
    /// The logo to use for the user interface
    /// </summary>
    public ReadOnlyMemory<byte>? Logo
    {
        get => _logo;
        private set => this.RaiseAndSetIfChanged(ref _logo, value);
    }

    /// <summary>
    /// Background color of the header line
    /// </summary>
    public TypoColor HeaderBackColor
    {
        get => _headerBackColor;
        set => this.RaiseAndSetIfChanged(ref _headerBackColor, value);
    }

    /// <summary>
    /// Background color of the form body
    /// </summary>
    public TypoColor BodyBackColor
    {
        get => _bodyBackColor;
        set => this.RaiseAndSetIfChanged(ref _bodyBackColor, value);
    }

    /// <summary>
    /// Create the main form of the application
    /// </summary>
    /// <returns></returns>
    public virtual object CreateWindow()
    {
        throw new NotSupportedException("Override in superclass");
    }

    /// <summary>
    /// Start the event listener
    /// </summary>
    public void StartEventListener()
    {
        _logDataTimer = new Timer(dispatcherTimer_Tick, null, 1000, 1000);
    }

    private void dispatcherTimer_Tick(object? state)
    {
        _logDataTimer?.Change(
            Timeout.Infinite,
            Timeout.Infinite);

        try
        {
            CheckLogs();
        }
        catch //(Exception exception)
        {
            // Do nothing
        }

        _logDataTimer?.Change(
            1000,
            1000);
    }


    /// <summary>Gets the Router associated with this Screen.</summary>
    public RoutingState Router { get; set; } = new();

    /// <summary>
    /// Menu items for a menu in the window
    /// </summary>
    public List<IUiMenuItem> MenuItems { get;} = [];

    /// <summary>
    /// <see cref="IUiMenuBuilder"/> instance used for the current window
    /// </summary>
    public IUiMenuBuilder? MenuBuilder { get; set; }

    /// <summary>
    /// Define the menu items to be stored in <see cref="IUiMenuWindow.MenuItems"/>
    /// </summary>
    public virtual void DefineMenuItems()
    {
        // Do nothing
    }

    /// <summary>
    /// Build the menu with the menu builder <see cref="IUiMenuWindow.MenuBuilder"/> from the menu items <see cref="IUiMenuWindow.MenuItems"/>
    /// </summary>
    public void BuildIt()
    {
        if (MenuBuilder == null)
        {
            return;
        }

        MenuBuilder.Clear();
        MenuBuilder.AddRange(MenuItems);
        MenuBuilder.BuildIt();
    }
}