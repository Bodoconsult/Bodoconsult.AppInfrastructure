// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Interfaces;
using Bodoconsult.App.Windows.ConsoleTools;
using BodoFtpTransfer.Business.App;
using System.Runtime.Versioning;
using Bodoconsult.App.Extensions;
using BodoFtpTransfer.Business.Interfaces;

namespace BodoFtpTransfer;

/// <summary>
/// Console starter for JobExecuter app
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class BodoFtpTransferConsoleStarter: BaseConsoleStarter
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="args">Command line args</param>
    public BodoFtpTransferConsoleStarter(string[] args): base("BodoFtpTransfer", args)
    {
        MainAppRunnerDelegate = StartInternal;
    }

    /// <summary>
    /// Do the requested start action
    /// </summary>
    public int StartInternal(IConsoleStarter consoleStarter)
    {
        var args0 = consoleStarter.Args == null || consoleStarter.Args.Length==0 ? string.Empty: consoleStarter.Args[0];

        // Command line arg is a filename
        if (!args0.StartsWith("/", StringComparison.OrdinalIgnoreCase) && !args0.StartsWith("--", StringComparison.OrdinalIgnoreCase))
        {
            return RunJobTask(args0);
        }

        // Run command line args
        switch (args0)
        {
            case "/p":
            case "--p":
                LoadWebCredentialsConfig();
                return 0;
            default:
                //ShowHelp();
                return 0;
        }
    }

    /// <summary>
    /// Run a job task
    /// </summary>
    /// <param name="cjsonFile">Filename of a CJSON file with the job task definition</param>
    /// <returns>Error code. If 0 job was executed without error</returns>
    public int RunJobTask(string cjsonFile)
    {
        var x = Globals.Instance;

        if (x is not IBodoFtpTransferAppGlobals globals)
        {
            throw new ArgumentException("x is not IBodoFtpTransferAppGlobals");
        }

        globals.Credentials = WebCredentials;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForConsoleApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.SoftwareTeam = "Robert Leisner";

        globals.AppStartParameter.Args = Args;

        // Now start app buiding process
        var builder = new BodoFtpTransferAppBuilder(globals);
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
        // See BodoFileTransferService.StartApplication() for the relevante parts of app start
        builder.StartApplication();
        return 0;
    }

    
}