// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Extensions;

namespace Bodoconsult.App.Test.Extensions;

[TestFixture]
internal class IntExtensionsTests
{
    [Test]
    public void ArabicToRoman_ValidString_ReturnsString()
    {
        // Arrange 
        var input = 1;
        const string expectedResult = "I";

        // Act  
        var result = input.ArabicToRoman();

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void ToLowerLatin_ValidString_ReturnsString()
    {
        // Arrange 
        var input = 1;
        const string expectedResult = "a";

        // Act  
        var result = input.ToLowerLatin();

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void ToUpperLatin_ValidString_ReturnsString()
    {
        // Arrange 
        var input = 1;
        const string expectedResult = "A";

        // Act  
        var result = input.ToUpperLatin();

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}