// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Interfaces;
using Bodoconsult.App.Windows.ConsoleTools;
using EmergencyShutdown.Business.App;
using EmergencyShutdown.Business.Interfaces;
using System.Runtime.Versioning;
using EmergencyShutdown.Business.Helpers;

namespace EmergencyShutdown;

/// <summary>
/// Console starter for EmergencyShutdown
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class EmergencyShutdownConsoleStarter : BaseConsoleStarter
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="args">Current command line args</param>
    /// <param name="isTestEnvironment">Is running in test environment</param>
    public EmergencyShutdownConsoleStarter(string[] args, bool isTestEnvironment = false) : base($"EmergencyShutdown{(isTestEnvironment ? "Test" : string.Empty)}", args)
    {
        MainAppRunnerDelegate = StartInternal;

        ArgumentNullException.ThrowIfNull(AppPath);
        CredentialsImportFileName = Path.Combine(AppPath, "cred.json");

        ArgumentNullException.ThrowIfNull(DataPath);
        CredentialsFileName = Path.Combine(DataPath, "data2.dat");
    }

    /// <summary>
    /// Current workload
    /// </summary>
    public Workload? Workload { get; private set; }

    /// <summary>
    /// Filename of the workload storage file
    /// </summary>
    public string CredentialsFileName { get; }

    /// <summary>
    /// Filename of the workload storage import file. To be deleted after import
    /// </summary>
    public string CredentialsImportFileName { get; }

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
        var globals = (IEmergencyShutdownAppGlobals)Globals.Instance;
        globals.Workload = Workload;
        globals.DataProtectionService = consoleStarter.DataProtectionService;

        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForConsoleApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.SoftwareTeam = "Robert Leisner";

        // Now start app buiding process
        IAppBuilder builder = new EmergencyShutdownAppBuilder(globals);
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

    /// <summary>
    /// Load the workload
    /// </summary>
    public void LoadWorkload()
    {
        Workload = LoadCredentials<Workload>("Workload", CredentialsFileName, CredentialsImportFileName);
    }

    /// <summary>
    /// Load an entity from encrypted storage. Uses Newtonsoft.Json to support interfaces
    /// </summary>
    /// <param name="key">Key to use</param>
    /// <param name="fileName">Target filename</param>
    /// <param name="importFileName">Name of the import file</param>
    public override T? LoadCredentials<T>(string key, string fileName, string importFileName) where T : default
    {
        if (File.Exists(fileName))
        {
            var crypted = File.ReadAllText(fileName);

            var json = DataProtectionService.Unprotect(key, crypted);

            return JsonHelper.LoadJsonString<T>(json);
        }
        else
        {
            if (!File.Exists(importFileName))
            {
                return default;
            }

            var json = File.ReadAllText(importFileName);

            var result = JsonHelper.LoadJsonString<T>(json);

            SaveCredentials(result, key, fileName);

            FileSystemHelper.DeleteFile(importFileName);

            return result;
        }
    }

    /// <summary>
    /// Save an entity to encrypted storage. Uses Newtonsoft.Json to support interfaces
    /// </summary>
    /// <param name="entity">Entity to save</param>
    /// <param name="key">Key to use</param>
    /// <param name="fileName">Target filename</param>
    public override void SaveCredentials<T>(T entity, string key, string fileName)
    {
        var json = JsonHelper.Serialize(entity);

        var crypted = DataProtectionService.Protect(key, json);

        File.WriteAllText(fileName, crypted);
    }
}