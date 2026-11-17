// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Drawing.SkiaSharp.Helpers;

namespace Bodoconsult.Drawing.SkiaSharp.Tests;

internal class UnitHelperTests
{
    [Test]
    public void GetPixelsFromCm_2cm_236Pixels()
    {
        // Arrange 
        UnitHelper.DotsPerInch = 300;
        var value = 2d;
        var expectedResult = 236;

        // Act  
        var result = UnitHelper.GetPixelsFromCm(value);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void GetPixelsFromCm_2inch_600Pixels()
    {
        // Arrange 
        UnitHelper.DotsPerInch = 300;
        var value = 2d;
        var expectedResult = 600;

        // Act  
        var result = UnitHelper.GetPixelsFromInch(value);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}