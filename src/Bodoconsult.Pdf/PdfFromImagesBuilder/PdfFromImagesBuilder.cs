// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.IO;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.Extensions;

namespace Bodoconsult.Pdf.PdfFromImagesBuilder;

/// <summary>
/// This class creates an PDF file from a list of images provide with images each filling a full page
/// </summary>
public class PdfFromImagesBuilder
{
    private readonly PdfDocument _pdfDocument;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="images">Images to add as single page</param>
    /// <param name="title"></param>
    /// <param name="author"></param>
    public PdfFromImagesBuilder(List<byte[]> images, string title, string author)
    {
        PdfSharpLicense.SetAgplLicense();

        _pdfDocument = new PdfDocument();
        _pdfDocument.Info.Author = author;
        _pdfDocument.Info.CreationDate = DateTime.Now;
        _pdfDocument.Info.Title = title;
        _pdfDocument.Info.Creator = author;

        _pdfDocument.PageLayout = PdfPageLayout.OneColumn;

        Images = images;
    }

    ///// <summary>
    ///// Page width in mm. Default: 210mm DIN A4 portrait
    ///// </summary>
    //public double PageWidth { get; set; } = 210;

    ///// <summary>
    ///// Page height in mm. Default: 297mm DIN A4 portrait
    ///// </summary>
    //public double PageHeight { get; set; } = 297;

    /// <summary>
    /// Page size. Default: A4
    /// </summary>
    public TypoPaperFormat PageSize { get; set; } = PaperFormats.A4Format;

    /// <summary>
    /// Page orientation. Default: true = Portait
    /// </summary>
    public bool Orientation { get; set; } = true;

    /// <summary>
    /// Use the PDF/A standard. Default: true
    /// </summary>
    public bool UsePdfA { get; set; } = true;

    /// <summary>
    /// Images to add as single page
    /// </summary>
    public List<byte[]> Images { get; }

    /// <summary>
    /// Create a new page in the document for each image and fill the page with the image 
    /// </summary>
    public void ProcessImages()
    {
        foreach (var image in Images)
        {
            // Add a page to the document
            var page = _pdfDocument.AddPage();

            page.Size = PageSize.ToPageSize();
            page.Orientation = Orientation ? PageOrientation.Portrait: PageOrientation.Landscape;

            ProcessImage(page, image);
        }
    }

    /// <summary>
    /// Process a single page
    /// </summary>
    /// <param name="page">Current page</param>
    /// <param name="imgfile">Current image to draw page-filling on the page</param>
    public static void ProcessImage(PdfPage page, byte[] imgfile)
    {
        var ms = new MemoryStream(imgfile);
        ms.Position = 0;
        var backimg = XImage.FromStream(ms);
        ms.Dispose();

        var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
        gfx.DrawImage(backimg, 0, 0, page.Width.Point, page.Height.Point);
    }

    /// <summary>
    /// Save the PDF as file
    /// </summary>
    /// <param name="filename">Full file path</param>
    public void SaveAs(string filename)
    {
        if (UsePdfA)
        {
            _pdfDocument.SetPdfA();
        }
        _pdfDocument.Save(filename);
    }
}