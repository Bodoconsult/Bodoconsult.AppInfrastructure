// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Toc4"/> instances
/// </summary>
public class Toc4DocxTextRendererElement : TocxDocxTextRendererElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public Toc4DocxTextRendererElement(Toc4 toc4) : base(toc4)
    {
        ClassName = toc4.StyleName;
    }
}