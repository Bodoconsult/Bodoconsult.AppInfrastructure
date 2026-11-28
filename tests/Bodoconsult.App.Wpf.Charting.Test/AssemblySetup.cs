// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Wpf.Services;

namespace Bodoconsult.App.Wpf.Charting.Test;

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
        DispatcherService.OpenDispatcher();
    }

    /// <summary>
    /// At startup of the assembly
    /// </summary>
    [OneTimeTearDown]
    public static void AssemblyTearDown()
    {
        DispatcherService.DisposeDispatcher();
    }
}