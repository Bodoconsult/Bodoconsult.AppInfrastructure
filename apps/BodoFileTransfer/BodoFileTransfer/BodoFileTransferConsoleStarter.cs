// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Interfaces;
using Bodoconsult.App.Windows.ConsoleTools;
using Bodoconsult.App.Windows.System;
using Bodoconsult.Web.Mail.Helpers;
using BodoFileTransfer.Business.App;
using BodoFileTransfer.Business.Helpers;

namespace BodoFileTransfer;

/// <summary>
/// Console starter for BodoWebMailer
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class BodoFileTransferConsoleStarter : BaseConsoleStarter
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="args">Current command line args</param>
    public BodoFileTransferConsoleStarter(string[] args) : base("BodoFileTransfer", args)
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
            case "/c":
            case "--c":
                EncryptSingleValue();
                return 0;
            default:
                return 0;
        }
    }

    /// <summary>
    /// Encrypt a single value. Public only for unit testing
    /// </summary>
    public static void EncryptSingleValue()
    {
        var value = ConsoleHelper.AskUserForStringValue("Please insert value to encrypt for usage with BodoFileTransfer:");

        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        var crypted = PasswordHandler.Encrypt(value);
        
        Debug.Print(crypted);

        Clipboard.SetText(crypted);

        ConsoleHelper.WaitUntilAnyKeyPressed("Crypted value was copied to clipboard!");
    }

    /// <summary>
    /// Encrypt mail account data in smtp.json or o365.json into file mail.json to use it in settings table in the database. Public only for unit testing 
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

        var newfileName = Path.Combine(AppPath, "mail.txt");

        var s = new StringBuilder();

        if (useSmtp)
        {
            var x = (SmtpMailAccount)crypted;
            s.AppendLine($"Server: {x.SmtpServer}");
            s.AppendLine($"AccountName: {x.SmtpAccountName}");
            s.AppendLine($"Password: {x.SmtpPassword}");
            s.AppendLine($"MailAddressSender: {x.MailAddressSender}");
            s.AppendLine($"Port: {x.SmtpPort}");
            s.AppendLine($"UseSecureConnection: {x.UseSecureConnection}");
        }
        else
        {
            var x = (O365MailAccount)crypted;
            s.AppendLine($"Instance: {x.Instance}");
            s.AppendLine($"Scope: {x.Scope}");
            s.AppendLine($"Tenant: {x.Tenant}");
            s.AppendLine($"ClientId: {x.ClientId}");
            s.AppendLine($"ClientSecret: {x.ClientSecret}");
            s.AppendLine($"UserName: {x.UserName}");
            s.AppendLine($"MailAddressSender: {x.MailAddressSender}");
        }

        File.WriteAllText(newfileName, s.ToString());

        FileSystemHelper.DeleteFile(fileName);

        ConsoleHelper.WaitUntilAnyKeyPressed("Created encrypted file mail.txt!");
    }

    /// <summary>
    /// Run the main application mask
    /// </summary>
    /// <returns>Error code or 0 in case of successful</returns>
    public int RunMainTask(IConsoleStarter consoleStarter)
    {
        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForConsoleApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.SoftwareTeam = "Robert Leisner";

        globals.AppStartParameter.Args = Args;

        // Now start app buiding process
        IAppBuilder builder = new BodoFileTransferAppBuilder(globals);
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