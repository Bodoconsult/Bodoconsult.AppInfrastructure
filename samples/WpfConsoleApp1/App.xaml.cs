// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Windows;
using System.Windows.Media;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Logging;
using Bodoconsult.App.Wpf.AppStarter.ViewModels;
using WpfConsoleApp1.AppData;

// ReSharper disable LocalizableElement

namespace WpfConsoleApp1;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
    /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
    protected override void OnStartup(StartupEventArgs e)
    {

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
        builder.LoadBasicSettings();

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
        var viewModel = new MainWindowViewModel(listener, null)
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

    }
}