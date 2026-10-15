// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO;
using System.Reflection;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Web.Ftp.Test.Helpers;

public static class TestHelper
{

    private static readonly string TestDataPath;

    static TestHelper()
    {
        var path = (new FileInfo(Assembly.GetExecutingAssembly().Location)).Directory.Parent.Parent.Parent.FullName;

        TestDataPath = Path.Combine(path, "TestData");
    }


    public static WebCredentials GetCredentials()
    {
        var path = @"D:\Daten\Projekte\_work\Data\FtpCredentials.json";

        var c1 = JsonHelper.LoadJsonFile<WebCredentials>(path);

        var c2 = new WebCredentials
        {
            Url = c1.Url,
            // ToDo
            //Password = PasswordHelper.Decrypt(c1.Password),
            //Username = PasswordHelper.Decrypt(c1.Username)
        };

        return c2;
    }


    public const string FtpSubDir = "/Test";

    public const string FtpSubDirCreate = "/Test/AAA";


    public const string FtpTestFileName = "A.txt";


    public static string FtpTestFilePath => Path.Combine(TestDataPath, FtpTestFileName);


    public const string LocalTargetPath = @"D:\temp";


    //public string LocalTestFile => 


}