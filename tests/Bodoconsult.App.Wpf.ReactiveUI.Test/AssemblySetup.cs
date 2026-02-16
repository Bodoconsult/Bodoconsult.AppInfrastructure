// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using System.Windows;
using Bodoconsult.App.Wpf.Services;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.ReactiveUI.Test;

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
        DispatcherService.OpenDispatcher();

        var x = Application.Current.Dispatcher;
    }

    [OneTimeTearDown]
    public static void AssemblyTearDown()
    {
        DispatcherService.OpenDispatcher();
    }
}