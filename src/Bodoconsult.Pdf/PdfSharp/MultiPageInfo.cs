// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

namespace Bodoconsult.Pdf.PdfSharp;

internal class MultiPageInfo
{
    /// <summary>
    /// Current page
    /// </summary>
    public PdfPage Page { get; set; }

    /// <summary>
    /// Current rendering info
    /// </summary>
    public List<RenderInfo[]> RenderInfo { get; set; } = new();

}