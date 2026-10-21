// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.DataProtection;

namespace Bodoconsult.App.Test.DataProtection;

[TestFixture]
public class PasswordHandlerTests
{
    [Test]
    public void CreateKeys_ValidSetup_KeyCreated()
    {
        // Arrange

        // Act
        PasswordHandler.CreateKeys();

        // Assert
        Assert.That(PasswordHandler.Key1.Length, Is.Not.Zero);
        Assert.That(PasswordHandler.Key2.Length, Is.Not.Zero);
        Assert.That(PasswordHandler.Key3.Length, Is.Not.Zero);
        Assert.That(PasswordHandler.Salt.Length, Is.Not.Zero);

        Debug.Print(PasswordHandler.Key1);
        Debug.Print(PasswordHandler.Key2);
        Debug.Print(PasswordHandler.Key3);
        Debug.Print(PasswordHandler.Salt);
    }

    [Test]
    public void Decrypt_ValidPassword_PasswordDecrypted()
    {
        const string password = "rechnung@bodoconsult.de";

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