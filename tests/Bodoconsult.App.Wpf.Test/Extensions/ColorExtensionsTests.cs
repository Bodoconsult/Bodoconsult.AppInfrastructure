// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows.Media;
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
}