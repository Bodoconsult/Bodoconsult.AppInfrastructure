// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Drawing.SkiaSharp.Extensions;

namespace Bodoconsult.Drawing.SkiaSharp.Tests;

[TestFixture]
internal class ColorExtensionsTests
{
    [Test]
    public void ToSKColor_TypoColor_ReturnsSKColor()
    {
        // Arrange 
        var typoColor = TypoColors.AliceBlue;

        // Act  
        var result = typoColor.ToSKColor();

        // Assert
        Assert.That(result.Red, Is.EqualTo(typoColor.R));
        Assert.That(result.Green, Is.EqualTo(typoColor.G));
        Assert.That(result.Blue, Is.EqualTo(typoColor.B));
        Assert.That(result.Alpha, Is.EqualTo(typoColor.A));
    }
}