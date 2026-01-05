// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Extensions;

namespace Bodoconsult.App.Test.Extensions;

[TestFixture]
internal class StringExtensionsTests
{



    [Test]
    public void Repeat_Blank_ReturnsRepeatedBlanks()
    {
        // Arrange 
        const string value = " ";

        // Act  
        var result = value.Repeat(3);

        // Assert
        Assert.That(result, Is.EqualTo("   "));

    }

    [Test]
    public void AsSqlString_EmptyString_ReturnsNull()
    {
        // Arrange 
        const string value = "";

        // Act  
        var result = value.AsSqlString();

        // Assert
        Assert.That(result, Is.EqualTo("null"));

    }

    [Test]
    public void AsSqlString_NullString_ReturnsNull()
    {
        // Arrange 
        const string value = null;

        // Act  
        var result = value.AsSqlString();

        // Assert
        Assert.That(result, Is.EqualTo("null"));

    }

    [Test]
    public void AsSqlString_String_ReturnsNull()
    {
        // Arrange 
        const string value = "Value";

        // Act  
        var result = value.AsSqlString();

        // Assert
        Assert.That(result, Is.EqualTo("'Value'"));

    }

    [Test]
    public void LimitToLength_String_ReturnsTruncatedString()
    {
        // Arrange 
        const string value = "Value";

        // Act  
        var result = value.LimitToLength(2);

        // Assert
        Assert.That(result, Is.EqualTo("Va"));

    }

    [Test]
    public void LimitToLength_String_ReturnsString()
    {
        // Arrange 
        const string value = "Value";

        // Act  
        var result = value.LimitToLength(15);

        // Assert
        Assert.That(result, Is.EqualTo("Value"));

    }

    [Test]
    public void LimitToLength_EmptyString_ReturnsEmptyString()
    {
        // Arrange 
        const string value = "";

        // Act  
        var result = value.LimitToLength(15);

        // Assert
        Assert.That(result, Is.EqualTo(""));

    }

    [Test]
    public void LimitToLength_NullString_ReturnsNull()
    {
        // Arrange 
        const string value = null;

        // Act  
        var result = value.LimitToLength(15);

        // Assert
        Assert.That(result, Is.EqualTo(null));

    }

    [Test]
    public void SpaceCount_StringWith2Spaces_Returns2()
    {
        // Arrange 
        const string value = "Blubb Blabb Blubb";

        // Act  
        var result = value.SpaceCount();

        // Assert
        Assert.That(result, Is.EqualTo(2));

    }

    [Test]
    public void SpaceCount_EmptyString_Returns0()
    {
        // Arrange 
        const string value = "";

        // Act  
        var result = value.SpaceCount();

        // Assert
        Assert.That(result, Is.EqualTo(0));

    }

    [Test]
    public void SpaceCount_Null_Returns0()
    {
        // Arrange 
        const string value = null;

        // Act  
        var result = value.SpaceCount();

        // Assert
        Assert.That(result, Is.EqualTo(0));

    }


    [Test]
    public void ToGuid_ValidGuid_ReturnsGuid()
    {
        // Arrange 
        var uid = Guid.NewGuid();
        var input = uid.ToString();

        // Act  
        var result = input.ToGuid();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(uid));

    }

    [Test]
    public void ToGuid_ValidGuidWithoutMinuses_ReturnsGuid()
    {
        // Arrange 
        var uid = Guid.NewGuid();
        var input = uid.ToPlainString();

        // Act  
        var result = input.ToGuid();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(uid));

    }

    [Test]
    public void ToGuid_InvalidGuid_ReturnsNull()
    {
        // Arrange 
        var input = "blubb";

        // Act  
        var result = input.ToGuid();

        // Assert
        Assert.That(result, Is.EqualTo(Guid.Empty));
    }


    [Test]
    public void FirstCharToUpperCase_InvalidGuid_ReturnsNull()
    {
        // Arrange 
        var input = "blubB";

        // Act  
        var result = input.FirstCharToUpperCase();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result[0], Is.EqualTo('B'));
        Assert.That(result[4], Is.EqualTo('b'));
    }


    [Test]
    public void EscapeForXml_ValidString_ReturnsEscapedString()
    {
        // Arrange 
        var input = "<<page>>";

        // Act  
        var result = input.EscapeForXml();

        // Assert
        Assert.That(result, Is.Not.EqualTo(input));

    }
}