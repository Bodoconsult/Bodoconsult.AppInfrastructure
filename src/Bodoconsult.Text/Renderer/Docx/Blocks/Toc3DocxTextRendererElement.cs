// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Office;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Toc3"/> instances
/// </summary>
public class Toc3DocxTextRendererElement : TocxDocxTextRendererElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public Toc3DocxTextRendererElement(Toc3 toc3) : base(toc3)
    {
        ClassName = toc3.StyleName;
    }
}