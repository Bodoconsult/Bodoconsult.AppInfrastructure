// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.DataProtection;
using BodoFileTransfer.Business.Helpers;
using NUnit.Framework;

namespace BodoFileTransfer.Test;

[TestFixture]
internal class PasswordHandlerTests
{
    public PasswordHandlerTests()
    {
        AppHelper.LoadPasswordData();
    }

    [Test]
    public void Decrypt_ValidPassword_PasswordDecrypted()
    {
        const string password = "blubb";

        var encryptedPassword = PasswordHandler.Encrypt(password);

        Debug.Print(encryptedPassword);

        var decryptedPassword = PasswordHandler.Decrypt(encryptedPassword);

        Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(password == decryptedPassword);
    }

    [Test]
    public void Decrypt2_ValidPassword_PasswordDecrypted()
    {
        const string password = "Blubb";

        var encryptedPassword = PasswordHandler.Encrypt2(password);

        Debug.Print(encryptedPassword);

        var decryptedPassword = PasswordHandler.Decrypt2(encryptedPassword);

        Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(password == decryptedPassword);
    }

    [Test]
    public void Decrypt3_ValidPassword_PasswordDecrypted()
    {
        const string password = "Blubb";

        var encryptedPassword = PasswordHandler.Encrypt3(password);

        Debug.Print(encryptedPassword);

        var decryptedPassword = PasswordHandler.Decrypt3(encryptedPassword);

        Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(password == decryptedPassword);
    }
}