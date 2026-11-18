// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Drawing.SkiaSharp.Extensions;
using SkiaSharp;

namespace Bodoconsult.Drawing.SkiaSharp.Tests;

[TestFixture]
internal class TextAligmentExtensionsTests
{
    [Test]
    public void ToSKTextAlign_Center_ReturnsSKTextAlignCenter()
    {
        // Arrange 
        var typoAlign= TypoTextAlignment.Center;

        // Act  
        var result = typoAlign.ToSKTextAlign();

        // Assert
        Assert.That(result, Is.EqualTo(SKTextAlign.Center));
    }

    [Test]
    public void ToSKTextAlign_Left_ReturnsSKTextAlignLeft()
    {
        // Arrange 
        var typoAlign = TypoTextAlignment.Left;

        // Act  
        var result = typoAlign.ToSKTextAlign();

        // Assert
        Assert.That(result, Is.EqualTo(SKTextAlign.Left));
    }

    [Test]
    public void ToSKTextAlign_Justify_ReturnsSKTextAlignLeft()
    {
        // Arrange 
        var typoAlign = TypoTextAlignment.Justify;

        // Act  
        var result = typoAlign.ToSKTextAlign();

        // Assert
        Assert.That(result, Is.EqualTo(SKTextAlign.Left));
    }

    [Test]
    public void ToSKTextAlign_Right_ReturnsSKTextAlignRight()
    {
        // Arrange 
        var typoAlign = TypoTextAlignment.Right;

        // Act  
        var result = typoAlign.ToSKTextAlign();

        // Assert
        Assert.That(result, Is.EqualTo(SKTextAlign.Right));
    }
}