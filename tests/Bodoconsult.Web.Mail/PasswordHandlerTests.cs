// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Diagnostics;
using Bodoconsult.Web.Mail.Helpers;
using Bodoconsult.Web.Mail.Models;
using Bodoconsult.Web.Mail.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Web.Mail.Test;

[TestFixture]
internal class PasswordHandlerTests
{

    [Test]
    [Explicit]
    public void Test3()
    {
        var pwdKey = new O365MailAccount()
        {
            Tenant = "",
            Instance = "",
            Scope = "",
            ClientId = "",
            ClientSecret = "",
            UserName = "",

        };

        JsonHelper.SaveAsFile(@"C:\temp\O365Mailer.json", pwdKey);

        Assert.That(true);
    }


    [Test]
    [Explicit]
    public void Test1()
    {
        var pwdKey = new PwdKeys
        {
            Key1 = "abc",
            Key2 = "def",
            Key3 = "ghi",
            Salt = [0x46, 0x76, 0x62, 0x6e, 0x21, 0x4d, 0x66, 0x63, 0x76, 0x65, 0x64, 0x65, 0x76]

        };

        JsonHelper.SaveAsFile(@"C:\temp\pwd.json", pwdKey);

        Assert.That(true);
    }

    [Test]
    public void Encrypt_ValidString_EncryptedSuccessfully()
    {
        // Arrange 
        var s = "noreply@bodoconsult.de";

        // Act  
        var result1 = PasswordHandler.Encrypt(s);

        // Assert
        Assert.That(result1, Is.Not.Null);
        Assert.That(s, Is.Not.EqualTo(result1));

    }

    [Test]
    public void Decrypt_ValidString_DecryptedSuccessfully()
    {
        // Arrange 
        var s = "TestBahnhof123";

        // Act  
        var result1 = PasswordHandler.Encrypt(s);

        // Assert
        Assert.That(result1, Is.Not.Null);
        Assert.That(s, Is.Not.EqualTo(result1));

        // Act
        var result2 = PasswordHandler.Decrypt(result1);

        Assert.That(result2, Is.EqualTo(s));
    }


    [Explicit]
    [Test]
    public void Test2()
    {

        var s = "TestBahnhof123";

        var x = TestHelper.TempPath;
        Assert.That(s, Is.Not.Null);

        s = PasswordHandler.Encrypt(s);

        Assert.That(!string.IsNullOrEmpty(s));

        Debug.Print(s);

        //var s = new AppSettings
        //{
        //    ConnectionString =
        //        "Data Source=192.168.10.125;Initial Catalog=BodoFileTransfer;Integrated Security=SSPI;",
        //    ErrorMailer = "test@bodoconsult.de",
        //};

        //JsonHelper.SaveAsFile(@"D:\temp\appSettings.json", s);

        //Assert.That(true);
    }
}