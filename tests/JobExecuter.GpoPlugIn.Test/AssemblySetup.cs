// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Dal.App;
using NUnit.Framework;

namespace JobExecuter.GpoPlugIn.Test;

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
       // GlobalValues.LoadAppSettings();
    }

    ///// <summary>
    ///// Unlooad app
    ///// </summary>
    //[OneTimeTearDown]
    //public void UnloadApp()
    //{

    //}
}