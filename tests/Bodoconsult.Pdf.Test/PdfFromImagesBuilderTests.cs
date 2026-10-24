// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.IO;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Helpers;
using NUnit.Framework;

// https://stackoverflow.com/questions/71440686/pdfsharp-migradoc-add-background-images-to-pages

// https://github.com/ststeiger/PdfSharpCore/issues/203

namespace Bodoconsult.Pdf.Test;

[TestFixture]
internal class PdfFromImagesBuilderTests
{
    private readonly byte[] _image = ResourceHelper.GetByteResource("Bodoconsult.Pdf.Test.Resources.page.jpg");
    private readonly string _fileName = Path.Combine(Path.GetTempPath(), "test.pdf");

    [Test]
    public void ProcessImages_OneImage_PdfCreated()
    {
        // Arrange 
        var images = new List<byte[]> { _image };

        FileSystemHelper.DeleteFile(_fileName);

        var pdf = new PdfFromImagesBuilder.PdfFromImagesBuilder(images, "Blubb title", "Testauthor");

        // Act  
        pdf.ProcessImages();
        pdf.SaveAs(_fileName);

        // Assert
        Assert.That(File.Exists(_fileName));

        FileSystemHelper.RunInDebugMode(_fileName);
    }

    [Test]
    public void ProcessImages_TwoImages_PdfCreated()
    {
        // Arrange 
        var images = new List<byte[]> { _image, _image };

        FileSystemHelper.DeleteFile(_fileName);

        var pdf = new PdfFromImagesBuilder.PdfFromImagesBuilder(images, "Blubb title", "Testauthor");

        // Act  
        pdf.ProcessImages();
        pdf.SaveAs(_fileName);

        // Assert
        Assert.That(File.Exists(_fileName));

        FileSystemHelper.RunInDebugMode(_fileName);
    }


}