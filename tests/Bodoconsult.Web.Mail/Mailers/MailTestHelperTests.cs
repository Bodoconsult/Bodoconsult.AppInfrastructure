// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.IO;
using Bodoconsult.App.Helpers;
using Bodoconsult.Web.Mail.Helpers;
using Bodoconsult.Web.Mail.Models;
using Bodoconsult.Web.Mail.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Web.Mail.Test.Mailers;

[TestFixture]
internal class MailTestHelperTests
{
    [Test]
    public void GetO365Credentials_ValidSetup_ReturnsAccount()
    {
        // Act
        var result = TestHelper.GetTestO365Account();

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Explicit]
    [Test]
    public void SaveO365Credentials_ValidCredentials_SavedAsJson()
    {
        // Arrange
        const string fileName = @"C:\temp\O365Mailer1.json";

        var acc = new O365MailAccount
        {
            ClientId = PasswordHandler.Encrypt(""),
            ClientSecret = PasswordHandler.Encrypt(""),
            Scope = PasswordHandler.Encrypt("https://graph.microsoft.com/.default"),
            Instance = PasswordHandler.Encrypt("https://login.microsoftonline.com/{0}"),
            Tenant = PasswordHandler.Encrypt(""),
            UserName = PasswordHandler.Encrypt("")
        };

        // Act
        JsonHelper.SaveAsFile(fileName, acc);

        // Assert
        Assert.That(File.Exists(fileName));
    }
}