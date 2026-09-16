// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using BodoWebMailer.Business.App;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using Bodoconsult.App.Extensions;

namespace BodoWebMailer;

[SupportedOSPlatform("windows10.0.17763.0")]
internal class Program
{
    private static int Main(string[] args)
    {
        if (args.Length > 0)
        {
            switch (args[0])
            {
                case "/p": // Ask user for token, encrypt it and copy it to clipboard
                    ConsoleHelper.EncryptToken();
                    return 0;
                default:
                    break;
            }
        }

        Debug.Print("Hello, World!");

        Console.WriteLine("ConsoleApp1 initiation starts...");

        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForConsoleApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.SoftwareTeam = "Robert Leisner";

        const string performanceToken = "--PERF";

        if (args.Contains(performanceToken))
        {
            param.IsPerformanceLoggingActivated = true;
        }

        // Now start app buiding process
        IAppBuilder builder = new BodoWebMailerAppBuilder(globals);
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
        ArgumentNullException.ThrowIfNull(globals.Logger);
        globals.Logger.LogInformation($"{param.AppName} {param.AppVersion} starts...");
        globals.StatusMessageDelegate = ShowStatus;

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
        return 0;
    }

    internal static void ShowStatus(string message)
    {
        Console.WriteLine(message);
        //Logger.Info(message);
    }
}