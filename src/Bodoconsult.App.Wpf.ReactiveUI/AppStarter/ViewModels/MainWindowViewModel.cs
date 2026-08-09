//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using Bodoconsult.App.Abstractions.Interfaces;
//using Bodoconsult.App.Helpers;
//using Bodoconsult.App.Logging;
//using Bodoconsult.App.Wpf.AppStarter.Views;
//using CommunityToolkit.Mvvm.Input;
//using ReactiveUI;
//using System.Diagnostics;
//using System.Diagnostics.Tracing;
//using System.Reflection;
//using System.Windows;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Threading;
//using Bodoconsult.App.ReactiveUI.Interfaces;
//using Bodoconsult.App.ReactiveUI.Regions;
//using ReactiveUI.SourceGenerators;

//namespace Bodoconsult.App.Wpf.ReactiveUI.AppStarter.ViewModels;

///// <summary>
///// ViewModel for MainWindow window
///// </summary>
//public partial class MainWindowViewModel : ReactiveObject, Interfaces.IMainWindowViewModel, IUiWindowViewModel
//{

//    private bool _showInTaskbar;
//    private WindowState _windowState;

//    private DispatcherTimer? _dispatcherTimer;

//    private const int MaxNumberOfLogEntries = 100;

//    private readonly IAppEventListener? _listener;

//    private readonly List<string> _logData = new();

//    private EventLevel _logEventLevel;
//    private double _width = 100;
//    private double _height = 100;
//    private ObservableAsPropertyHelper<double> _headerHeight = ObservableAsPropertyHelper<double>.Default(0);
//    private IAppBuilder? _appBuilder;
//    private string _msgConsoleWait = string.Empty;
//    private string _msgHowToShutdownServer = string.Empty;
//    private string _msgExit = "Exit the app?";
//    private BitmapImage? _logo;
//    private Color _headerBackColor = Colors.Coral;
//    private string _msgServerIsListeningOnPort = string.Empty;
//    private string _msgServerProcessId = string.Empty;
//    private string? _appExe;
//    private bool _minimizeToTray;

//    private readonly SolidColorBrush _brush = new(Colors.LightSteelBlue);
//    private readonly SolidColorBrush _brush1 = new(Colors.White);
//    private readonly Thickness _margin = new(0, 0, 0, 0);
//    private readonly Thickness _padding = new(0, 10, 0, 10);
//    private Color _bodyBackColor = Colors.LightGray;

//    /// <summary>
//    /// Ctor providing an <see cref="AppEventListener"/> instance
//    /// </summary>
//    /// <param name="listener">Current EventSource listener: neede to bring logging entries to UI</param>
//    /// <param name="translationService">Translation service</param>
//    /// <param name="regionManager">Current region manager</param>
//    public MainWindowViewModel(IAppEventListener listener, II18N translationService, IRegionManager regionManager)
//    {
//        TranslationService = translationService;
//        _listener = listener;
//        RegionManager = regionManager;
//        NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Normal; });
//        NotifyIconExitCommand = new RelayCommand(ShutDown);
//        WindowState = WindowState.Normal;
//        ShowInTaskbar = true;

//        var doc = new FlowDocument();
//        _flowDoc = ObservableAsPropertyHelper<FlowDocument>.Default(doc);


//        //this.WhenAnyValue(x => x._flowDoc)
//        //    .Select(x=> x.Value)
//        //    .ToProperty(this, x => x.LogData, out _flowDoc);

//        //this.WhenAnyValue(x => x._headerHeight)
//        //    .Select(x => x.Value * 0.15)
//        //    .ToProperty(this, x => x.HeaderHeight, out _headerHeight);
//    }

//    /// <summary>
//    /// II18N instance to use with MVVM / WPF / Xamarin / Avalonia
//    /// </summary>
//    /// <returns>Translated string</returns>
//    public II18N? TranslationService { get; }

//    /// <summary>
//    /// Menu text for open menu in system tray bar
//    /// </summary>
//    public string OpenMenuText { get; set; } = "Open";

//    /// <summary>
//    /// Menu text for exit menu in system tray bar
//    /// </summary>
//    public string ExitMenuText { get; set; } = "Exit";

//    /// <summary>
//    /// Open command for binding in XAML to taskbar
//    /// </summary>
//    public ICommand NotifyIconOpenCommand { get; }

//    /// <summary>
//    /// Exit command for binding in XAML to taskbar
//    /// </summary>
//    public ICommand NotifyIconExitCommand { get; }

