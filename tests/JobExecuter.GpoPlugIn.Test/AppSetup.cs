// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Dal.App;
using NUnit.Framework;

namespace JobExecuter.GpoPlugIn.Test;

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

    }


    ///// <summary>
    ///// Unlooad app
    ///// </summary>
    //[OneTimeTearDown]
    //public void UnloadApp()
    //{

    //}

}