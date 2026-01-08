// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Text;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Rtf.Blocks;

/// <summary>
/// Rtf rendering element for <see cref="Tot"/> instances
/// </summary>
public abstract class ToxRtfTextRendererElementBase : RtfTextRendererElementBase
{
    private readonly ParagraphBase _tot;

    /// <summary>
    /// Default ctor
    /// </summary>
    protected ToxRtfTextRendererElementBase(ParagraphBase tot) : base(tot)
    {
        _tot = tot;
        ClassName = tot.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {
        // Get the content of all inlines as string
        var sb = new StringBuilder();

        var style = (ParagraphStyleBase)renderer.Styleset.FindStyle(_tot.StyleName);
        renderer.Content.Append($@"\pard\plain\q{renderer.Styleset.GetIndexOfStyle(Block.StyleName)}{RtfHelper.GetFormatSettings(style, renderer.Styleset)}");

        DocumentRendererHelper.RenderInlineChildsToRtf(renderer, sb, Block.ChildInlines);

        CleanRtfString(sb);

        renderer.Content.Append($"{{{{\\field{{\\*\\fldinst HYPERLINK \"#{_tot.TagName}\"}}{{\\fldrslt {sb}}}}}");

        renderer.Content.Append($"{{\\ptablnone\\pindtabqr}}{{\\field{{\\*\\fldinst PAGEREF {_tot.TagName} \\# \"0\"  \\* MERGEFORMAT  }}{{\\fldrslt 1}}}}");
        renderer.Content.Append($"\\par}}{Environment.NewLine}");
    }
}