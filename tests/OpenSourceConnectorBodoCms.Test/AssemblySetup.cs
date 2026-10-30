// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Extensions;
using NUnit.Framework;
using OpenSourceConnectorBodoCms.Business.App;
using OpenSourceConnectorBodoCms.Business.Interfaces;
using OpenSourceConnectorBodoCms.Test.App;
using System;
using System.Diagnostics;

namespace OpenSourceConnectorBodoCms.Test;

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
        globals.StatusMessageDelegate = ShowState;
        globals.LoggingConfig.AddDefaultLoggerProviderConfiguratorsForUiApp();

        // Set additional app start parameters as required
        var param = globals.AppStartParameter;
        param.AppName = "OpenSourceConnectorBodoCmsTests";
        param.SoftwareTeam = "Robert Leisner";
        //param.LogoRessourcePath = "WinFormsConsoleApp1.Resources.logo.jpg";
        param.AppFolderName = "OpenSourceConnectorBodoCmsTests";

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

    private static void ShowState(string message)
    {
        Debug.Print(message);
    }
}