// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.HelperTests;

[TestFixture]
internal class PasswordHandlerTests
{
    [Test]
    public void Encrypt_ValidString_EncryptedSuccessfully()
    {
        // Arrange 
        const string s = "noreply@bodoconsult.de";

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
        const string s = "TestBahnhof123";

        // Act  
        var result1 = PasswordHandler.Encrypt(s);

        // Assert
        Assert.That(result1, Is.Not.Null);
        Assert.That(s, Is.Not.EqualTo(result1));

        // Act
        var result2 = PasswordHandler.Decrypt(result1);

        Assert.That(result2, Is.EqualTo(s));
    }
}