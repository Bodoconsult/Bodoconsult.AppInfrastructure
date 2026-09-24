using Bodoconsult.Core.Web.Ftp;
using BodoFtpTransferCore.Business.Helpers;
using BodoFtpTransferCore.Business.Model;
using BodoFtpTransferCore.Business.Test.Helpers;
using NUnit.Framework;

namespace BodoFtpTransferCore.Business.Test
{
    [TestFixture]
    public class UnitTestsTestHelper
    {
        //[SetUp]
        //public void Setup()
        //{
        //}

        [Test]
        public void TestCreateAppSettingsJson()
        {

            var appSettings = new AppSettings
            {
                BaseDirectory = TestHelper.TestDataPath,
                RemoteDirectory = "/Test",
                ExcludeDirs = "{logs}{Internet}",
                ExcludedFiles = "{thumbs.db}{robots.txt}{.htaccess}"
            };

            var c = new SshCredentials
            {
                Url = "www.test.de", // www.test.de
                Username = PasswordHelper.Encrypt("YourUserName"),
                Password = PasswordHelper.Encrypt("YourPassword"),
            };

            appSettings.Credentials = c;


            const string filename = @"D:\temp\appSettings.json";
            JsonHelper.SaveAsFile(filename, appSettings);
        }

        [Test]
        public void TestGetAppsettings()
        {

            var result = TestHelper.GetAppsettings();

            Assert.IsNotNull(result);

        }


        
    }
}