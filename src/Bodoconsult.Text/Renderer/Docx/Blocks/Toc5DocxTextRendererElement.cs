// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Office;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Toc5"/> instances
/// </summary>
public class Toc5DocxTextRendererElement : TocxDocxTextRendererElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public Toc5DocxTextRendererElement(Toc5 toc5) : base(toc5)
    {
        ClassName = toc5.StyleName;
    }
}