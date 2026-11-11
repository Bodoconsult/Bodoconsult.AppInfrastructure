// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.Extensions;
using NUnit.Framework;

namespace Bodoconsult.Pdf.Test.Extensions;

[TestFixture]
internal class PdfColorExtensionsTests
{
    [Test]
    public void ToPdfColor_ValidColor_Converted()
    {
        // Arrange 
        var color = TypoColors.AliceBlue;

        // Act  
        var pdfColor = color.ToPdfColor();

        // Assert
        Assert.That(pdfColor, Is.Not.Null);
        Assert.That(pdfColor.A, Is.EqualTo(color.A));
        Assert.That(pdfColor.R, Is.EqualTo(color.R));
        Assert.That(pdfColor.G, Is.EqualTo(color.G));
        Assert.That(pdfColor.B, Is.EqualTo(color.B));
    }

    [Test]
    public void FromHtml_ValidHtmlColor9_Converted()
    {
        // Arrange 
        const string color = "#00FFEEAACC";

        // Act  
        var pdfColor = color.FromHtml();

        // Assert
        Assert.That(pdfColor, Is.Not.Null);
        Assert.That(pdfColor.A, Is.EqualTo(0));
        Assert.That(pdfColor.R, Is.EqualTo(255));
        Assert.That(pdfColor.G, Is.EqualTo(238));
        Assert.That(pdfColor.B, Is.EqualTo(170));
    }

    [Test]
    public void FromHtml_ValidHtmlColor7_Converted()
    {
        // Arrange 
        const string color = "#E4EDFF";

        // Act  
        var pdfColor = color.FromHtml();

        // Assert
        Assert.That(pdfColor, Is.Not.Null);
        Assert.That(pdfColor.A, Is.EqualTo(255));
        Assert.That(pdfColor.R, Is.EqualTo(228));
        Assert.That(pdfColor.G, Is.EqualTo(237));
        Assert.That(pdfColor.B, Is.EqualTo(255));
    }
}