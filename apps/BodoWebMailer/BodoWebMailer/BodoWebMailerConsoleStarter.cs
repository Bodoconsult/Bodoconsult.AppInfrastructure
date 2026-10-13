// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Interfaces;
using Bodoconsult.App.Windows.ConsoleTools;
using BodoWebMailer.Business.App;
using System;
using System.IO;
using System.Runtime.Versioning;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;
using Bodoconsult.Web.Mail.Helpers;
using BodoWebMailer.Business.Helpers;
using BodoWebMailer.Business.Interfaces;

namespace BodoWebMailer;

/// <summary>
/// Console starter for BodoWebMailer
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class BodoWebMailerConsoleStarter : BaseConsoleStarter
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="args">Current command line args</param>
    public BodoWebMailerConsoleStarter(string[] args) : base("BodoWebMailer", args)
    {
        MainAppRunnerDelegate = StartInternal;

        AppHelper.LoadPasswordData();
    }

    private int StartInternal(IConsoleStarter consoleStarter)
    {
        if (Args is null || Args.Length == 0)
        {
            return RunMainTask(consoleStarter);
        }

        switch (Args[0])
        {
            case "0":
                return RunMainTask(consoleStarter);
            case "/p": // Encrypt mail account data in smtp.json or o365.json to use it in settings table in the database 
            case "--p":
                EncryptMailAccountData();
                return 0;
            default:
                return 0;
        }
    }

    /// <summary>
    /// Encrypt mail account data in smtp.json or o365.json into file mail.json to use it in settings table in the database 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void EncryptMailAccountData()
    {
        ArgumentNullException.ThrowIfNull(AppPath);

        var fileName = Path.Combine(AppPath, "o365.json");

        var useSmtp = false;

        if (!File.Exists(fileName))
        {
            fileName = Path.Combine(AppPath, "smtp.json");
            if (!File.Exists(fileName))
            {
                Console.WriteLine("No file to encrypt found.");
                ConsoleHelper.WaitUntilAnyKeyPressed();
                return;
            }

            useSmtp = true;
        }

        var json = File.ReadAllText(fileName);

        var crypted = MailHelper.GetEncryptedAccountFromJson(json, useSmtp);

        

        var newfileName = Path.Combine(AppPath, "mail.json");

        if (useSmtp)
        {
            SimpleJsonHelper.SerializeAsFile(newfileName, (SmtpMailAccount)crypted);
        }
        else
        {
            SimpleJsonHelper.SerializeAsFile(newfileName, (O365MailAccount)crypted);
        }

        DeleteFile(fileName);

        ConsoleHelper.WaitUntilAnyKeyPressed("Created encrypted file mail.json!");
    }

    /// <summary>
    /// Run the main application mask
    /// </summary>
    /// <returns>Error code or 0 in case of successful</returns>
#pragma warning disable CA1822
    public int RunMainTask(IConsoleStarter consoleStarter)
#pragma warning restore CA1822
    {
        var globals = (IBodoWebMailerAppGlobals)Globals.Instance;
        globals.DataProtectionService = consoleStarter.DataProtectionService;

        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForConsoleApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.SoftwareTeam = "Robert Leisner";

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