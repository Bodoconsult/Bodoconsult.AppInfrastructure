// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using NUnit.Framework;

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
}