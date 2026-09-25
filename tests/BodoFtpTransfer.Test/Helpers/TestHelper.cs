using System.IO;
using System.Reflection;
using BodoFtpTransferCore.Business.Helpers;
using BodoFtpTransferCore.Business.Model;

namespace BodoFtpTransferCore.Business.Test.Helpers
{
    public static class TestHelper
    {

        public static readonly string TestDataPath;

        static TestHelper()
        {
            var path = (new FileInfo(Assembly.GetExecutingAssembly().Location)).Directory.Parent.Parent.Parent.FullName;

            TestDataPath = Path.Combine(path, "TestData");
        }

        /// <summary>
        /// Output path for test results
        /// </summary>
        public const string OutputPath = @"D:\temp";


        public const string DatabaseFileName = @"BodoFtpTransfer.sqlite";

        /// <summary>
        /// Get a text from a embedded resource file
        /// </summary>
        /// <param name="resourceName">resource name = file name</param>
        /// <returns></returns>
        public static string GetTextResource(string resourceName)
        {
            var ass = Assembly.GetExecutingAssembly();
            var str = ass.GetManifestResourceStream(resourceName);

            if (str == null) return null;

            string s;

            using (var file = new StreamReader(str))
            {
                s = file.ReadToEnd();
            }

            return s;
        }


        public static AppSettings GetAppsettings()
        {
            var path = @"D:\Daten\Projekte\_work\Data\BodoFtpTransfer.json";

            var appSettings = JsonHelper.LoadJsonFile<AppSettings>(path);

            return appSettings;
        }


        //public const string FtpSubDir = "/Test";

        //public const string FtpSubDirCreate = "/Test/AAA";


        //public const string FtpTestFileName = "A.txt";


        //public static string FtpTestFilePath => Path.Combine(TestDataPath, FtpTestFileName);


        //public const string LocalTargetPath = @"D:\temp";


        //public string LocalTestFile => 


    }
}
