// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;
using GrpcServerApp.Grpc.App;

namespace GrpcServerApp;

public static class Program
{
    private static GrpcServerAppAppBuilder _builder;

    public static void Main(string[] args)
    {

        Debug.Print("Hello, World!");

        Debug.Print("GrpcServerApp initiation starts...");

        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForBackgroundServiceApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.AppName = "GrpcServerApp: Demo app";
        param.SoftwareTeam = "Robert Leisner";
        param.LogoRessourcePath = "GrpcServerApp.Resources.logo.jpg";
        param.AppFolderName = "GrpcServerApp";

        const string performanceToken = "--PERF";

        if (args.Contains(performanceToken))
        {
            param.IsPerformanceLoggingActivated = true;
        }

        // Now start app buiding process
        _builder = new GrpcServerAppAppBuilder(globals, args);

#if !DEBUG
        AppDomain.CurrentDomain.UnhandledException += _builder.CurrentDomainOnUnhandledException;
#endif

        // Load basic app metadata
        _builder.LoadBasicSettings(typeof(Program));

        // Process the config file
        _builder.ProcessConfiguration();

        // Now load the globally needed settings
        _builder.LoadGlobalSettings();

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
        _builder.RegisterGrpcDiServices();
        _builder.LoadDiContainerServiceProviderPackage();
        _builder.RegisterDiServices();

        // Now configure GRPC IP, port, protocol
        _builder.ConfigureGrpc();

        // Proto services load in GrpcServerAppAppBuilder.RegisterProtoServices()

        // builder.FinalizeDiContainerSetup(); Do not call this method for a background service. It is too early for it

        // Now finally start the app and wait
        _builder.StartApplication();

        Environment.Exit(0);
    }

    /// <summary>
    /// Shutdown the app (intended for testing)
    /// </summary>
    public static void Shutdown()
    {
        _builder?.StopApplication();
    }

}







