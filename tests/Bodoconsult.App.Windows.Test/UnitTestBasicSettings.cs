// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.IO;
using Bodoconsult.App.DataProtection;
using Bodoconsult.App.Windows.Test.Helpers;
using Bodoconsult.App.Windows.Test.Model;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test;

[TestFixture]
public class UnitTestBasicSettings
{
    [Test]
    public void TestGenerateAppSettings()
    {

        var fileName = Path.Combine(TestHelper.OutputPath, TestHelper.NameAppSettingsFile);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        Assert.That(!File.Exists(fileName));

        var s = new AppSettings
        {
            Domain = "xyz.de", 
            UserName = PasswordHandler.Encrypt("YourUserName"), 
            DomainServer = "FqnDnsServer",
            Password = PasswordHandler.Encrypt("YourPassword")
        };
           

        JsonHelper.SaveAsFile(fileName, s);


        Assert.That(File.Exists(fileName));

    }

}