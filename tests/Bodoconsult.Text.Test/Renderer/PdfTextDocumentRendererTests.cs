// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using Bodoconsult.App.Helpers;
using Bodoconsult.Pdf.PdfSharp;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Renderer.Pdf;
using Bodoconsult.Text.Test.Helpers;
using NUnit.Framework;
using PdfSharp.Fonts;

namespace Bodoconsult.Text.Test.Renderer;

[SupportedOSPlatform("windows")]
[TestFixture]
public class PdfTextDocumentRendererTests
{
    private readonly IFontResolver _fontResolver = new WindowsFontResolver();

    [Test]
    public void Ctor_ValidDocument_PropsSetCorrectly()
    {
        // Arrange 
        var document = TestDataHelper.CreateDocument();
        var factory = new PdfTextRendererElementFactory();

        // Act  
        var renderer = new PdfTextDocumentRenderer(document, factory, _fontResolver);

        // Assert
        Assert.That(renderer.Document, Is.Not.Null);
        Assert.That(renderer.Styleset, Is.Not.Null);
        Assert.That(renderer.PageStyleBase, Is.Not.Null);
        Assert.That(renderer.PdfDocument, Is.Not.Null);
    }

    [Test]
    public void RenderIt_ValidDocument_PropsSetCorrectly()
    {
        // Arrange 
        var document = TestDataHelper.CreateDocument();

        var calc = new LdmlCalculator(document);
        calc.UpdateAllTables();
        calc.EnumerateAllItems();
        calc.PrepareAllItems();
        calc.PrepareAllSections();

        var factory = new PdfTextRendererElementFactory();

        var renderer = new PdfTextDocumentRenderer(document, factory, _fontResolver);

        // Act  
        renderer.RenderIt();

        // Assert
        if (!Debugger.IsAttached)
        {
            return;
        }

        var filePath = Path.Combine(Path.GetTempPath(), "test.pdf");

        renderer.SaveAsFile(filePath);

        FileSystemHelper.RunInDebugMode(filePath);
    }

    [Test]
    public void RenderIt_ValidDocumentLandscape3Columns_PropsSetCorrectly()
    {
        // Arrange 
        var document = TestDataHelper.CreateDocumentLandscape3Columns();

        var calc = new LdmlCalculator(document);
        calc.UpdateAllTables();
        calc.EnumerateAllItems();
        calc.PrepareAllItems();
        calc.PrepareAllSections();

        var factory = new PdfTextRendererElementFactory();

        var renderer = new PdfTextDocumentRenderer(document, factory, _fontResolver);

        // Act  
        renderer.RenderIt();

        // Assert
        if (!Debugger.IsAttached)
        {
            return;
        }

        var filePath = Path.Combine(Path.GetTempPath(), "test.pdf");

        renderer.SaveAsFile(filePath);

        FileSystemHelper.RunInDebugMode(filePath);
    }

}