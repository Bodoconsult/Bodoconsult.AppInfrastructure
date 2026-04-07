// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Diagnostics.Tracing;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Logging;
using Bodoconsult.App.WinForms.AppStarter.Forms.ViewModel;
using WinFormsConsoleApp1.App;

// ReSharper disable LocalizableElement

namespace WinFormsConsoleApp1;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {

        Debug.Print("Hello, World!");

        Console.WriteLine("WinFormsConsoleApp1 initiation starts...");

        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForUiApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.AppName = "WinFormsConsoleApp1: Demo app";
        param.SoftwareTeam = "Robert Leisner";
        param.LogoRessourcePath = "WinFormsConsoleApp1.Resources.logo.jpg";
        param.AppFolderName = "WinFormsConsoleApp1";

        const string performanceToken = "--PERF";

        if (args.Contains(performanceToken))
        {
            param.IsPerformanceLoggingActivated = true;
        }

        // Now start app buiding process
        var builder = new WinFormsConsoleApp1AppBuilder(globals);
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

        // App is ready now for doing something
        Console.WriteLine($"Connection string loaded: {param.DefaultConnectionString}");

        Console.WriteLine("");
        Console.WriteLine("");

        Console.WriteLine($"App name loaded: {param.AppName}");
        Console.WriteLine($"App version loaded: {param.AppVersion}");
        Console.WriteLine($"App path loaded: {param.AppPath}");

        Console.WriteLine("");
        Console.WriteLine("");

        Console.WriteLine($"Logging config: {ObjectHelper.GetObjectPropertiesAsString(Globals.Instance.LoggingConfig)}");

        // Prepare the DI container package
        builder.LoadDiContainerServiceProviderPackage();
        builder.RegisterDiServices();
        builder.FinalizeDiContainerSetup();

        // Create the viewmodel now
        var eventLevel = EventLevel.Warning;
        var listener = new AppEventListener(eventLevel);
        var viewModel = new MainWindowViewModel(listener, null);

        // Set the view model 
        builder.MainWindowViewModel = viewModel;

        // Now finally start the app and wait
        builder.StartApplication();

        Environment.Exit(0);
    }
}