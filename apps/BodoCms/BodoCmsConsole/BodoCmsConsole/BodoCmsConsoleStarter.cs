// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Runtime.Versioning;
using BodoCmsConsole.Business.App;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Interfaces;
using Bodoconsult.App.Windows.ConsoleTools;

namespace BodoCmsConsole;

/// <summary>
/// Console starter for BodoCms
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class BodoCmsConsoleStarter : BaseConsoleStarter
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="args">Current command line args</param>
    /// <param name="isTestEnvironment">Is running in test environment</param>
    public BodoCmsConsoleStarter(string[] args, bool isTestEnvironment = false) : base($"BodoCms{(isTestEnvironment ? "Test" : string.Empty)}", args)
    {
        MainAppRunnerDelegate = StartInternal;
    }

    private int StartInternal(IConsoleStarter consoleStarter)
    {
        //if (Args is null || Args.Length == 0)
        //{
        return RunMainTask(consoleStarter);
        //}

        //switch (Args[0])
        //{
        //    case "0":
        //        return RunMainTask(consoleStarter);
        //    case "/p": // Encrypt mail account data in smtp.json or o365.json to use it in settings table in the database 
        //    case "--p":
        //        AskForPassword();
        //        return 0;
        //    default:
        //        return 0;
        //}
    }

    /// <summary>
    /// Run the main application mask
    /// </summary>
    /// <returns>Error code or 0 in case of successful</returns>
#pragma warning disable CA1822
    public int RunMainTask(IConsoleStarter consoleStarter)
#pragma warning restore CA1822
    {
        var globals = Globals.Instance;
        globals.StatusMessageDelegate = SetStatus;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForConsoleApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.SoftwareTeam = "Robert Leisner";

        // Now start app buiding process
        IAppBuilder builder = new BodoCmsAppBuilder(globals);
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
        globals.StatusMessageDelegate = SetStatus;

        Console.WriteLine("Logging started...");

        // App is ready now for doing something
        Console.WriteLine($"Connection string loaded: {param.DefaultConnectionString}");

        // Prepare the DI container package
        builder.LoadDiContainerServiceProviderPackage();
        builder.RegisterDiServices();
        builder.FinalizeDiContainerSetup();

        // Now finally start the app and wait
        builder.StartApplication();

        return 0;
    }
}