// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;
using System;
using System.Diagnostics;
using System.Text;

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
            renderer.Content.Append($"\\colsx{MeasurementHelper.GetTwipsFromCm(docStyle.ColumnGap)}");
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

        if (!section.IsFooterRequired)
        {
            return;
        }

        var md = renderer.Document.DocumentMetaData;

        if (string.IsNullOrEmpty(md.HeaderTemplate))
        {
            return;
        }

        var style = (ParagraphStyleBase)renderer.Styleset.FindStyle("FooterStyle");

        var sections = md.HeaderTemplate.ToLowerInvariant().Split('|');

        var sb = new StringBuilder();

        sb.Append($"{{\\header{{\\pard\\plain{RtfHelper.GetFormatSettings(style, renderer.Styleset)}{{");

        // Draw left element
        CreateHeaderFooterElement(sb, md, sections[0], 0, true);

        // Draw middle element
        CreateHeaderFooterElement(sb, md, sections[1], 1, true);

        // Draw right element
        CreateHeaderFooterElement(sb, md, sections[2], 2, true);

        sb.Append("}\\par}}");

        Debug.Print(sb.ToString());

        renderer.Content.Append(sb);
    }

    private static void AddFooter(ITextDocumentRenderer renderer, PageStyleBase pageStyle, SectionBase section)
    {
        if (!section.IsFooterRequired)
        {
            return;
        }

        var md = renderer.Document.DocumentMetaData;

        if (string.IsNullOrEmpty(md.FooterTemplate))
        {
            return;
        }

        var style = (ParagraphStyleBase)renderer.Styleset.FindStyle("FooterStyle");

        var sections = md.FooterTemplate.ToLowerInvariant().Split('|');

        var sb = new StringBuilder();

        sb.Append($"{{\\footer{{\\pard\\plain{RtfHelper.GetFormatSettings(style, renderer.Styleset)}{{");

        // Draw left element
        CreateHeaderFooterElement(sb, md, sections[0],  0, false);

        // Draw middle element
        CreateHeaderFooterElement(sb, md, sections[1],  1, false);

        // Draw right element
        CreateHeaderFooterElement(sb, md, sections[2],  2, false);

        sb.Append("}\\par}}");

        renderer.Content.Append(sb);
    }

    private static void CreateHeaderFooterElement(StringBuilder content, ITypoMetaData documentMetaData, string section, int position, bool isHeader)
    {
        if (documentMetaData == null)
        {
            throw new ArgumentNullException(nameof(documentMetaData));
        }

        if (position == 1)
        {
            content.Append("{\\ptablnone\\pindtabqc}");
        }

        // Logo
        if (section == ITypography.LogoIndicator && !string.IsNullOrEmpty(documentMetaData?.LogoPath))
        {
            // Get the content of all inlines as string
            var sb = new StringBuilder();

            var width = MeasurementHelper.GetTwipsFromCm(documentMetaData.LogoWidth);
            var height = (int)(width / TypographicConstants.GoldenerSchnittRatio);

            // Add the image
            var bytes = ImageHelper.GetBytes(documentMetaData.LogoPath);

            var path = documentMetaData.LogoPath.ToLowerInvariant();

            sb.Append(@"{{\*\shppict\pict");

            if (path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
            {
                sb.Append("\\jpegblip");
            }
            else if (path.EndsWith(".png"))
            {
                sb.Append("\\pngblip");
            }
            else
            {
                throw new NotSupportedException("Unsupported image format. Use JPEG or PNG images!");
            }

            sb.Append($@"\picw{width}\pich{height}\picwgoal{width}\pichgoal{height}\bin{{");

            var str = BitConverter.ToString(bytes, 0).Replace("-", string.Empty);
            sb.Append(str);
            sb.Append("}}}");

            Debug.Print(sb.ToString());

            content.Append(sb);
        }

        // Footer / header text
        if (section == ITypography.TextIndicator)
        {
            var text = isHeader ? documentMetaData.HeaderText : documentMetaData.FooterText;

            if (string.IsNullOrEmpty(text))
            {
                text = documentMetaData.Title;
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }
            }

            content.Append($"{{{text}}}");
        }

        // Footer / header text
        if (section == ITypography.CompanyIndicator)
        {
            var text = documentMetaData.Company;

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            content.Append($"{{{documentMetaData.Company}}}");
        }

        // Page number
        if (section == ITypography.PageFieldIndicator)
        {

            content.Append($"{documentMetaData.PageNumberPrefix} {{\\field{{\\*\\fldinst PAGE}}}}");
        }

        // Date
        if (section == ITypography.DateIndicator)
        {
            var text = DateTime.Now.ToString("d", documentMetaData.CultureInfo);
            content.Append($"{{{text}}}");
        }

        // DateTime
        if (section == ITypography.DateTimeIndicator)
        {
            var text = DateTime.Now.ToString("g", documentMetaData.CultureInfo);
            content.Append($"{{{text}}}");
        }

        if (position == 1)
        {
            content.Append("{\\ptablnone\\pindtabqr}");
        }
    }

}