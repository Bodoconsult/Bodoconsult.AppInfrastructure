// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection;
using Bodoconsult.Web.Mail.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Web.Mail.Test.HelperTests;

[TestFixture]
internal class MailHelperTests
{
    public MailHelperTests()
    {
        PasswordHandler.CreateKeys();
    }

    [Test]
    public void GetEncryptedSmtpMail_GetDecryptedSmtpMailAccount_ValidAccount_AccountEncryptedAndDecrypted()
    {
        // Arrange 
        var ma = new SmtpMailAccount
        {
            SmtpPassword = "Test",
            SmtpAccountName = "Hallo",
            SmtpServer = "Blubb",
            SmtpPort = 99,
            UseSecureConnection = true,
            MailAddressSender = "mopp"
        };

        // Act  
        var nma = MailHelper.GetEncryptedSmtpMailAccount(ma);

        var rma = MailHelper.GetDecryptedSmtpMailAccount(nma);

        // Assert
        Assert.That(rma.SmtpPassword, Is.EqualTo(ma.SmtpPassword));
        Assert.That(rma.SmtpServer, Is.EqualTo(ma.SmtpServer));
        Assert.That(rma.SmtpAccountName, Is.EqualTo(ma.SmtpAccountName));
        Assert.That(rma.SmtpPort, Is.EqualTo(ma.SmtpPort));
        Assert.That(rma.UseSecureConnection, Is.EqualTo(ma.UseSecureConnection));
        Assert.That(rma.MailAddressSender, Is.EqualTo(ma.MailAddressSender));
    }

    [Test]
    public void GetEncryptedO365Mail_GetDecryptedO365MailAccount_ValidAccount_AccountEncryptedAndDecrypted()
    {
        // Arrange 
        var ma = new O365MailAccount
        {
            Tenant = "blubb",
            ClientId = "blabb",
            ClientSecret = "blobb",
            Instance = "blibb",
            Scope = "blepp",
            MailAddressSender = "aha",
            UserName = "mopp"
        };

        // Act  
        var nma = MailHelper.GetEncryptedO365MailAccount(ma);

        var rma = MailHelper.GetDecryptedO365MailAccount(nma);

        // Assert
        Assert.That(rma.Tenant, Is.EqualTo(ma.Tenant));
        Assert.That(rma.Scope, Is.EqualTo(ma.Scope));
        Assert.That(rma.ClientId, Is.EqualTo(ma.ClientId));
        Assert.That(rma.ClientSecret, Is.EqualTo(ma.ClientSecret));
        Assert.That(rma.Instance, Is.EqualTo(ma.Instance));
        Assert.That(rma.MailAddressSender, Is.EqualTo(ma.MailAddressSender));
        Assert.That(rma.UserName, Is.EqualTo(ma.UserName));
    }
}