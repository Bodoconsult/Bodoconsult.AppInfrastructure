// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BodoWebReporting.Dal;
using NUnit.Framework;

namespace BodoWebReporting.Test;

/// <summary>
/// Test app
/// </summary>
[SetUpFixture]
public class AppSetup
{
    /// <summary>
    /// Load initial data for testing
    /// </summary>
    [OneTimeSetUp]
    public void LoadApp()
    {
        GlobalValues.LoadAppSettings();

        if (!Directory.Exists(GlobalValues.CurrentAppSettings.TargetPath))
            Directory.CreateDirectory(GlobalValues.CurrentAppSettings.TargetPath);
    }


    ///// <summary>
    ///// Unlooad app
    ///// </summary>
    //[OneTimeTearDown]
    //public void UnloadApp()
    //{

    //}

}