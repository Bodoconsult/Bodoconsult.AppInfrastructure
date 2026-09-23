// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.Web.Mail.Helpers;
using NUnit.Framework;

namespace BodoFileTransfer.Business.Test;

public class PasswordHandlerTests
{
    [Test]
    public void TestEncrypt()
    {
        // Arrange 
        var s = "blubb";

        // Act  
        var result1 = PasswordHandler.Encrypt(s);

        // Assert
        Assert.That(result1, Is.Not.Null);
        Assert.That(s, Is.Not.EqualTo( result1));

    }


    [Test]
    public void TestDecrypt()
    {
        // Arrange 
        var s = "TestBahnhof123";

        // Act  
        var result1 = PasswordHandler.Encrypt(s);

        // Assert
        Assert.That(result1, Is.Not.Null);
        Assert.That(s, Is.EqualTo( result1));

        // Act
        var result2 = PasswordHandler.Decrypt(result1);

        Assert.That(result2, Is.EqualTo(s));

    }


    [Explicit]
    [Test]
    public void Test1()
    {

        var s = "4b322d2f-23f9-46f2-b96f-449cb994bce7";

        s = PasswordHandler.Encrypt(s);

        Assert.That(string.IsNullOrEmpty(s), Is.False);

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