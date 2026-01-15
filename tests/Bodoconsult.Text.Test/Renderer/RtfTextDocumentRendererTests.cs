// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;
using Bodoconsult.Pdf.Stylesets;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Renderer.Rtf;
using Bodoconsult.Text.Test.Helpers;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.RtfRendering;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace Bodoconsult.Text.Test.Renderer;

[TestFixture]
public class RtfTextDocumentRendererTests
{

    [Test]
    public void Ctor_ValidDocument_PropsSetCorrectly()
    {
        // Arrange 
        var document = TestDataHelper.CreateDocument();
        var factory = new RtfTextRendererElementFactory();

        // Act  
        var renderer = new RtfTextDocumentRenderer(document, factory);

        // Assert
        Assert.That(renderer.Document, Is.Not.Null);
        Assert.That(renderer.Styleset, Is.Not.Null);
        Assert.That(renderer.PageStyleBase, Is.Not.Null);
        Assert.That(renderer.Content, Is.Not.Null);
        Assert.That(renderer.Content.Length, Is.EqualTo(0));
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

        var factory = new RtfTextRendererElementFactory();

        var renderer = new RtfTextDocumentRenderer(document, factory);

        // Act  
        renderer.RenderIt();

        // Assert
        Assert.That(renderer.Content.Length, Is.Not.EqualTo(0));

        Debug.Print(renderer.Content.ToString());

        if (!Debugger.IsAttached)
        {
            return;
        }

        var filePath = Path.Combine(Path.GetTempPath(), "test.rtf");

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

        var factory = new RtfTextRendererElementFactory();

        var renderer = new RtfTextDocumentRenderer(document, factory);

        // Act  
        renderer.RenderIt();

        // Assert
        Assert.That(renderer.Content.Length, Is.Not.EqualTo(0));

        Debug.Print(renderer.Content.ToString());

        if (!Debugger.IsAttached)
        {
            return;
        }

        var filePath = Path.Combine(Path.GetTempPath(), "test.rtf");

        renderer.SaveAsFile(filePath);

        FileSystemHelper.RunInDebugMode(filePath);
    }

    [Test]
    public void Rtf_ValidDocument_RtfFileCreated()
    {
        var workingDir = Path.GetTempPath();

        var filePath = Path.Combine(workingDir, "test.rtf");


        var styleset = new DefaultStyleSet();
        styleset.CreatePageSetup();
        styleset.CalculateMeasures();
        styleset.InitializeStyles();

        // Create a new MigraDoc document.
        var document = new MigraDoc.DocumentObjectModel.Document();

        // Add a section to the document.
        var section = document.AddSection();
        section.PageSetup = styleset.PageSetup.Clone();


        // Add a paragraph
        section.AddParagraph("Blubb");

        var image = section.Headers.Primary.AddImage(TestHelper.TestBackgroundImage);
        image.Height = section.PageSetup.PageHeight;
        image.Width = section.PageSetup.PageWidth;
        image.RelativeVertical = RelativeVertical.Page;
        image.RelativeHorizontal = RelativeHorizontal.Page;
        image.WrapFormat.Style = WrapStyle.Through;

        section.Headers.Primary.AddParagraph("HeaderText");


        // Create an RTF renderer for the MigraDoc document.
        var rtfRenderer = new RtfDocumentRenderer();

        // Layout and render document to RTF.
        rtfRenderer.Render(document, filePath, workingDir);


        //FileSystemHelper.RunInDebugMode(filePath);
    }

}