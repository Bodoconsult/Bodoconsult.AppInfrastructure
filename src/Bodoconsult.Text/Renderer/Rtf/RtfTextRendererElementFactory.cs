// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Renderer.Rtf.Blocks;
using Bodoconsult.Text.Renderer.Rtf.Inlines;
using Bodoconsult.Text.Renderer.Rtf.Styles;
using DocumentMetaDataRtfTextRendererElement = Bodoconsult.Text.Renderer.Rtf.Blocks.DocumentMetaDataRtfTextRendererElement;
using StylesetRtfTextRendererElement = Bodoconsult.Text.Renderer.Rtf.Blocks.StylesetRtfTextRendererElement;

namespace Bodoconsult.Text.Renderer.Rtf;

/// <summary>
/// <see cref="ITextRendererElementFactory"/> implementation for Rtf text rendering
/// </summary>
public class RtfTextRendererElementFactory : ITextRendererElementFactory
{
    /// <summary>
    /// Create an instance of an <see cref="ITextRendererElement"/> for a given <see cref="TextElement"/>
    /// </summary>
    /// <param name="textElement">Given text element</param>
    /// <returns>Instance of an <see cref="ITextRendererElement"/></returns>
    public ITextRendererElement CreateInstance(DocumentElement textElement)
    {
        // Base elements
        if (textElement is Document document)
        {
            return new DocumentRtfTextRendererElement(document);
        }

        if (textElement is DocumentMetaData documentMetaData)
        {
            return new DocumentMetaDataRtfTextRendererElement(documentMetaData);
        }

        if (textElement is Styleset styleset)
        {
            return new StylesetRtfTextRendererElement(styleset);
        }

        if (textElement is Section section)
        {
            return new SectionRtfTextRendererElement(section);
        }

        if (textElement is TocSection tocSection)
        {
            return new TocSectionRtfTextRendererElement(tocSection);
        }

        if (textElement is ToeSection toeSection)
        {
            return new ToeSectionRtfTextRendererElement(toeSection);
        }

        if (textElement is TofSection tofSection)
        {
            return new TofSectionRtfTextRendererElement(tofSection);
        }

        if (textElement is TotSection totSection)
        {
            return new TotSectionRtfTextRendererElement(totSection);
        }

        // ParagraphBase based elements
        if (textElement is Citation citation)
        {
            return new CitationRtfTextRendererElement(citation);
        }

        if (textElement is Code code)
        {
            return new CodeRtfTextRendererElement(code);
        }

        if (textElement is Cell cell)
        {
            return new CellRtfTextRendererElement(cell);
        }

        if (textElement is Column column)
        {
            return new ColumnRtfTextRendererElement(column);
        }

        if (textElement is DefinitionList definitionList)
        {
            return new DefinitionListRtfTextRendererElement(definitionList);
        }

        if (textElement is DefinitionListTerm definitionListTerm)
        {
            return new DefinitionListTermRtfTextRendererElement(definitionListTerm);
        }

        if (textElement is DefinitionListItem definitionListItem)
        {
            return new DefinitionListItemRtfTextRendererElement(definitionListItem);
        }

        if (textElement is Equation equation)
        {
            return new EquationRtfTextRendererElement(equation);
        }

        if (textElement is Error error)
        {
            return new ErrorRtfTextRendererElement(error);
        }

        if (textElement is Figure figure)
        {
            return new FigureRtfTextRendererElement(figure);
        }

        if (textElement is Heading1 heading1)
        {
            return new Heading1RtfTextRendererElement(heading1);
        }

        if (textElement is Heading2 heading2)
        {
            return new Heading2RtfTextRendererElement(heading2);
        }

        if (textElement is Heading3 heading3)
        {
            return new Heading3RtfTextRendererElement(heading3);
        }

        if (textElement is Heading4 heading4)
        {
            return new Heading4RtfTextRendererElement(heading4);
        }

        if (textElement is Heading5 heading5)
        {
            return new Heading5RtfTextRendererElement(heading5);
        }

        if (textElement is Image image)
        {
            return new ImageRtfTextRendererElement(image);
        }

        if (textElement is Info info)
        {
            return new InfoRtfTextRendererElement(info);
        }

        if (textElement is List list)
        {
            return new ListRtfTextRendererElement(list);
        }

        if (textElement is ListItem listItem)
        {
            return new ListItemRtfTextRendererElement(listItem);
        }

        if (textElement is Paragraph paragraph)
        {
            return new ParagraphRtfTextRendererElement(paragraph);
        }

        if (textElement is ParagraphCenter paragraphCenter)
        {
            return new ParagraphCenterRtfTextRendererElement(paragraphCenter);
        }

        if (textElement is ParagraphJustify paragraphJustify)
        {
            return new ParagraphJustifyRtfTextRendererElement(paragraphJustify);
        }

        if (textElement is ParagraphRight paragraphRight)
        {
            return new ParagraphRightRtfTextRendererElement(paragraphRight);
        }

        if (textElement is Row row)
        {
            return new  RowRtfTextRendererElement(row);
        }

        if (textElement is SectionSubtitle sectionSubtitle)
        {
            return new SectionSubtitleRtfTextRendererElement(sectionSubtitle);
        }

        if (textElement is SectionTitle sectionTitle)
        {
            return new SectionTitleRtfTextRendererElement(sectionTitle);
        }

        if (textElement is Subtitle subtitle)
        {
            return new SubtitleRtfTextRendererElement(subtitle);
        }

        if (textElement is Table table)
        {
            return new TableRtfTextRendererElement(table);
        }

        if (textElement is Title title)
        {
            return new TitleRtfTextRendererElement(title);
        }

        if (textElement is Toc1 toc1)
        {
            return new Toc1RtfTextRendererElement(toc1);
        }

        if (textElement is Toc2 toc2)
        {
            return new Toc2RtfTextRendererElement(toc2);
        }

        if (textElement is Toc3 toc3)
        {
            return new Toc3RtfTextRendererElement(toc3);
        }

        if (textElement is Toc4 toc4)
        {
            return new Toc4RtfTextRendererElement(toc4);
        }

        if (textElement is Toc5 toc5)
        {
            return new Toc5RtfTextRendererElement(toc5);
        }

        if (textElement is Tof tof)
        {
            return new TofRtfTextRendererElement(tof);
        }

        if (textElement is Tot tot)
        {
            return new TotRtfTextRendererElement(tot);
        }

        if (textElement is Toe toe)
        {
            return new ToeRtfTextRendererElement(toe);
        }

        if (textElement is Warning warning)
        {
            return new WarningRtfTextRendererElement(warning);
        }

        // ToDo: add all others

        // Inline based elements
        if (textElement is Span span)
        {
            return new SpanRtfTextRendererElement(span);
        }

        if (textElement is Bold bold)
        {
            return new BoldRtfTextRendererElement(bold);
        }

        if (textElement is Italic italic)
        {
            return new ItalicRtfTextRendererElement(italic);
        }

        if (textElement is LineBreak lineBreak)
        {
            return new LineBreakRtfTextRendererElement(lineBreak);
        }

        if (textElement is Hyperlink hyperlink)
        {
            return new HyperlinkRtfTextRendererElement(hyperlink);
        }

        if (textElement is Value value)
        {
            return new ValueRtfTextRendererElement(value);
        }

        // Base styles
        if (textElement is DocumentStyle documentStyle)
        {
            return new DocumentStyleRtfTextRendererElement(documentStyle);
        }

        if (textElement is SectionStyle sectionStyle)
        {
            return new SectionStyleRtfTextRendererElement(sectionStyle);
        }

        if (textElement is TocSectionStyle tocSectionStyle)
        {
            return new TocSectionStyleRtfTextRendererElement(tocSectionStyle);
        }

        if (textElement is ToeSectionStyle toeSectionStyle)
        {
            return new ToeSectionStyleRtfTextRendererElement(toeSectionStyle);
        }

        if (textElement is TofSectionStyle tofSectionStyle)
        {
            return new TofSectionStyleRtfTextRendererElement(tofSectionStyle);
        }

        if (textElement is TotSectionStyle totSectionStyle)
        {
            return new TotSectionStyleRtfTextRendererElement(totSectionStyle);
        }

        // Paragraph style based
        if (textElement is CitationStyle citationStyle)
        {
            return new CitationStyleRtfTextRendererElement(citationStyle);
        }

        if (textElement is CitationSourceStyle citationSourceStyle)
        {
            return new CitationSourceStyleRtfTextRendererElement(citationSourceStyle);
        }

        if (textElement is CellLeftStyle cellLeftStyle)
        {
            return new CellLeftStyleRtfTextRendererElement(cellLeftStyle);
        }

        if (textElement is CellRightStyle cellRightStyle)
        {
            return new CellRightStyleRtfTextRendererElement(cellRightStyle);
        }

        if (textElement is CellCenterStyle cellCenterStyle)
        {
            return new CellCenterStyleRtfTextRendererElement(cellCenterStyle);
        }

        if (textElement is ColumnStyle columnStyle)
        {
            return new ColumnStyleRtfTextRendererElement(columnStyle);
        }

        if (textElement is CodeStyle codeStyle)
        {
            return new CodeStyleRtfTextRendererElement(codeStyle);
        }

        if (textElement is DefinitionListStyle definitionListStyle)
        {
            return new DefinitionListStyleRtfTextRendererElement(definitionListStyle);
        }

        if (textElement is DefinitionListTermStyle definitionListTermStyle)
        {
            return new DefinitionListTermStyleRtfTextRendererElement(definitionListTermStyle);
        }

        if (textElement is DefinitionListItemStyle definitionListItemStyle)
        {
            return new DefinitionListItemStyleRtfTextRendererElement(definitionListItemStyle);
        }

        if (textElement is EquationStyle equationStyle)
        {
            return new EquationStyleRtfTextRendererElement(equationStyle);
        }

        if (textElement is ErrorStyle errorStyle)
        {
            return new ErrorStyleRtfTextRendererElement(errorStyle);
        }

        if (textElement is FigureStyle figureStyle)
        {
            return new FigureStyleRtfTextRendererElement(figureStyle);
        }

        if (textElement is FooterStyle footerStyle)
        {
            return new FooterStyleRtfTextRendererElement(footerStyle);
        }

        if (textElement is HeaderStyle headerStyle)
        {
            return new HeaderStyleRtfTextRendererElement(headerStyle);
        }

        if (textElement is Heading1Style heading1Style)
        {
            return new Heading1StyleRtfTextRendererElement(heading1Style);
        }

        if (textElement is Heading2Style heading2Style)
        {
            return new Heading2StyleRtfTextRendererElement(heading2Style);
        }

        if (textElement is Heading3Style heading3Style)
        {
            return new Heading3StyleRtfTextRendererElement(heading3Style);
        }

        if (textElement is Heading4Style heading4Style)
        {
            return new Heading4StyleRtfTextRendererElement(heading4Style);
        }

        if (textElement is Heading5Style heading5Style)
        {
            return new Heading5StyleRtfTextRendererElement(heading5Style);
        }

        if (textElement is ImageStyle imageStyle)
        {
            return new ImageStyleRtfTextRendererElement(imageStyle);
        }

        if (textElement is InfoStyle infoStyle)
        {
            return new InfoStyleRtfTextRendererElement(infoStyle);
        }

        if (textElement is ListItemStyle listItemStyle)
        {
            return new ListItemStyleRtfTextRendererElement(listItemStyle);
        }

        if (textElement is ListStyle listStyle)
        {
            return new ListStyleRtfTextRendererElement(listStyle);
        }

        if (textElement is ParagraphCenterStyle paragraphCenterStyle)
        {
            return new ParagraphCenterStyleRtfTextRendererElement(paragraphCenterStyle);
        }

        if (textElement is ParagraphJustifyStyle paragraphJustifyStyle)
        {
            return new ParagraphJustifyStyleRtfTextRendererElement(paragraphJustifyStyle);
        }

        if (textElement is ParagraphRightStyle paragraphRightStyle)
        {
            return new ParagraphRightStyleRtfTextRendererElement(paragraphRightStyle);
        }

        if (textElement is ParagraphStyle paragraphStyle)
        {
            return new ParagraphStyleRtfTextRendererElement(paragraphStyle);
        }

        if (textElement is RowStyle rowStyle)
        {
            return new RowStyleRtfTextRendererElement(rowStyle);
        }

        if (textElement is SectionSubtitleStyle sectionSubtitleStyle)
        {
            return new SectionSubtitleStyleRtfTextRendererElement(sectionSubtitleStyle);
        }

        if (textElement is SectionTitleStyle sectionTitleStyle)
        {
            return new SectionTitleStyleRtfTextRendererElement(sectionTitleStyle);
        }

        if (textElement is SubtitleStyle subtitleStyle)
        {
            return new SubtitleStyleRtfTextRendererElement(subtitleStyle);
        }

        if (textElement is TableStyle tableStyle)
        {
            return new TableStyleRtfTextRendererElement(tableStyle);
        }

        if (textElement is TableHeaderLeftStyle tableHeaderLeftStyle)
        {
            return new TableHeaderLeftStyleRtfTextRendererElement(tableHeaderLeftStyle);
        }

        if (textElement is TableHeaderRightStyle tableHeaderRightStyle)
        {
            return new TableHeaderRightStyleRtfTextRendererElement(tableHeaderRightStyle);
        }

        if (textElement is TableHeaderCenterStyle tableHeaderCenterStyle)
        {
            return new TableHeaderCenterStyleRtfTextRendererElement(tableHeaderCenterStyle);
        }

        if (textElement is TableLegendStyle tableLegendStyle)
        {
            return new TableLegendStyleRtfTextRendererElement(tableLegendStyle);
        }

        if (textElement is TitleStyle titleStyle)
        {
            return new TitleStyleRtfTextRendererElement(titleStyle);
        }

        if (textElement is Toc1Style toc1Style)
        {
            return new Toc1StyleRtfTextRendererElement(toc1Style);
        }

        if (textElement is Toc2Style toc2Style)
        {
            return new Toc2StyleRtfTextRendererElement(toc2Style);
        }

        if (textElement is Toc3Style toc3Style)
        {
            return new Toc3StyleRtfTextRendererElement(toc3Style);
        }

        if (textElement is Toc4Style toc4Style)
        {
            return new Toc4StyleRtfTextRendererElement(toc4Style);
        }

        if (textElement is Toc5Style toc5Style)
        {
            return new Toc5StyleRtfTextRendererElement(toc5Style);
        }

        if (textElement is ToeStyle toeStyle)
        {
            return new ToeStyleRtfTextRendererElement(toeStyle);
        }

        if (textElement is TofStyle tofStyle)
        {
            return new TofStyleRtfTextRendererElement(tofStyle);
        }

        if (textElement is TotStyle totStyle)
        {
            return new TotStyleRtfTextRendererElement(totStyle);
        }

        if (textElement is TocHeadingStyle tocHeadingStyle)
        {
            return new TocHeadingStyleRtfTextRendererElement(tocHeadingStyle);
        }

        if (textElement is ToeHeadingStyle toeHeadingStyle)
        {
            return new ToeHeadingStyleRtfTextRendererElement(toeHeadingStyle);
        }

        if (textElement is TofHeadingStyle tofHeadingStyle)
        {
            return new TofHeadingStyleRtfTextRendererElement(tofHeadingStyle);
        }

        if (textElement is TotHeadingStyle totHeadingStyle)
        {
            return new TotHeadingStyleRtfTextRendererElement(totHeadingStyle);
        }

        if (textElement is WarningStyle warningStyle)
        {
            return new WarningStyleRtfTextRendererElement(warningStyle);
        }

        if (textElement is WatermarkStyle watermarkStyle)
        {
            return new WatermarkStyleRtfTextRendererElement(watermarkStyle);
        }

        return null;
    }

}