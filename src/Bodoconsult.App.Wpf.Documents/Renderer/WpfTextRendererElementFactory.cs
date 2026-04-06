// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.Documents.Interfaces;
using Bodoconsult.App.Wpf.Documents.Renderer.Blocks;
using Bodoconsult.App.Wpf.Documents.Renderer.Inlines;
using Bodoconsult.App.Wpf.Documents.Renderer.Styles;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.App.Wpf.Documents.Renderer;

/// <summary>
/// <see cref="ITextRendererElementFactory"/> implementation for Wpf text rendering
/// </summary>
public class WpfTextRendererElementFactory : IWpfTextRendererElementFactory
{
    /// <summary>
    /// Create an instance of an <see cref="ITextRendererElement"/> for a given <see cref="TextElement"/>
    /// </summary>
    /// <param name="textElement">Given text element</param>
    /// <returns>Instance of an <see cref="ITextRendererElement"/></returns>
    public ITextRendererElement CreateInstance(DocumentElement textElement)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create an instance of an <see cref="IWpfTextRendererElement"/> for a given <see cref="TextElement"/>
    /// </summary>
    /// <param name="textElement">Given text element</param>
    /// <returns>Instance of an <see cref="IWpfTextRendererElement"/></returns>
    public IWpfTextRendererElement CreateInstanceWpf(DocumentElement textElement)
    {
        // Base elements
        if (textElement is Document document)
        {
            return new DocumentWpfTextRendererElement(document);
        }

        if (textElement is DocumentMetaData documentMetaData)
        {
            return new DocumentMetaDataWpfTextRendererElement(documentMetaData);
        }

        if (textElement is Styleset styleset)
        {
            return new StylesetWpfTextRendererElement(styleset);
        }

        if (textElement is Section section)
        {
            return new SectionWpfTextRendererElement(section);
        }

        if (textElement is TocSection tocSection)
        {
            return new TocSectionWpfTextRendererElement(tocSection);
        }

        if (textElement is ToeSection toeSection)
        {
            return new ToeSectionWpfTextRendererElement(toeSection);
        }

        if (textElement is TofSection tofSection)
        {
            return new TofSectionWpfTextRendererElement(tofSection);
        }

        if (textElement is TotSection totSection)
        {
            return new TotSectionWpfTextRendererElement(totSection);
        }

        // ParagraphBase based elements
        if (textElement is Citation citation)
        {
            return new CitationWpfTextRendererElement(citation);
        }

        if (textElement is Code code)
        {
            return new CodeWpfTextRendererElement(code);
        }

        if (textElement is Cell cell)
        {
            return new CellWpfTextRendererElement(cell);
        }

        if (textElement is Column column)
        {
            return new ColumnWpfTextRendererElement(column);
        }

        if (textElement is DefinitionList definitionList)
        {
            return new DefinitionListWpfTextRendererElement(definitionList);
        }

        if (textElement is DefinitionListTerm definitionListTerm)
        {
            return new DefinitionListTermWpfTextRendererElement(definitionListTerm);
        }

        if (textElement is DefinitionListItem definitionListItem)
        {
            return new DefinitionListItemWpfTextRendererElement(definitionListItem);
        }

        if (textElement is Equation equation)
        {
            return new EquationWpfTextRendererElement(equation);
        }

        if (textElement is Error error)
        {
            return new ErrorWpfTextRendererElement(error);
        }

        if (textElement is Figure figure)
        {
            return new FigureWpfTextRendererElement(figure);
        }

        if (textElement is Heading1 heading1)
        {
            return new Heading1WpfTextRendererElement(heading1);
        }

        if (textElement is Heading2 heading2)
        {
            return new Heading2WpfTextRendererElement(heading2);
        }

        if (textElement is Heading3 heading3)
        {
            return new Heading3WpfTextRendererElement(heading3);
        }

        if (textElement is Heading4 heading4)
        {
            return new Heading4WpfTextRendererElement(heading4);
        }

        if (textElement is Heading5 heading5)
        {
            return new Heading5WpfTextRendererElement(heading5);
        }

        if (textElement is Image image)
        {
            return new ImageWpfTextRendererElement(image);
        }

        if (textElement is Info info)
        {
            return new InfoWpfTextRendererElement(info);
        }

        if (textElement is List list)
        {
            return new ListWpfTextRendererElement(list);
        }

        if (textElement is ListItem listItem)
        {
            return new ListItemWpfTextRendererElement(listItem);
        }

        if (textElement is Paragraph paragraph)
        {
            return new ParagraphWpfTextRendererElement(paragraph);
        }

        if (textElement is ParagraphCenter paragraphCenter)
        {
            return new ParagraphCenterWpfTextRendererElement(paragraphCenter);
        }

        if (textElement is ParagraphJustify paragraphJustify)
        {
            return new ParagraphJustifyWpfTextRendererElement(paragraphJustify);
        }

        if (textElement is ParagraphRight paragraphRight)
        {
            return new ParagraphRightWpfTextRendererElement(paragraphRight);
        }

        if (textElement is Row row)
        {
            return new RowWpfTextRendererElement(row);
        }

        if (textElement is SectionSubtitle sectionSubtitle)
        {
            return new SectionSubtitleWpfTextRendererElement(sectionSubtitle);
        }

        if (textElement is SectionTitle sectionTitle)
        {
            return new SectionTitleWpfTextRendererElement(sectionTitle);
        }

        if (textElement is Subtitle subtitle)
        {
            return new SubtitleWpfTextRendererElement(subtitle);
        }

        if (textElement is Table table)
        {
            return new TableWpfTextRendererElement(table);
        }

        if (textElement is Title title)
        {
            return new TitleWpfTextRendererElement(title);
        }

        if (textElement is Toc1 toc1)
        {
            return new Toc1WpfTextRendererElement(toc1);
        }

        if (textElement is Toc2 toc2)
        {
            return new Toc2WpfTextRendererElement(toc2);
        }

        if (textElement is Toc3 toc3)
        {
            return new Toc3WpfTextRendererElement(toc3);
        }

        if (textElement is Toc4 toc4)
        {
            return new Toc4WpfTextRendererElement(toc4);
        }

        if (textElement is Toc5 toc5)
        {
            return new Toc5WpfTextRendererElement(toc5);
        }

        if (textElement is Toe toe)
        {
            return new ToeWpfTextRendererElement(toe);
        }

        if (textElement is Tof tof)
        {
            return new TofWpfTextRendererElement(tof);
        }

        if (textElement is Tot tot)
        {
            return new TotWpfTextRendererElement(tot);
        }

        if (textElement is Warning warning)
        {
            return new WarningWpfTextRendererElement(warning);
        }

        // ToDo: add all others

        // Inline based elements
        if (textElement is Span span)
        {
            return new SpanWpfTextRendererElement(span);
        }

        if (textElement is Bold bold)
        {
            return new BoldWpfTextRendererElement(bold);
        }

        if (textElement is Italic italic)
        {
            return new ItalicWpfTextRendererElement(italic);
        }

        if (textElement is LineBreak lineBreak)
        {
            return new LineBreakWpfTextRendererElement(lineBreak);
        }

        if (textElement is Hyperlink hyperlink)
        {
            return new HyperlinkWpfTextRendererElement(hyperlink);
        }

        if (textElement is Value value)
        {
            return new ValueWpfTextRendererElement(value);
        }

        // Base styles
        if (textElement is DocumentStyle documentStyle)
        {
            return new DocumentStyleWpfTextRendererElement(documentStyle);
        }

        if (textElement is SectionStyle sectionStyle)
        {
            return new SectionStyleWpfTextRendererElement(sectionStyle);
        }

        if (textElement is TocSectionStyle tocSectionStyle)
        {
            return new TocSectionStyleWpfTextRendererElement(tocSectionStyle);
        }

        if (textElement is ToeSectionStyle toeSectionStyle)
        {
            return new ToeSectionStyleWpfTextRendererElement(toeSectionStyle);
        }

        if (textElement is TofSectionStyle tofSectionStyle)
        {
            return new TofSectionStyleWpfTextRendererElement(tofSectionStyle);
        }

        if (textElement is TotSectionStyle totSectionStyle)
        {
            return new TotSectionStyleWpfTextRendererElement(totSectionStyle);
        }

        if (textElement is CellLeftStyle cellLeftStyle)
        {
            return new CellLeftStyleWpfTextRendererElement(cellLeftStyle);
        }

        if (textElement is CellRightStyle cellRightStyle)
        {
            return new CellRightStyleWpfTextRendererElement(cellRightStyle);
        }

        if (textElement is CellCenterStyle cellCenterStyle)
        {
            return new CellCenterStyleWpfTextRendererElement(cellCenterStyle);
        }

        if (textElement is ColumnStyle columnStyle)
        {
            return new ColumnStyleWpfTextRendererElement(columnStyle);
        }

        // Paragraph style based
        if (textElement is CitationStyle citationStyle)
        {
            return new CitationStyleWpfTextRendererElement(citationStyle);
        }

        if (textElement is CitationSourceStyle citationSourceStyle)
        {
            return new CitationSourceStyleWpfTextRendererElement(citationSourceStyle);
        }

        if (textElement is CodeStyle codeStyle)
        {
            return new CodeStyleWpfTextRendererElement(codeStyle);
        }

        if (textElement is WatermarkStyle waterMarkStyle)
        {
            return new WatermarkStyleWpfTextRendererElement(waterMarkStyle);
        }

        if (textElement is DefinitionListStyle definitionListStyle)
        {
            return new DefinitionListStyleWpfTextRendererElement(definitionListStyle);
        }

        if (textElement is DefinitionListTermStyle definitionListTermStyle)
        {
            return new DefinitionListTermStyleWpfTextRendererElement(definitionListTermStyle);
        }

        if (textElement is DefinitionListItemStyle definitionListItemStyle)
        {
            return new DefinitionListItemStyleWpfTextRendererElement(definitionListItemStyle);
        }

        if (textElement is EquationStyle equationStyle)
        {
            return new EquationStyleWpfTextRendererElement(equationStyle);
        }

        if (textElement is ErrorStyle errorStyle)
        {
            return new ErrorStyleWpfTextRendererElement(errorStyle);
        }

        if (textElement is FigureStyle figureStyle)
        {
            return new FigureStyleWpfTextRendererElement(figureStyle);
        }

        if (textElement is FooterStyle footerStyle)
        {
            return new FooterStyleWpfTextRendererElement(footerStyle);
        }

        if (textElement is HeaderStyle headerStyle)
        {
            return new HeaderStyleWpfTextRendererElement(headerStyle);
        }

        if (textElement is Heading1Style heading1Style)
        {
            return new Heading1StyleWpfTextRendererElement(heading1Style);
        }

        if (textElement is Heading2Style heading2Style)
        {
            return new Heading2StyleWpfTextRendererElement(heading2Style);
        }

        if (textElement is Heading3Style heading3Style)
        {
            return new Heading3StyleWpfTextRendererElement(heading3Style);
        }

        if (textElement is Heading4Style heading4Style)
        {
            return new Heading4StyleWpfTextRendererElement(heading4Style);
        }

        if (textElement is Heading5Style heading5Style)
        {
            return new Heading5StyleWpfTextRendererElement(heading5Style);
        }

        if (textElement is ImageStyle imageStyle)
        {
            return new ImageStyleWpfTextRendererElement(imageStyle);
        }

        if (textElement is InfoStyle infoStyle)
        {
            return new InfoStyleWpfTextRendererElement(infoStyle);
        }

        if (textElement is ListItemStyle listItemStyle)
        {
            return new ListItemStyleWpfTextRendererElement(listItemStyle);
        }

        if (textElement is ListStyle listStyle)
        {
            return new ListStyleWpfTextRendererElement(listStyle);
        }

        if (textElement is ParagraphCenterStyle paragraphCenterStyle)
        {
            return new ParagraphCenterStyleWpfTextRendererElement(paragraphCenterStyle);
        }

        if (textElement is ParagraphJustifyStyle paragraphJustifyStyle)
        {
            return new ParagraphJustifyStyleWpfTextRendererElement(paragraphJustifyStyle);
        }

        if (textElement is ParagraphRightStyle paragraphRightStyle)
        {
            return new ParagraphRightStyleWpfTextRendererElement(paragraphRightStyle);
        }

        if (textElement is ParagraphStyle paragraphStyle)
        {
            return new ParagraphStyleWpfTextRendererElement(paragraphStyle);
        }

        if (textElement is RowStyle rowStyle)
        {
            return new RowStyleWpfTextRendererElement(rowStyle);
        }

        if (textElement is SectionSubtitleStyle sectionSubtitleStyle)
        {
            return new SectionSubtitleStyleWpfTextRendererElement(sectionSubtitleStyle);
        }

        if (textElement is SectionTitleStyle sectionTitleStyle)
        {
            return new SectionTitleStyleWpfTextRendererElement(sectionTitleStyle);
        }

        if (textElement is SubtitleStyle subtitleStyle)
        {
            return new SubtitleStyleWpfTextRendererElement(subtitleStyle);
        }

        if (textElement is TableStyle tableStyle)
        {
            return new TableStyleWpfTextRendererElement(tableStyle);
        }

        if (textElement is TableHeaderLeftStyle tableHeaderLeftStyle)
        {
            return new TableHeaderLeftStyleWpfTextRendererElement(tableHeaderLeftStyle);
        }

        if (textElement is TableHeaderRightStyle tableHeaderRightStyle)
        {
            return new TableHeaderRightStyleWpfTextRendererElement(tableHeaderRightStyle);
        }

        if (textElement is TableHeaderCenterStyle tableHeaderCenterStyle)
        {
            return new TableHeaderCenterStyleWpfTextRendererElement(tableHeaderCenterStyle);
        }

        if (textElement is TableLegendStyle tableLegendStyle)
        {
            return new TableLegendStyleWpfTextRendererElement(tableLegendStyle);
        }

        if (textElement is TitleStyle titleStyle)
        {
            return new TitleStyleWpfTextRendererElement(titleStyle);
        }

        if (textElement is Toc1Style toc1Style)
        {
            return new Toc1StyleWpfTextRendererElement(toc1Style);
        }

        if (textElement is Toc2Style toc2Style)
        {
            return new Toc2StyleWpfTextRendererElement(toc2Style);
        }

        if (textElement is Toc3Style toc3Style)
        {
            return new Toc3StyleWpfTextRendererElement(toc3Style);
        }

        if (textElement is Toc4Style toc4Style)
        {
            return new Toc4StyleWpfTextRendererElement(toc4Style);
        }

        if (textElement is Toc5Style toc5Style)
        {
            return new Toc5StyleWpfTextRendererElement(toc5Style);
        }

        if (textElement is ToeStyle toeStyle)
        {
            return new ToeStyleWpfTextRendererElement(toeStyle);
        }

        if (textElement is TofStyle tofStyle)
        {
            return new TofStyleWpfTextRendererElement(tofStyle);
        }

        if (textElement is TotStyle totStyle)
        {
            return new TotStyleWpfTextRendererElement(totStyle);
        }

        if (textElement is TocHeadingStyle tocHeadingStyle)
        {
            return new TocHeadingStyleWpfTextRendererElement(tocHeadingStyle);
        }

        if (textElement is TofHeadingStyle tofHeadingStyle)
        {
            return new TofHeadingStyleWpfTextRendererElement(tofHeadingStyle);
        }

        if (textElement is ToeHeadingStyle toeHeadingStyle)
        {
            return new ToeHeadingStyleWpfTextRendererElement(toeHeadingStyle);
        }


        if (textElement is TotHeadingStyle totHeadingStyle)
        {
            return new TotHeadingStyleWpfTextRendererElement(totHeadingStyle);
        }

        if (textElement is WarningStyle warningStyle)
        {
            return new WarningStyleWpfTextRendererElement(warningStyle);
        }

        return null;

    }
}