// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Rtf.Blocks;

/// <summary>
/// Rtf rendering element for <see cref="HeadingBase"/> instances
/// </summary>
public class HeadingBaseRtfTextRendererElement : RtfTextRendererElementBase
{
    private readonly HeadingBase _headingBase;

    /// <summary>
    /// Default ctor
    /// </summary>
    public HeadingBaseRtfTextRendererElement(HeadingBase headingBase) : base(headingBase)
    {
        _headingBase = headingBase;
        ClassName = headingBase.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {
        // Get the content of all inlines as string
        var sb = new StringBuilder();

        var style = (ParagraphStyleBase)renderer.Styleset.FindStyle(_headingBase.StyleName);
        renderer.Content.Append($@"\pard\plain\q{renderer.Styleset.GetIndexOfStyle(Block.StyleName)} {RtfHelper.GetFormatSettings(style, renderer.Styleset)}");

        var childs = new List<Inline>();

        if (!string.IsNullOrEmpty(_headingBase.CurrentPrefix))
        {
            childs.Add(new Span(_headingBase.CurrentPrefix));
        }

        childs.AddRange(_headingBase.ChildInlines);

        DocumentRendererHelper.RenderInlineChildsToRtf(renderer, sb, childs);

        CleanRtfString(sb);

        renderer.Content.Append($@"{{{{\*\bkmkstart {_headingBase.TagName}}}{{{sb}}}{{\*\bkmkend {_headingBase.TagName}}}}}");
        renderer.Content.Append($"\\par{Environment.NewLine}");
    }
}