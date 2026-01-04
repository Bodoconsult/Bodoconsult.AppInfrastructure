// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Text;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Rtf.Blocks;

/// <summary>
/// Rtf rendering element for <see cref="SectionBase"/> based instances
/// </summary>
public abstract class SectionBaseRtfTextRendererElement : RtfTextRendererElementBase
{
    private readonly SectionBase _section;

    /// <summary>
    /// Default ctor
    /// </summary>
    protected SectionBaseRtfTextRendererElement(SectionBase section) : base(section)
    {
        _section = section;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    /// <param name="styleName">Stylename to use for caption</param>
    /// <param name="caption">Caption</param>
    public void RenderItInternal(ITextDocumentRenderer renderer, string styleName, string caption)
    {
        if (_section.ChildBlocks.Count == 0)
        {
            return;
        }
        // Page break before?
        if (!_section.IsFirstSection)
        {
            renderer.Content.Append(_section.PageBreakBefore ? @"\sect\sectd\sbkpage" : @"\sect\sectd\sbknone");
            renderer.Content.Append(_section.IsRestartPageNumberingRequired ? @"\pgnstarts1\pgnrestart" : @"\pgncont");
        }

        // Set page numbering format
        switch (_section.PageNumberFormat)
        {
            case PageNumberFormatEnum.UpperRoman:
                renderer.Content.Append("\\pgnucrm");
                break;
            case PageNumberFormatEnum.LowerRoman:
                renderer.Content.Append("\\pgnlcrm");
                break;
            case PageNumberFormatEnum.UpperLatin:
                renderer.Content.Append("\\pgnucltr");
                break;
            case PageNumberFormatEnum.LowerLatin:
                renderer.Content.Append("\\pgnlcltr");
                break;
            default:
                renderer.Content.Append("\\pgndec");
                break;
        }

        var docStyle = (PageStyleBase)renderer.Document.Styleset.FindStyle("DocumentStyle");

        // Columns
        if (docStyle.NumberOfColumns > 1)
        {
            renderer.Content.Append($"\\cols{docStyle.NumberOfColumns}");
            renderer.Content.Append($"\\colsx{MeasurementHelper.GetTwipsFromCm(docStyle.Space)}");
        }

        renderer.Content.Append('{');

        

        var pageStyle = (PageStyleBase)renderer.Styleset.FindStyle(_section.StyleName);
        AddFooter(renderer, pageStyle, _section);
        AddHeader(renderer, pageStyle, _section);

        if (string.IsNullOrEmpty(caption))
        {
            base.RenderIt(renderer);

            renderer.Content.Append('}');

            return;
        }

        // Add heading if necessary

        // Get the content of all inlines as string
        var style = (ParagraphStyleBase)renderer.Styleset.FindStyle(styleName);
        renderer.Content.Append($@"\pard\plain\q{renderer.Styleset.GetIndexOfStyle(Block.StyleName)} {RtfHelper.GetFormatSettings(style, renderer.Styleset)}{{");

        var sb = new StringBuilder(renderer.CheckContent(caption));
        CleanRtfString(sb);
        renderer.Content.Append(sb);
        renderer.Content.Append($"\\par}}{Environment.NewLine}");

        base.RenderIt(renderer);

        renderer.Content.Append('}');

    }

    private static void AddHeader(ITextDocumentRenderer renderer, PageStyleBase pageStyle, SectionBase section)
    {
        if (!section.IsHeaderRequired)
        {
            return;
        }

        // Todo: add logo

        var style = (ParagraphStyleBase)renderer.Styleset.FindStyle("HeaderStyle");

        renderer.Content.Append($@"{{\header{{\pard\plain{RtfHelper.GetFormatSettings(style, renderer.Styleset)}{{{renderer.Document.DocumentMetaData.HeaderText}}}\par}}}}");
    }

    private static void AddFooter(ITextDocumentRenderer renderer, PageStyleBase pageStyle, SectionBase section)
    {
        if (!section.IsFooterRequired)
        {
            return;
        }

        // /{\field{\*\fldinst SECTIONPAGES'}}

        var style = (ParagraphStyleBase)renderer.Styleset.FindStyle("FooterStyle");

        renderer.Content.Append($"{{\\footer{{\\pard\\plain{RtfHelper.GetFormatSettings(style, renderer.Styleset)}{{{renderer.Document.DocumentMetaData.Company} {{\\ptablnone\\pindtabqr}}{renderer.Document.DocumentMetaData.PageNumberPrefix} {{\\field{{\\*\\fldinst PAGE}}}}}}\\par}}}}");
    }
}