//    /// <summary>
//    /// Instance name of the window. If null or string.Empty the window instance name is derived from the window type name (loading the window as a singleton instance)
//    /// </summary>
//    public string? InstanceName { get; set; } = null;

//    /// <summary>
//    /// Current region manager
//    /// </summary>
//    public IRegionManager RegionManager { get; }

//    /// <summary>
//    /// Region 1
//    /// </summary>
//    [Reactive]
//    public partial UiRegion? Region1 { get; set; }

//    /// <summary>
//    /// Region 2
//    /// </summary>
//    [Reactive]
//    public partial UiRegion? Region2 { get; set; }

//    /// <summary>
//    /// Region 3
//    /// </summary>
//    [Reactive]
//    public partial UiRegion? Region3 { get; set; }

//    /// <summary>
//    /// Current window state
//    /// </summary>
//    public WindowState WindowState
//    {
//        get => _windowState;
//        set
//        {
//            ShowInTaskbar = true;
//            this.RaiseAndSetIfChanged(ref _windowState, value);
//            ShowInTaskbar = value != WindowState.Minimized;
//        }
//    }

//    /// <summary>
//    /// Show the main window in taskbar
//    /// </summary>
//    public bool ShowInTaskbar
//    {
//        get => _showInTaskbar;
//        set => this.RaiseAndSetIfChanged(ref _showInTaskbar, value);
//    }

//    /// <summary>
//    /// Inner width of the main window
//    /// </summary>
//    public double Width
//    {
//        get => _width;
//        set => this.RaiseAndSetIfChanged(ref _width, value);
//    }

//    /// <summary>
//    /// Inner height of the main window
//    /// </summary>
//    public double Height
//    {
//        get => _height;
//        set
//        {
//            this.RaiseAndSetIfChanged(ref _height, value);
//            _headerHeight = ObservableAsPropertyHelper<double>.Default(value);
//        }
//    }

//    /// <summary>
//    /// Inner height of the main window
//    /// </summary>
//    public double HeaderHeight => _headerHeight.Value;


//    /// <summary>
//    /// Current app start process handler
//    /// </summary>
//    public IAppBuilder? AppBuilder
//    {
//        get => _appBuilder;
//        private set => this.RaiseAndSetIfChanged(ref _appBuilder, value);
//    }

//    /// <summary>
//    /// Message shown during console is waiting
//    /// </summary>
//    public string MsgConsoleWait
//    {
//        get => _msgConsoleWait;
//        set => this.RaiseAndSetIfChanged(ref _msgConsoleWait, value);
//    }

//    /// <summary>
//    /// Message "how to shitdon server app"
//    /// </summary>
//    public string MsgHowToShutdownServer
//    {
//        get => _msgHowToShutdownServer;
//        set => this.RaiseAndSetIfChanged(ref _msgHowToShutdownServer, value);
//    }

//    /// <summary>
//    /// Message on what port the app is listening
//    /// </summary>
//    public string MsgServerIsListeningOnPort
//    {
//        get => _msgServerIsListeningOnPort;
//        set => this.RaiseAndSetIfChanged(ref _msgServerIsListeningOnPort, value);
//    }

//    /// <summary>
//    /// Message with the current process ID
//    /// </summary>
//    public string MsgServerProcessId
//    {
//        get => _msgServerProcessId;
//        set => this.RaiseAndSetIfChanged(ref _msgServerProcessId, value);
//    }

//    /// <summary>
//    /// Message to exit the app
//    /// </summary>
//    public string MsgExit
//    {
//        get => _msgExit;
//        set => this.RaiseAndSetIfChanged(ref _msgExit, value);
//    }


//    /// <summary>
//    /// Clear text name of the app to show in windows and message boxes
//    /// </summary>
//    public string AppName
//    {
//        get => AppBuilder?.AppGlobals.AppStartParameter.AppName ?? string.Empty;
//        set
//        {
//            //if (value == AppBuilder.AppGlobals.AppStartParameter.AppName)
//            //{
//            //    return;
//            //}
//            //_appVersion = value;
//            //OnPropertyChanged();
//        }
//    }

//    /// <summary>
//    /// Application exe file name
//    /// </summary>
//    public string AppExe
//    {
//        get => _appExe ?? string.Empty;
//        //set
//        //{
//        //    if (value == _appExe)
//        //    {
//        //        return;
//        //    }
//        //    _appExe = value;
//        //    OnPropertyChanged();
//        //}
//        set => this.RaiseAndSetIfChanged(ref _appExe, value);
//    }


