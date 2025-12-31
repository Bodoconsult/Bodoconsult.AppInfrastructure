// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Html.Styles;

/// <summary>
/// HTML rendering element for <see cref="CellCenterStyle"/> instances
/// </summary>
public class CellCenterStyleHtmlTextRendererElement : CellStyleHtmlTextRendererElement
{
    private readonly CellCenterStyle _cellCenterStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public CellCenterStyleHtmlTextRendererElement(CellCenterStyle cellCenterStyle) : base(cellCenterStyle)
    {
        _cellCenterStyle = cellCenterStyle;
        ClassName = "CellCenterStyle";
        
    }
}