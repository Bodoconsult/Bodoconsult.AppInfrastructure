Bodoconsult.App.Avalonia
===========================

# Overview

## What does the library

Bodoconsult.App.Avalonia is a library with basic functionality for multilayered monolithic Avalonia based applications. 

## How to use the library

The source code contains NUnit test classes the following source code is extracted from. The samples below show the most helpful use cases for the library.

# App start infrastructure basics

See page [app start infrastructure](../Bodoconsult.App/AppStartInfrastructure.md) for details.

## Using app start infrastructure for a classical Avalonia based apps

Here a sample from app.axaml.cs OnFrameworkInitializationCompleted() how to setup the Avalonia app with a main window MainWindow in project AvaloniaApp1 contained in this repo:

``` csharp
base.OnFrameworkInitializationCompleted();

var type = typeof(App);

var globals = Globals.Instance;
globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForUiApp();

// Set additional app start parameters as required
var param = globals.AppStartParameter;
param.AppName = "AvaloniaApp1: Demo app";
param.SoftwareTeam = "Robert Leisner";
param.LogoRessourcePath = "AvaloniaApp1.Resources.logo.jpg";
param.AppFolderName = "AvaloniaApp1";

const string performanceToken = "--PERF";

if (args!=null && args.Contains(performanceToken))
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

// Write first log entry with default logger
Globals.Instance.Logger.LogInformation($"{param.AppName} {param.AppVersion} starts...");
Console.WriteLine("Logging started...");

// App is ready now for doing something
Console.WriteLine($"Connection string loaded: {param.DefaultConnectionString}");

Console.WriteLine(string.Empty);
Console.WriteLine(string.Empty);

Console.WriteLine($"App name loaded: {param.AppName}");
Console.WriteLine($"App version loaded: {param.AppVersion}");
Console.WriteLine($"App path loaded: {param.AppPath}");

Console.WriteLine(string.Empty);
Console.WriteLine(string.Empty);

Console.WriteLine($"Logging config: {ObjectHelper.GetObjectPropertiesAsString(Globals.Instance.LoggingConfig)}");

// Prepare the DI container package
builder.LoadDiContainerServiceProviderPackage();
builder.RegisterDiServices();
builder.FinalizeDiContainerSetup();

// Create the viewmodel now
MainWindowViewModel = Globals.Instance.DiContainer.Get<IMainWindowViewModel>();
MainWindowViewModel.HeaderBackColor = Colors.DarkBlue;
MainWindowViewModel.BodyBackColor = Colors.Beige;
MainWindowViewModel.AppExe = param.AppExe;

//var eventLevel = EventLevel.Warning;
//var listener = new AppEventListener(eventLevel);

//MainWindowViewModel = new AvaloniaApp1MainWindowViewModel(listener)
//{
//    HeaderBackColor = Colors.DarkBlue,
//    BodyBackColor = Colors.Beige,
//    AppExe = param.AppExe
//};

DataContext = MainWindowViewModel;

// Load the logo now
MainWindowViewModel.LoadLogo(type.Assembly, param.LogoRessourcePath);

// Set the view model 
builder.MainWindowViewModel = MainWindowViewModel;

// Now finally start the app and wait
builder.StartApplication();
```

Start by creating your own viewmodel class AvaloniaApp1MainWindowViewModel derived from MainWindowViewModel and override CreateWindow() method to load your custom start window.

## Using app start infrastructure for a console app employing a Avalonia dispatcher

If you want to use Avalonia features in a console app you need to employ a Avalonia dispatcher. 
In a classical Avalonia app starting the Avalonia dispatcher is done by the App.xaml start process internally. 
In a console app you do not have a App.xaml so the disptacher has to be started separately.
Using Bodoconsult.App.Avalonia you can handles this easily.

Here a sample from app.axaml.cs OnFrameworkInitializationCompleted() how to setup the console app employing a Avalonia dispatcher in project ConsoleAvaloniaApp1 contained in this repo:

``` csharp

```

Start by creating your own app builder class similar to ConsoleWpfApp1AppBuilder class based on BaseConsoleWpfAppBuilderl.

## Using app start infrastructure for a OS service like Avalonia based apps

If you want to implement a OS service like application without implementing a real OS service you can use AvaloniaStarterUi class. 

By default AvaloniaStarterUi class loads the app as a simple window not shown on Windows task bar but task tray. The app can be closed only with keys STRC + C / CTRL + C or from task tray.

Here a sample from App.Xaml.cs OnStartup() how to setup the Avalonia console app in project AvaloniaConsoleApp1 contained in this repo:

``` csharp
base.OnFrameworkInitializationCompleted();

var type = typeof(App);

Console.WriteLine("AvaloniaConsoleApp1 initiation starts...");

var globals = Globals.Instance;
globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForUiApp();

// Set additional app start parameters as required
var param = globals.AppStartParameter;
param.AppName = "AvaloniaConsoleApp1: Demo app";
param.SoftwareTeam = "Robert Leisner";
param.LogoRessourcePath = "AvaloniaConsoleApp1.Resources.logo.jpg";
param.AppFolderName = "AvaloniaConsoleApp1";

const string performanceToken = "--PERF";

if (args!=null && args.Contains(performanceToken))
{
    param.IsPerformanceLoggingActivated = true;
}

// Now start app buiding process
var builder = new AvaloniaConsoleApp1AppBuilder(globals);
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

// App is ready now for doing something
Console.WriteLine($"Connection string loaded: {param.DefaultConnectionString}");

Console.WriteLine(string.Empty);
Console.WriteLine(string.Empty);

Console.WriteLine($"App name loaded: {param.AppName}");
Console.WriteLine($"App version loaded: {param.AppVersion}");
Console.WriteLine($"App path loaded: {param.AppPath}");

Console.WriteLine(string.Empty);
Console.WriteLine(string.Empty);

Console.WriteLine($"Logging config: {ObjectHelper.GetObjectPropertiesAsString(Globals.Instance.LoggingConfig)}");

// Prepare the DI container package
builder.LoadDiContainerServiceProviderPackage();
builder.RegisterDiServices();
builder.FinalizeDiContainerSetup();

// Create the viewmodel now
var eventLevel = EventLevel.Warning;
var listener = new AppEventListener(eventLevel);
MainWindowViewModel = new MainWindowViewModel(listener, null)
{
    HeaderBackColor = Colors.DarkBlue,
    BodyBackColor = Colors.Beige,
    AppExe = param.AppExe
};
DataContext = MainWindowViewModel;

// Load the logo now
MainWindowViewModel.LoadLogo(type.Assembly, param.LogoRessourcePath);

// Set the view model 
builder.MainWindowViewModel = MainWindowViewModel;

// Now finally start the app and wait
builder.StartApplication();
```

# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

