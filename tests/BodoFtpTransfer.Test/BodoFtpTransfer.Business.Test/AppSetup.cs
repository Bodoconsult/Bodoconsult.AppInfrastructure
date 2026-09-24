using BodoFtpTransferCore.Business.App;
using BodoFtpTransferCore.Business.Test.Helpers;
using NUnit.Framework;

namespace BodoFtpTransferCore.Business.Test
{
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
            GlobalValues.LoadAppSettings(TestHelper.GetAppsettings());
        }


        ///// <summary>
        ///// Unlooad app
        ///// </summary>
        //[OneTimeTearDown]
        //public void UnloadApp()
        //{

        //}

    }
}