//    /// <summary>
//    /// Clear text name of the app with version to show in windows and message boxes
//    /// </summary>
//    public string FullAppName
//    {
//        get =>
//            $"{AppBuilder?.AppGlobals.AppStartParameter.AppName} {AppBuilder?.AppGlobals.AppStartParameter.AppVersion}";
//        set
//        {
//            //if (value == AppBuilder.AppGlobals.AppStartParameter.AppName)
//            //{
//            //    return;
//            //}
//            ////_appVersion = value;
//            //OnPropertyChanged();
//        }
//    }

//    /// <summary>
//    /// Current app version
//    /// </summary>
//    public string AppVersion
//    {
//        get => AppBuilder?.AppGlobals.AppStartParameter.AppVersion ?? string.Empty;
//        set
//        {
//            //if (value == AppBuilder.AppGlobals.AppStartParameter.AppVersion)
//            //{
//            //    return;
//            //}
//            ////_appVersion = value;
//            //OnPropertyChanged();
//        }
//    }

//    /// <summary>
//    /// Load the current <see cref="IAppBuilder"/> instance to use
//    /// </summary>
//    /// <param name="appBuilder">Current <see cref="IAppBuilder"/> instance to use</param>
//    public void LoadAppBuilder(IAppBuilder appBuilder)
//    {
//        AppBuilder = appBuilder;

//        MsgServerIsListeningOnPort = AppBuilder.AppGlobals.AppStartParameter.Port == 0 ? string.Empty : $"{UiMessages.MsgServerIsListeningOnPort} {AppBuilder.AppGlobals.AppStartParameter.Port}";
//        MsgHowToShutdownServer = UiMessages.MsgHowToShutdownServer;
//        MsgServerProcessId = $"{UiMessages.MsgServerProcessId} {Process.GetCurrentProcess().Id}";
//    }

//    /// <summary>
//    /// Load the logo
//    /// </summary>
//    /// <param name="assembly">Assembly to load the logo from</param>
//    /// <param name="ressourcePath">Ressource path</param>
//    public void LoadLogo(Assembly assembly, string ressourcePath)
//    {
//        //try
//        //{

//        if (assembly is null)
//        {
//            return;
//        }

//        var logoStream = assembly.GetManifestResourceStream(ressourcePath);

//        if (logoStream is null)
//        {
//            return;
//        }

//        logoStream.Position = 0;

//        Logo = new BitmapImage();

//        // BitmapImage.UriSource must be in a BeginInit/EndInit block
//        Logo.BeginInit();
//        Logo.CacheOption = BitmapCacheOption.OnLoad;
//        Logo.StreamSource = logoStream;

//        // To save significant application memory, set the DecodePixelWidth or
//        // DecodePixelHeight of the BitmapImage value of the image source to the desired
//        // height or width of the rendered image. If you don't do this, the application will
//        // cache the image as though it were rendered as its normal size rather than just
//        // the size that is displayed.
//        // Note: In order to preserve aspect ratio, set DecodePixelWidth
//        // or DecodePixelHeight but not both.
//        Logo.DecodePixelHeight = 300;
//        Logo.EndInit();

//        //}
//        //catch
//        //{
//        //    // Do nothing
//        //}
//    }

//    /// <summary>
//    /// Shutdown for app
//    /// </summary>
//    public void ShutDown()
//    {
//        AppBuilder?.StopApplication();

//        _dispatcherTimer?.Stop();

//        Environment.Exit(0);
//    }

//    ///// <summary>
//    ///// Show a notification
//    ///// </summary>
//    ///// <param name="notification">Notification to show</param>
//    //public void ShowNotification(NotificationData notification)
//    //{
//    //    Notification = notification;
//    //}

//    /// <summary>
//    /// Minimize the app to the tray icon
//    /// </summary>
//    public bool MinimizeToTray
//    {
//        get => _minimizeToTray;
//        set => this.RaiseAndSetIfChanged(ref _minimizeToTray, value);
//    }

//    /// <summary>
//    /// Check if there are new log entries
//    /// </summary>
//    public void CheckLogs()
//    {

//        if (_listener is null)
//        {
//            return;
//        }

//        var count = _listener.Messages.Count;

//        if (count == 0)
//        {
//            return;
//        }

