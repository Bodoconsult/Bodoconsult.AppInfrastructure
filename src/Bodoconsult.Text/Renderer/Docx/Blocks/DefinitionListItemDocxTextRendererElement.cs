// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="DefinitionListItem"/> instances
/// </summary>
public class DefinitionListItemDocxTextRendererElement : DocxTextRendererElementBase
{
    private readonly DefinitionListItem _item;

    /// <summary>
    /// Default ctor
    /// </summary>
    public DefinitionListItemDocxTextRendererElement(DefinitionListItem item) : base(item)
    {
        _item = item;
        ClassName = item.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(DocxTextDocumentRenderer renderer)
    {
        // Do nothing
    }
}