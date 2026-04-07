// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.WinForms.Interfaces;
using WinFormsApp1.App;

// ReSharper disable LocalizableElement

namespace WinFormsApp1;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {

        Debug.Print("Hello, World!");

        Console.WriteLine("WinFormsApp1 initiation starts...");

        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForUiApp();

        // Set additional app start parameters as required. Take some settings from appsettings.json here
        var param = globals.AppStartParameter;
        //param.AppName = "WinFormsApp1: Demo app"; // from appsettings.json
        param.SoftwareTeam = "Robert Leisner";
        param.LogoRessourcePath = "WinFormsApp1.Resources.logo.jpg";
        //param.AppFolderName = "WinFormsApp1"; // from appsettings.json

        const string performanceToken = "--PERF";

        if (args.Contains(performanceToken))
        {
            param.IsPerformanceLoggingActivated = true;
        }

        // Now start app buiding process
        var builder = new WinFormsApp1AppBuilder(Globals.Instance);
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
        var viewModel = Globals.Instance.DiContainer.Get<IMainWindowViewModel>();

        //var eventLevel = EventLevel.Warning;
        //var listener = new AppEventListener(eventLevel);
        //var viewModel = new WinFormsApp1MainWindowViewModel(listener, null);

        // Set the view model 
        builder.MainWindowViewModel = viewModel;

        // Now finally start the app and wait
        builder.StartApplication();

        Environment.Exit(0);
    }
}