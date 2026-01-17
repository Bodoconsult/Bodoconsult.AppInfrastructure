Bodoconsult.App.Wpf
===========================

# Overview

>   [Using app start infrastructure for a classical Wpf based apps](#using-app-start-infrastructure-for-a-classical-wpf-based-apps)

>   [Using app start infrastructure for a console app employing a WPF dispatcher](#using-app-start-infrastructure-for-a-os-service-like-wpf-based-apps)

>   [Using app start infrastructure for a OS service like WPF based apps](#using-app-start-infrastructure-for-a-console-app-employing-a-wpf-dispatcher)

>   [Other tools](#other-tools)

>   >   [SharedResourceDictionary](#sharedresourcedictionary)

>   >   [Converters](#converters)

>   >   [Sending TOAST messages from WPF app](#sending-toast-messages-from-wpf-app)

>   >   [Starting WPD Dispatcher from console apps: DispatcherService class](#starting-wpd-dispatcher-from-console-apps-dispatcherservice-class)

## What does the library

Bodoconsult.App.Wpf is a library with basic functionality for multilayered monolithic WPF based applications. 

## App start infrastructure basics

See page [app start infrastructure](../Bodoconsult.App/AppStartInfrastructure.md) for details.

## How to use the library

The source code contains NUnit test classes the following source code is extracted from. The samples below show the most helpful use cases for the library.

# Using app start infrastructure for a classical Wpf based apps

Here a sample from Program.cs Main() how to setup the Wpf app with a main form Forms1 in project AvaloniaApp1 contained in this repo:

``` csharp
var type = typeof(App);

Debug.Print("Hello, World!");

Console.WriteLine("AvaloniaApp1 initiation starts...");

var globals = Globals.Instance;
globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForUiApp();

// Set additional app start parameters as required
var param = globals.AppStartParameter;
param.AppName = "AvaloniaApp1: Demo app";
param.SoftwareTeam = "Robert Leisner";
param.LogoRessourcePath = "AvaloniaApp1.Resources.logo.jpg";
param.AppFolderName = "AvaloniaApp1";

const string performanceToken = "--PERF";

if (e.Args.Contains(performanceToken))
{
    param.IsPerformanceLoggingActivated = true;
}

// Now start app buiding process
var builder = new AvaloniaApp1AppBuilder(globals);
#if !DEBUG
AppDomain.CurrentDomain.UnhandledException += builder.CurrentDomainOnUnhandledException;
#endif

// Load basic app metadata
builder.LoadBasicSettings(type);

// Process the config file
builder.ProcessConfiguration();

// Now load the globally needed settings
builder.LoadGlobalSettings();

// Prepare the DI container package
builder.LoadDiContainerServiceProviderPackage();
builder.RegisterDiServices();
builder.FinalizeDiContainerSetup();

// Create the viewmodel now
var eventLevel = EventLevel.Warning;
var listener = new AppEventListener(eventLevel);
var viewModel = new AvaloniaApp1MainWindowViewModel(listener)
{
    HeaderBackColor = Colors.DarkBlue,
    BodyBackColor = Colors.Beige,
    AppExe = param.AppExe
};

// Load the logo now
viewModel.LoadLogo(type.Assembly, param.LogoRessourcePath);

// Set the view model 
builder.MainWindowViewModel = viewModel;

// Now finally start the app and wait
builder.StartApplication();

base.OnStartup(e);
```

Start by creating your own viewmodel class AvaloniaApp1MainWindowViewModel derived from MainWindowViewModel and override CreateWindow() method to load your custom start window.

# Using app start infrastructure for a console app employing a WPF dispatcher

If you want to use WPF features like FlowDocuments in a console app you need to employ a WPF dispatcher. 
In a classical WPF app starting the WPF dispatcher is done by the App.xaml start process internally. 
In a console app you do not have a App.xaml so the disptacher has to be started separately.
Using Bodoconsult.App.Wpf you can handles this easily.

Here a sample from Program.cs Main() how to setup the console app employing a WPF dispatcher in project ConsoleAvaloniaApp1 contained in this repo:

``` csharp
var globals = Globals.Instance;
globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForConsoleApp();

// Set additional app start parameters as required
var param = globals.AppStartParameter;
param.AppName = "ConsoleAvaloniaApp1: Demo app";
param.SoftwareTeam = "Robert Leisner";
param.AppFolderName = "ConsoleAvaloniaApp1";

const string performanceToken = "--PERF";

if (args.Contains(performanceToken))
{
    param.IsPerformanceLoggingActivated = true;
}

// Now start app buiding process
IAppBuilder builder = new ConsoleAvaloniaApp1AppBuilder(globals);
#if !DEBUG
    AppDomain.CurrentDomain.UnhandledException += builder.CurrentDomainOnUnhandledException;
#endif

// Load basic app metadata
builder.LoadBasicSettings(typeof(Program));

// Process the config file
builder.ProcessConfiguration();

// Now load the globally needed settings
builder.LoadGlobalSettings();

// Write first log entry with default logger
Globals.Instance.Logger.LogInformation($"{param.AppName} {param.AppVersion} starts...");
Console.WriteLine("Logging started...");

// Prepare the DI container package
builder.LoadDiContainerServiceProviderPackage();
builder.RegisterDiServices();
builder.FinalizeDiContainerSetup();

// Now finally start the app and wait
builder.StartApplication();

Environment.Exit(0);
```

Start by creating your own app builder class similar to ConsoleAvaloniaApp1AppBuilder class based on BaseConsoleAvaloniaAppBuilderl.

# Using app start infrastructure for a OS service like WPF based apps

If you want to implement a OS service like application without implementing a real OS service you can use WpfStarterUi class. 

By default WpfStarterUi class loads the app as a simple window not shown on Windows task bar but task tray. The app can be closed only with keys STRC + C / CTRL + C or from task tray.

Here a sample from App.Xaml.cs OnStartup() how to setup the WPF console app in project WpfConsoleApp1 contained in this repo:

``` csharp
var type = typeof(App);

Debug.Print("Hello, World!");

Console.WriteLine("WpfConsoleApp1 initiation starts...");

var globals = Globals.Instance;
globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForUiApp();

// Set additional app start parameters as required
var param = globals.AppStartParameter;
param.AppName = "WpfConsoleApp1: Demo app";
param.SoftwareTeam = "Robert Leisner";
param.LogoRessourcePath = "WpfConsoleApp1.Resources.logo.jpg";
param.AppFolderName = "WpfConsoleApp1";

const string performanceToken = "--PERF";

if (e.Args.Contains(performanceToken))
{
    param.IsPerformanceLoggingActivated = true;
}

// Now start app buiding process
var builder = new WpfConsoleApp1AppBuilder(globals);
#if !DEBUG
AppDomain.CurrentDomain.UnhandledException += builder.CurrentDomainOnUnhandledException;
#endif

// Load basic app metadata
builder.LoadBasicSettings(type);

// Process the config file
builder.ProcessConfiguration();

// Now load the globally needed settings
builder.LoadGlobalSettings();

// Write first log entry with default logger
Globals.Instance.Logger.LogInformation($"{param.AppName} {param.AppVersion} starts...");
Console.WriteLine("Logging started...");

// Prepare the DI container package
builder.LoadDiContainerServiceProviderPackage();
builder.RegisterDiServices();
builder.FinalizeDiContainerSetup();

// Create the viewmodel now
var eventLevel = EventLevel.Warning;
var listener = new AppEventListener(eventLevel);
var viewModel = new MainWindowViewModel(listener)
{
    HeaderBackColor = Colors.Coral,
    AppExe = param.AppExe
};

// Load the logo now
viewModel.LoadLogo(type.Assembly, param.LogoRessourcePath);

// Set the view model 
builder.MainWindowViewModel = viewModel;

// Now finally start the app and wait
builder.StartApplication();

base.OnStartup(e);
```

# Other tools

## SharedResourceDictionary

Use SharedResourceDictionary class to avoid loading resources multiple times.

## Use I18N providers for using Bodoocnsult.I18N with WPF resources

For basic info how to use Bodoconsult.I18N and I18N providers see [Bodoconsult.I18N section](../Bodoconsult.I18N/README.md).

Use WpfEmbeddedResourceProvider and WpfFileLocalesProvider to add I18N resources from WPF to Bodoocnsult.I18N.

``` csharp
    private const string Path = "Locales";

    [Test]
    public void RegisterLocaleItems_ExistingResources_ResourceItemsLoaded()
    {
        // Arrange 
        var ass = TestHelper.CurrentAssembly;

        var provider = new WpfEmbeddedResourceProvider(ass, Path);

        // Act  
        provider.RegisterLocaleItems();

        // Assert
        Assert.That(provider.ResourceItems, Is.Not.Null);
        Assert.That(provider.LocaleItems.Count, Is.Not.EqualTo(0));
    }
```

## Converters

Bodoconsult.App.Wpf defines a set of converters based on IValueConverter interface:

-   ColorToBrushConverter

-   DateTimeConverter

-   DateTimeUiConverter

-   FlowDocumentContentToXamlConverter

-   FlowDocumentToXamlConverter

-   NullImageConverter

-   NumberConverter

-   PercentageConverter

-   

## Sending TOAST messages from WPF app

WpfToastMessagingService class implements sending of simple TOAST messages to the OS.

## Starting WPD Dispatcher from console apps: DispatcherService class

Use the DispatcherService class to start a WPF Dispatcher in a console environment to use WPF features like Flowdocument.

# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

