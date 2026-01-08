// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Office;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using Bodoconsult.Text.Renderer.Rtf.Blocks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Tot"/> instances
/// </summary>
public class TotDocxTextRendererElement : TocxDocxTextRendererElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public TotDocxTextRendererElement(Tot tot) : base(tot)
    {
        ClassName = tot.StyleName;
    }
}