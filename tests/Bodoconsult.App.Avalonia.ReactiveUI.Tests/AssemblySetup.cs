// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.ReactiveUI.Tests.App;

namespace Bodoconsult.App.ReactiveUI.Tests;

/// <summary>
/// Setup for the assembly for all tests
/// </summary>
[SupportedOSPlatform("windows")]
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
        param.AppName = "WinAppTests: Demo app";
        param.SoftwareTeam = "Robert Leisner";
        //param.LogoRessourcePath = "WinFormsConsoleApp1.Resources.logo.jpg";
        param.AppFolderName = "WinAppTests";


        // Now start the app building process
        var builder = new MyDebugAppBuilder(globals);
#if !DEBUG
        AppDomain.CurrentDomain.UnhandledException += builder.CurrentDomainOnUnhandledException;
#endif

        // Load basic app metadata

        builder.LoadBasicSettings(typeof(AssemblySetup));

        // Process the config file
        builder.ProcessConfiguration();

        // Now load the globally needed settings
        builder.LoadGlobalSettings();

        ArgumentNullException.ThrowIfNull(Globals.Instance.Logger);
        Globals.Instance.Logger.LogInformation("Starting tests...");

        // Start test app with ReactiveUI
        builder.StartApplication();

    }

    [OneTimeTearDown]
    public static void AssemblyTearDown()
    {
        //DispatcherService.OpenDispatcher();
    }
}