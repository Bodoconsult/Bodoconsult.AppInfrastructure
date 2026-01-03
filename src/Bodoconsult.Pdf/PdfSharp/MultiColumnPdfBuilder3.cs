// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.IO;
using Bodoconsult.Pdf.Stylesets;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// Class representing a multi-column text PDF document and basic functionality to add content to it. Adjust to use with LDML. Does not create TOC etc. automatically
/// </summary>
public class MultiColumnPdfBuilder3 : PdfBuilderBase
{
    #region Constructors

    /// <summary>
    /// Default ctor to load a complete styleset
    /// </summary>
    /// <param name="styleSet">Styleset to use</param>
    /// <param name="fontResolver">Font resolver to load</param>
    public MultiColumnPdfBuilder3(IStyleSet styleSet, IFontResolver fontResolver)
    {
        LoadDefaults();

        GlobalFontSettings.FontResolver ??= fontResolver;

        LoadStyleset(styleSet);
    }

    #endregion

    /// <summary>
    /// Save Pdf to a stream
    /// </summary>
    /// <param name="stream">Stream</param>
    public override void RenderToPdf(Stream stream)
    {
        var document = CreateMasterPdfDocument();
        document.Save(stream);
    }


    /// <summary>
    /// Save Pdf to a file
    /// </summary>
    /// <param name="fileName">Full path for pdf file's destination</param>
    /// <param name="showPdfFile">Show Pdf-File in a viewer</param>
    public override void RenderToPdf(string fileName, bool showPdfFile)
    {
        var document = CreateMasterPdfDocument();

        document.Save(fileName);

        if (!showPdfFile)
        {
            return;
        }

        // ...and start a viewer.
        OpenFile(fileName);
    }


    private static Section GetSection(DocumentRenderer docRenderer, int pageNumber)
    {
        return docRenderer.GetDocumentObjectsFromPage(pageNumber)[0].Section;
    }

    private XRect GetRect(int index)
    {
        var x = StyleSet.PageSetupOriginal.LeftMargin.Point + index * StyleSet.ColumnWidth.Point + index * StyleSet.Space.Point;

        var rect = new XRect(x, 0, StyleSet.PageSetup.PageWidth.Point, StyleSet.PageSetup.PageHeight.Point);
        return rect;
    }

    private PdfDocument CreateMasterPdfDocument()
    {
        var document = new PdfDocument();
        document.Info.Title = Document.Info.Title;
        document.Info.Subject = Document.Info.Subject;
        document.Info.Author = Document.Info.Author;


        // Create a renderer and prepare (=layout) the document
        var docRenderer = new DocumentRenderer(Document);
        docRenderer.PrepareDocument();

        // For clarity, we use point as unit of measure in this sample.
        // A4 is the standard letter size in Germany (21cm x 29.7cm).
        var a4Rect = new XRect(0, 0, StyleSet.PageSetup.PageWidth.Point, StyleSet.PageSetup.PageHeight.Point);

        var pageCount = docRenderer.FormattedDocument?.PageCount ?? 0;

        var idx = 0;

        XGraphics gfx = null;

        var oldSection = GetSection(docRenderer, 1);


        List<MultiPageInfo> pageInfos = new();

        MultiPageInfo currentPageInfo = null;

        var renderInfo = docRenderer.GetRenderInfoFromPage(1)[0];

        for (var pageNum = 0; pageNum < pageCount; pageNum++)
        {
            var section = GetSection(docRenderer, pageNum + 1);

            if (section != oldSection)
            {
                //var page = document.AddPage();
                //gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
                //page.Size = PageSize.A4;
                //page.Orientation = StyleSet.PageSetupOriginal.Orientation == Orientation.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait;

                idx = 0;
                oldSection = section;
            }

            if (idx == 0)
            {
                currentPageInfo = new MultiPageInfo();
                pageInfos.Add(currentPageInfo);

                var page = document.AddPage();
                currentPageInfo.Page = page;
                page.Size = PageSize.A4;
                page.Orientation = StyleSet.PageSetupOriginal.Orientation == Orientation.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait;
            }

            var docOs = docRenderer.GetRenderInfoFromPage(pageNum + 1);
            currentPageInfo.RenderInfo.Add(docOs);

            //foreach (var docO in docOs)
            //{
            //    var ca = docO.LayoutInfo.ContentArea;
            //    docRenderer.RenderObject(gfx, ca.X, ca.Y, ca.Width, docO.DocumentObject);
            //}

            idx++;
            if (idx >= StyleSet.NumberOfColumns)
            {
                idx = 0;
            }
        }

        for (var pageNum = pageInfos.Count - 1; pageNum >= 0; pageNum--)
        {
            var page = pageInfos[pageNum];
            gfx = XGraphics.FromPdfPage(page.Page, XGraphicsPdfPageOptions.Prepend);

            idx = 0;
            foreach (var ri in page.RenderInfo)
            {

                foreach (var docO in ri)
                {
                    //try
                    //{
                    var ca = docO.LayoutInfo.ContentArea;
                    docRenderer.RenderObject(gfx, ca.X + idx * (StyleSet.ColumnWidth.Point + StyleSet.Space.Point), ca.Y, ca.Width, docO.DocumentObject);
                    //}
                    //catch (Exception e)
                    //{

                    //}
                }

                idx++;
            }
        }

        return document;
    }
}