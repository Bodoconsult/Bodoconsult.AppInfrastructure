// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataProtection.Hashing;

namespace Bodoconsult.App.Test.DataProtection;

[TestFixture]
internal class HasherTests
{
    [Test]
    public void Ctor_DefaultSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            var unused = new Hasher();
        });

        // Assert
    }

    [Test]
    public void ComputeHash_DefaultSetupString_HashCreated()
    {
        // Arrange 
        var hasher = new Hasher();

        var pwd = "Test";

        // Act
        var result = hasher.ComputeHash(pwd);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.Not.Zero);
    }

    [Test]
    public void ValidateHash_DefaultSetupString_HashValidated()
    {
        // Arrange 
        var hasher = new Hasher();

        var pwd = "Test";

        var hash = hasher.ComputeHash(pwd);

        // Act
        var result = hasher.ValidateHash(pwd, hash);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void ComputeHash_DefaultSetupStream_HashCreated()
    {
        // Arrange 
        var hasher = new Hasher();

        var pwd = "Test"u8.ToArray();
        var stream = new MemoryStream(pwd);

        // Act
        var result = hasher.ComputeHash(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.Not.Zero);
    }

    [Test]
    public void ValidateHash_DefaultSetupStream_HashValidated()
    {
        // Arrange 
        var hasher = new Hasher();

        var pwd = "Test"u8.ToArray();
        var stream = new MemoryStream(pwd);

        var hash = hasher.ComputeHash(stream);

        // Act
        var result = hasher.ValidateHash(stream, hash);

        // Assert
        Assert.That(result, Is.True);
    }
}