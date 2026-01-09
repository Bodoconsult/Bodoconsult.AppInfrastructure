// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Toc1"/> instances
/// </summary>
public class Toc1DocxTextRendererElement : TocxDocxTextRendererElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public Toc1DocxTextRendererElement(Toc1 toc1) : base(toc1)
    {
        ClassName = toc1.StyleName;
    }



}