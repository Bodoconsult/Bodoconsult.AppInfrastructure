// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using Avalonia;
using NUnit.Framework;
using Splat;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test;

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
        //DispatcherService.OpenDispatcher();
    }

    [OneTimeTearDown]
    public static void AssemblyTearDown()
    {
        //DispatcherService.OpenDispatcher();
    }

    //public static AppBuilder BuildAvaloniaApp()
    //{
    //    //Bootstrapper.RegisterAsync(Locator.CurrentMutable, Locator.Current).Wait();
    //    return AppBuilder.Configure<App>()
    //        .UsePlatformDetect()
    //        .WithInterFont()!
    //        .LogToTrace()
    //        .UseReactiveUI();
    //}
}