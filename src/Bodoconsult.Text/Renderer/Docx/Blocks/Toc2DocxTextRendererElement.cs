// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Toc2"/> instances
/// </summary>
public class Toc2DocxTextRendererElement : TocxDocxTextRendererElement
{

    /// <summary>
    /// Default ctor
    /// </summary>
    public Toc2DocxTextRendererElement(Toc2 toc2) : base(toc2)
    {
        ClassName = toc2.StyleName;
    }
}