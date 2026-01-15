// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Extensions;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Test.Extensions;

[TestFixture]
public class TypoColorExtensionsTests
{

    [Test]
    public void ToArgbInt_LightGray_ValidInt()
    {
        // Arrange 
        var color = TypoColors.LightGray;

        // Act  
        var result = color.ToArgbInt();

        // Assert
        Assert.That(result, Is.EqualTo(-2894893));

    }

    [Test]
    public void ToInt_LightGray_ValidInt()
    {
        // Arrange 
        var color = TypoColors.LightGray;

        // Act  
        var result = color.ToInt();

        // Assert
        Assert.That(result, Is.EqualTo(19737901));

    }

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
    public void ToHtml2_LightGray_ValidInt()
    {
        // Arrange 
        var color = TypoColors.LightGray;

        // Act  
        var result = color.ToHtml2();

        // Assert
        Assert.That(result, Is.EqualTo("D3D3D3"));

    }


}