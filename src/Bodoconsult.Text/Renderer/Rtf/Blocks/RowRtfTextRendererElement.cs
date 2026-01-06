// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Rtf.Blocks;

/// <summary>
/// Rtf rendering element for <see cref="Row"/> instances
/// </summary>
public class RowRtfTextRendererElement : ITextRendererElement
{
    private readonly Row _row;

    /// <summary>
    /// Default ctor
    /// </summary>
    public RowRtfTextRendererElement(Row row) 
    {
        _row = row;
    }

    /// <summary>
    /// Cell background color
    /// </summary>
    public Color Color { get; set; }


    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public void RenderIt(ITextDocumentRenderer renderer)
    {
        renderer.Content.Append(@"\trowd\trautofit1\trqc\trpaddfb3\trpaddfr3\trpaddft3\trpaddfl3\trpaddb80\trpaddr80\trpaddt80\trpaddl80");

        //DocumentRendererHelper.RenderCellWidthsToRtf(renderer, _row.Cells);

        DocumentRendererHelper.RenderCellsToRtf(renderer, _row.Cells, Color);

        renderer.Content.Append($"\\row{Environment.NewLine}");
    }
}