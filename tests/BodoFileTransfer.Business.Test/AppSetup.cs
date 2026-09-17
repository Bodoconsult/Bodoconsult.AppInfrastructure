using BodoFileTransferCore.Business.App;
using NUnit.Framework;

namespace BodoFileTransferCore.Business.Test;

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