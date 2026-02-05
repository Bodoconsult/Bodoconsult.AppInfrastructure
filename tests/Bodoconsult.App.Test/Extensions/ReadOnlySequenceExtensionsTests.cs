// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Buffers;
using Bodoconsult.App.Abstractions.Extensions;

namespace Bodoconsult.App.Test.Extensions;

[TestFixture]
internal class ReadOnlySequenceExtensionsTests
{
    [Test]
    public void IsEqualTo_EqualValues_ReturnsTrue()
    {
        // Arrange 
        var r1 = new ReadOnlySequence<byte>([0x2, 0x42, 0x6c, 0x75, 0x62, 0x62, 0x3]);
        var r2 = new ReadOnlySequence<byte>([0x2, 0x42, 0x6c, 0x75, 0x62, 0x62, 0x3]);

        // Act  
        var result = r1.IsEqualTo(r2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsEqualTo_ValuesWithDifferentLength_ReturnsFalse()
    {
        // Arrange 
        var r1 = new ReadOnlySequence<byte>([0x2, 0x42, 0x6c, 0x75, 0x62, 0x62]);
        var r2 = new ReadOnlySequence<byte>([0x2, 0x42, 0x6c, 0x75, 0x62, 0x62, 0x3]);

        // Act  
        var result = r1.IsEqualTo(r2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsEqualTo_ValuesWithDifferentContent_ReturnsFalse()
    {
        // Arrange 
        var r1 = new ReadOnlySequence<byte>([0x2, 0x42, 0x6c, 0x75, 0x62, 0x62]);
        var r2 = new ReadOnlySequence<byte>([0x2, 0x42, 0x6b, 0x75, 0x62, 0x62, 0x3]);

        // Act  
        var result = r1.IsEqualTo(r2);

        // Assert
        Assert.That(result, Is.False);
    }
}