// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows.Media;
using Bodoconsult.App.Abstractions.Extensions;
using Bodoconsult.App.Abstractions.Interfaces;
using NUnit.Framework;
using Bodoconsult.App.Wpf.Extensions;

namespace Bodoconsult.App.Wpf.Test.Extensions;

[TestFixture]
public class ColorExtensionsTests
{
    [Test]
    public void ToHtml_LightGray_ValidInt()
    {
        // Arrange 
        var color = Colors.LightGray;

        // Act  
        var result = color.ToHtml();

        // Assert
        Assert.That(result, Is.EqualTo("#D3D3D3"));

    }

    [Test]
    public void ToHtml_LightGrayAlpha_ValidInt()
    {
        // Arrange 
        var color = Colors.LightGray;
        color.A = 55;

        // Act  
        var result = color.ToHtml();

        // Assert
        Assert.That(result, Is.EqualTo("#37D3D3D3"));

    }

    [Test]
    public void ToTypoColor_LightGrayAlpha_ValidColor()
    {
        // Arrange 
        var color = Colors.LightGray;
        color.A = 55;

        // Act  
        var result = color.ToTypoColor();

        // Assert
        Assert.That(result.IsEqualTo(color), Is.True);
    }

    [Test]
    public void Equals_LightGray_Equal()
    {
        // Arrange 
        var color = Colors.LightGray;

        // Act  
        var result = color.IsEqualTo(TypoColors.LightGray);

        // Assert
        Assert.That(result, Is.True);
    }

}

[TestFixture]
public class TypoColorExtensionsTests
{
    [Test]
    public void ToHtml_LightGray_ValidInt()
    {
        // Arrange 
        var color = TypoColors.LightGray;

        // Act  
        var result = color.ToHtml();

        // Assert
        Assert.That(result, Is.EqualTo("#D3D3D3"));
    }

    [Test]
    public void ToHtml_LightGrayAlpha_ValidInt()
    {
        // Arrange 
        var color = Colors.LightGray;
        color.A = 55;

        // Act  
        var result = color.ToHtml();

        // Assert
        Assert.That(result, Is.EqualTo("#37D3D3D3"));
    }

    [Test]
    public void ToTypoColor_LightGrayAlpha_ValidColor()
    {
        // Arrange 
        var color = TypoColors.LightGray;
        color.A = 55;

        // Act  
        var result = color.ToColor();

        // Assert
        Assert.That(result.IsEqualTo(color), Is.True);
    }

    [Test]
    public void Equals_LightGray_Equal()
    {
        // Arrange 
        var color = TypoColors.LightGray;

        // Act  
        var result = color.IsEqualTo(Colors.LightGray);

        // Assert
        Assert.That(result, Is.True);
    }

}