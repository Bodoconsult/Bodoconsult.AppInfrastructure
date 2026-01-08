// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Office;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Toe"/> instances
/// </summary>
public class ToeDocxTextRendererElement : TocxDocxTextRendererElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public ToeDocxTextRendererElement(Toe toe) : base(toe)
    {
        ClassName = toe.StyleName;
    }
}