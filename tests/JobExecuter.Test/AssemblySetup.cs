// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Test.App;
using NUnit.Framework;
using System;
using Bodoconsult.App.Extensions;

namespace JobExecuter.Test;

/// <summary>
/// Setup for the assembly for all tests
/// </summary>
[SetUpFixture]
public static class AssemblySetup
{
    /// <summary>
    /// At startup of the assembly
    /// </summary>
    [OneTimeSetUp]
    public static void AssemblyStartUp()
    {
        var globals = Globals.Instance;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForUiApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.AppName = "JobExecuter.Test";
        param.SoftwareTeam = "Robert Leisner";
        //param.LogoRessourcePath = "WinFormsConsoleApp1.Resources.logo.jpg";
        param.AppFolderName = "JobExecuter.Test";

        // Now start the app building process
        var builder = new MyDebugAppBuilder(globals);
#if !DEBUG
        AppDomain.CurrentDomain.UnhandledException += builder.CurrentDomainOnUnhandledException;
#endif

        // Load basic app metadata
        builder.LoadBasicSettings();

        // Process the config file
        builder.ProcessConfiguration();

        // Now load the globally needed settings
        builder.LoadGlobalSettings();

        ArgumentNullException.ThrowIfNull(globals.Logger);
        globals.Logger.LogInformation("Starting tests...");
    }
}