// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Web.Ftp.Models;
using Bodoconsult.Web.Ftp.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Web.Ftp.Test;

[TestFixture]
internal class FtpTestHelperTests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void TestCreateCredentials()
    {

        const string filename = @"D:\temp\FtpCredentials.json";

        var c = new SshCredentials
        {
            Url = "YourFtpServerUrl", // www.test.de
            Password = PasswordHelper.Encrypt("YourUsername"), 
            Username = PasswordHelper.Encrypt("Password")
        };

        JsonHelper.SaveAsFile(filename, c);
    }

    [Test]
    public void TestGetCredentials()
    {

        // Act
        var result = Helpers.TestHelper.GetCredentials();

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}