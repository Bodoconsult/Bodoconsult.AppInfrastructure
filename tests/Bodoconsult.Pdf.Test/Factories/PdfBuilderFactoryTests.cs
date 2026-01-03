// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.PdfSharp;
using NUnit.Framework;
using System.Runtime.Versioning;
using Bodoconsult.Pdf.Stylesets;

namespace Bodoconsult.Pdf.Test.Factories;

[TestFixture]
[SupportedOSPlatform("windows")]
internal class PdfBuilderFactoryTests
{

    [Test]
    public void CreateInstance_DefaultStyleSetSingleColumn_PdfBuilderCreated()
    {
        // Arrange 
        var fontResolver = new WindowsFontResolver();

        var styleSet = new DefaultStyleSet
        {
            NumberOfColumns = 1
        };
        styleSet.CreatePageSetup();
        styleSet.CalculateMeasures();
        styleSet.InitializeStyles();

        var factory = new PdfBuilderFactory(fontResolver);

        // Act  
        var result = factory.CreateInstance(styleSet);

        // Assert
        Assert.That(result is PdfBuilder);
    }

    [Test]
    public void CreateInstance_DefaultStyleSetMultiColumn_MultiColumnPdfBuilderCreated()
    {
        // Arrange 
        var fontResolver = new WindowsFontResolver();

        var styleSet = new DefaultStyleSet
        {
            NumberOfColumns = 3
        };
        styleSet.CreatePageSetup();
        styleSet.CalculateMeasures();
        styleSet.InitializeStyles();

        var factory = new PdfBuilderFactory(fontResolver);

        // Act  
        var result = factory.CreateInstance(styleSet);

        // Assert
        Assert.That(result is MultiColumnPdfBuilder);
    }

}