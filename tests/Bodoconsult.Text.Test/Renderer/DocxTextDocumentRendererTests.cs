// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.IO;
using Bodoconsult.App.Helpers;
using Bodoconsult.Office.Helpers;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Renderer.Docx;
using Bodoconsult.Text.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Text.Test.Renderer;

[TestFixture]
public class DocxTextDocumentRendererTests
{

    [Test]
    public void Ctor_ValidDocument_PropsSetCorrectly()
    {
        // Arrange 
        var document = TestDataHelper.CreateDocument();
        var factory = new DocxTextRendererElementFactory();

        // Act  
        var renderer = new DocxTextDocumentRenderer(document, factory);

        // Assert
        Assert.That(renderer.Document, Is.Not.Null);
        Assert.That(renderer.Styleset, Is.Not.Null);
        Assert.That(renderer.PageStyleBase, Is.Not.Null);
        Assert.That(renderer.DocxDocument, Is.Not.Null);
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

        var factory = new DocxTextRendererElementFactory();

        var renderer = new DocxTextDocumentRenderer(document, factory);

        // Act  
        renderer.RenderIt();

        // Assert
        if (!Debugger.IsAttached)
        {
            return;
        }

        var filePath = Path.Combine(Path.GetTempPath(), "test.docx");

        renderer.SaveAsFile(filePath);

        OpenXmlHelper.ValidateWordDocument(filePath);

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

        var factory = new DocxTextRendererElementFactory();

        var renderer = new DocxTextDocumentRenderer(document, factory);

        // Act  
        renderer.RenderIt();

        // Assert
        if (!Debugger.IsAttached)
        {
            return;
        }

        var filePath = Path.Combine(Path.GetTempPath(), "test.docx");

        renderer.SaveAsFile(filePath);

        FileSystemHelper.RunInDebugMode(filePath);
    }

}