// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;
using System.Numerics;

namespace Bodoconsult.App.Test.HelperTests;

[TestFixture]
internal class MathHelperTests
{
    [TestCase(1, true, TestName="IsPowerOfTwo_1_ReturnsTrue")]
    [TestCase(2, true, TestName = "IsPowerOfTwo_2_ReturnsTrue")]
    [TestCase(3, false, TestName = "IsPowerOfTwo_3_ReturnsFalse")]
    [TestCase(4, true, TestName = "IsPowerOfTwo_4_ReturnsTrue")]
    [TestCase(7, false, TestName = "IsPowerOfTwo_7_ReturnsFalse")]
    public void IsPowerOfTwo_GivenNumber_ReturnsTrueOrFalse(int value, bool expectedResult)
    {
        // Arrange 

        // Act  
        var result = MathHelper.IsPowerOfTwo(value);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase(456504, 262144, TestName = "LastPowerOf2SmallerThanNumber_456504_ReturnsValue")]
    [TestCase(18, 16, TestName = "LastPowerOf2SmallerThanNumber_18_ReturnsValue")]
    [TestCase(7, 4, TestName = "LastPowerOf2SmallerThanNumber_7_ReturnsValue")]
    [TestCase(4, 4, TestName = "LastPowerOf2SmallerThanNumber_4_ReturnsValue")]
    [TestCase(3, 2, TestName = "LastPowerOf2SmallerThanNumber_3_ReturnsValue")]
    [TestCase(2, 2, TestName = "LastPowerOf2SmallerThanNumber_2_ReturnsValue")]
    [TestCase(1, 1, TestName= "LastPowerOf2SmallerThanNumber_1_ReturnsValue")]
    public void LastPowerOf2SmallerThanNumber_Number_ReturnsValue(int value, int expectedValue)
    {
        // Arrange 

        // Act  
        var result = MathHelper.LastPowerOf2SmallerThanNumber((uint)value);

        // Assert
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void CheckListLengthForFft_ListLength1_RemovesNoItem()
    {
        // Arrange 
        var b = new List<Complex> { new(real: 42, imaginary: 12) };

        // Act  
        var result = MathHelper.CheckListLengthForFft(b);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Count, Is.EqualTo(1));
        }
    }

    [Test]
    public void CheckListLengthForFft_ListLength2_RemovesNoItem()
    {
        // Arrange 
        var b = new List<Complex>
            {
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12)
            };

        // Act  
        var result = MathHelper.CheckListLengthForFft(b);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Count, Is.EqualTo(2));
        }
    }

    [Test]
    public void CheckListLengthForFft_ListLength3_Removes1Item()
    {
        // Arrange 
        var b = new List<Complex>
            {
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12)
            };

        // Act  
        var result = MathHelper.CheckListLengthForFft(b);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Count, Is.EqualTo(2));
        }
    }


    [Test]
    public void CheckListLengthForFft_ListLength4_RemovesNoItem()
    {
        // Arrange 
        var b = new List<Complex>
            {
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12)
            };

        // Act  
        var result = MathHelper.CheckListLengthForFft(b);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Count, Is.EqualTo(4));
        }
    }

    [Test]
    public void CheckListLengthForFft_ListLength7_Removes3Items()
    {
        // Arrange 
        var b = new List<Complex>
            {
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12),
                new(real: 42, imaginary: 12)
            };

        // Act  
        var result = MathHelper.CheckListLengthForFft(b);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Count, Is.EqualTo(4));
        }
    }

    [Test]
    public void CheckListLengthForFft_ListLength456504_Removes194360Items()
    {
        // Arrange 
        var b = new List<Complex>(456504);

        for (var i = 0; i < 456504; i++)
        {
            b.Add(new(real: 42, imaginary: 12));
        }

        // Act  
        var result = MathHelper.CheckListLengthForFft(b);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Count, Is.EqualTo(262144));
        }
    }
}