//        // Keep maximum log data length equal to MaxNumberOfLogEntries
//        if (_logData.Count > 0 && _logData.Count + count > MaxNumberOfLogEntries)
//        {
//            for (var i = _logData.Count - MaxNumberOfLogEntries - 2; i >= 0; i--)
//            {
//                _logData.Remove(_logData[i]);
//            }
//        }

//        // Add the received messages to log data
//        for (var i = 0; i < count; i++)
//        {
//            var logMsg = GeneralHelper.DequeueFromQueue(_listener.Messages);

//            if (_logData.Count > MaxNumberOfLogEntries)
//            {
//                continue;
//            }

//            _logData.Add(logMsg);
//        }

//        // If there are to many entries
//        for (var i = _logData.Count - MaxNumberOfLogEntries - 2; i >= 0; i--)
//        {
//            _logData.Remove(_logData[i]);
//        }

//        var doc = new FlowDocument
//        {
//            FontFamily = SystemFonts.StatusFontFamily,
//            FontSize = 14,
//            PageWidth = 1000,
//            ColumnWidth = 1000,
//            IsOptimalParagraphEnabled = true,
//            IsHyphenationEnabled = true
//        };

//        var data = _logData.ToList();

//        var isActive = false;

//        for (var index = data.Count - 1; index >= 0; index--)
//        {
//            var message = data[index];
//            var myParagraph = new Paragraph
//            {
//                Margin = _margin,
//                Padding = _padding,
//                Background = isActive ? _brush : _brush1
//            };

//            isActive = !isActive;

//            myParagraph.Inlines.Add(message);
//            doc.Blocks.Add(myParagraph);
//        }

//        _flowDoc = ObservableAsPropertyHelper<FlowDocument>.Default(doc);
//    }

//    private ObservableAsPropertyHelper<FlowDocument> _flowDoc;


//    /// <summary>
//    /// Log data as FlowDocument to show on UI
//    /// </summary>
//    public FlowDocument LogData => _flowDoc.Value;

//    /// <summary>
//    /// Event level
//    /// </summary>
//    public EventLevel LogEventLevel
//    {
//        get => _logEventLevel;
//        set
//        {
//            if (value == _logEventLevel || _listener is null)
//            {
//                return;
//            }
//            _logEventLevel = value;
//            _listener.EventLevel = _logEventLevel;
//            this.RaiseAndSetIfChanged(ref _logEventLevel, value);
//        }
//    }

//    /// <summary>
//    /// The logo to use for the user interface
//    /// </summary>
//    public BitmapImage? Logo
//    {
//        get => _logo;
//        private set => this.RaiseAndSetIfChanged(ref _logo, value);
//    }

//    /// <summary>
//    /// Background color of the header line
//    /// </summary>
//    public Color HeaderBackColor
//    {
//        get => _headerBackColor;
//        set => this.RaiseAndSetIfChanged(ref _headerBackColor, value);
//    }

//    /// <summary>
//    /// Background color of the form body
//    /// </summary>
//    public Color BodyBackColor
//    {
//        get => _bodyBackColor;
//        set => this.RaiseAndSetIfChanged(ref _bodyBackColor, value);
//    }

//    /// <summary>
//    /// Create the main form of the application
//    /// </summary>
//    /// <returns></returns>
//    public virtual Window CreateWindow()
//    {
//        var w = new MainWindow
//        {
//            WindowState = WindowState.Normal,
//            Visibility = Visibility.Visible
//        };
//        w.InjectViewModel(this);
//        return w;
//    }

//    /// <summary>
//    /// Start the event listener
//    /// </summary>
//    public void StartEventListener()
//    {
//        _dispatcherTimer = new DispatcherTimer();
//        _dispatcherTimer.Tick += dispatcherTimer_Tick!;
//        _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
//        _dispatcherTimer.Start();
//    }

//    private void dispatcherTimer_Tick(object sender, EventArgs e)
//    {
//        _dispatcherTimer?.Stop();

//        try
//        {
//            CheckLogs();
//        }
//        catch //(Exception exception)
//        {
//            // Do nothing
//        }


//        //LogWindow.SelectionStart = LogWindow.Text.Length;
//        //LogWindow.SelectionLength = 0;

//        _dispatcherTimer?.Start();
//    }


//    /// <summary>Gets the Router associated with this Screen.</summary>
//    public RoutingState Router { get; set; } = new();
//}