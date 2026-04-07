// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;
using ConsoleWpfApp1.App;

namespace ConsoleWpfApp1;

internal static class Program
{

    private static void Main(string[] args)
    {

        Debug.Print("Hello, World!");

        Console.WriteLine("ConsoleWpfApp1 initiation starts...");

        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForConsoleApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.AppName = "ConsoleWpfApp1: Demo app";
        param.SoftwareTeam = "Robert Leisner";
        param.LogoRessourcePath = "ConsoleWpfApp1.Resources.logo.jpg";
        param.AppFolderName = "ConsoleWpfApp1";

        const string performanceToken = "--PERF";

        if (args.Contains(performanceToken))
        {
            param.IsPerformanceLoggingActivated = true;
        }

        // Now start app buiding process
        IAppBuilder builder = new ConsoleWpfApp1AppBuilder(globals);
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

        // Now finally start the app and wait
        builder.StartApplication();

        Environment.Exit(0);
    }
}