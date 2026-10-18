// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bodoconsult.Pdf.PdfFromImagesBuilder
{
    /// <summary>
    /// This class creates an PDF file from a list of images provide with images each filling a full page
    /// </summary>
    public class PdfFromImagesBuilder
    {
        //public void AddBackgroundImageFromByteArray(PdfPage page, byte[] imgfile)
        //{
        //    if (!ximageLoaded)
        //    {
        //        MemoryStream ms = new System.IO.MemoryStream(imgfile);
        //        ms.Position = 0;
        //        backimg = XImage.FromStream(() => ms);
        //        ms.Dispose();
        //        ximageLoaded = true;
        //    }

        //    XGraphics gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
        //    gfx.DrawImage(backimg, 0, 0, page.Width, page.Height);
        //}
    }
}
