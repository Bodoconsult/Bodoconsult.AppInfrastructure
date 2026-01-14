// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Renderer.Pdf.Blocks;
using Bodoconsult.Text.Renderer.Pdf.Inlines;
using Bodoconsult.Text.Renderer.Pdf.Styles;

namespace Bodoconsult.Text.Renderer.Pdf;

/// <summary>
/// <see cref="ITextRendererElementFactory"/> implementation for PDF text rendering
/// </summary>
public class PdfTextRendererElementFactory : IPdfTextRendererElementFactory
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
    /// Create an instance of an <see cref="IPdfTextRendererElement"/> for a given <see cref="TextElement"/>
    /// </summary>
    /// <param name="textElement">Given text element</param>
    /// <returns>Instance of an <see cref="IPdfTextRendererElement"/></returns>
    public IPdfTextRendererElement CreateInstancePdf(DocumentElement textElement)
    {
        // Base elements
        if (textElement is Document document)
        {
            return new DocumentPdfTextRendererElement(document);
        }

        if (textElement is DocumentMetaData documentMetaData)
        {
            return new DocumentMetaDataPdfTextRendererElement(documentMetaData);
        }

        if (textElement is Styleset styleset)
        {
            return new StylesetPdfTextRendererElement(styleset);
        }

        if (textElement is Section section)
        {
            return new SectionPdfTextRendererElement(section);
        }

        if (textElement is TocSection tocSection)
        {
            return new TocSectionPdfTextRendererElement(tocSection);
        }

        if (textElement is ToeSection toeSection)
        {
            return new ToeSectionPdfTextRendererElement(toeSection);
        }

        if (textElement is TofSection tofSection)
        {
            return new TofSectionPdfTextRendererElement(tofSection);
        }

        if (textElement is TotSection totSection)
        {
            return new TotSectionPdfTextRendererElement(totSection);
        }

        // ParagraphBase based elements
        if (textElement is Citation citation)
        {
            return new CitationPdfTextRendererElement(citation);
        }

        if (textElement is Code code)
        {
            return new CodePdfTextRendererElement(code);
        }

        if (textElement is Cell cell)
        {
            return new CellPdfTextRendererElement(cell);
        }

        if (textElement is Column column)
        {
            return new ColumnPdfTextRendererElement(column);
        }

        if (textElement is DefinitionList definitionList)
        {
            return new DefinitionListPdfTextRendererElement(definitionList);
        }

        if (textElement is DefinitionListTerm definitionListTerm)
        {
            return new DefinitionListTermPdfTextRendererElement(definitionListTerm);
        }

        if (textElement is DefinitionListItem definitionListItem)
        {
            return new DefinitionListItemPdfTextRendererElement(definitionListItem);
        }

        if (textElement is Equation equation)
        {
            return new EquationPdfTextRendererElement(equation);
        }

        if (textElement is Error error)
        {
            return new ErrorPdfTextRendererElement(error);
        }

        if (textElement is Figure figure)
        {
            return new FigurePdfTextRendererElement(figure);
        }

        if (textElement is Heading1 heading1)
        {
            return new Heading1PdfTextRendererElement(heading1);
        }

        if (textElement is Heading2 heading2)
        {
            return new Heading2PdfTextRendererElement(heading2);
        }

        if (textElement is Heading3 heading3)
        {
            return new Heading3PdfTextRendererElement(heading3);
        }

        if (textElement is Heading4 heading4)
        {
            return new Heading4PdfTextRendererElement(heading4);
        }

        if (textElement is Heading5 heading5)
        {
            return new Heading5PdfTextRendererElement(heading5);
        }

        if (textElement is Image image)
        {
            return new ImagePdfTextRendererElement(image);
        }

        if (textElement is Info info)
        {
            return new InfoPdfTextRendererElement(info);
        }

        if (textElement is List list)
        {
            return new ListPdfTextRendererElement(list);
        }

        if (textElement is ListItem listItem)
        {
            return new ListItemPdfTextRendererElement(listItem);
        }

        if (textElement is Paragraph paragraph)
        {
            return new ParagraphPdfTextRendererElement(paragraph);
        }

        if (textElement is ParagraphCenter paragraphCenter)
        {
            return new ParagraphCenterPdfTextRendererElement(paragraphCenter);
        }

        if (textElement is ParagraphJustify paragraphJustify)
        {
            return new ParagraphJustifyPdfTextRendererElement(paragraphJustify);
        }

        if (textElement is ParagraphRight paragraphRight)
        {
            return new ParagraphRightPdfTextRendererElement(paragraphRight);
        }

        if (textElement is Row row)
        {
            return new RowPdfTextRendererElement(row);
        }

        if (textElement is SectionSubtitle sectionSubtitle)
        {
            return new SectionSubtitlePdfTextRendererElement(sectionSubtitle);
        }

        if (textElement is SectionTitle sectionTitle)
        {
            return new SectionTitlePdfTextRendererElement(sectionTitle);
        }

        if (textElement is Subtitle subtitle)
        {
            return new SubtitlePdfTextRendererElement(subtitle);
        }

        if (textElement is Table table)
        {
            return new TablePdfTextRendererElement(table);
        }

        if (textElement is Title title)
        {
            return new TitlePdfTextRendererElement(title);
        }

        if (textElement is Toc1 toc1)
        {
            return new Toc1PdfTextRendererElement(toc1);
        }

        if (textElement is Toc2 toc2)
        {
            return new Toc2PdfTextRendererElement(toc2);
        }

        if (textElement is Toc3 toc3)
        {
            return new Toc3PdfTextRendererElement(toc3);
        }

        if (textElement is Toc4 toc4)
        {
            return new Toc4PdfTextRendererElement(toc4);
        }

        if (textElement is Toc5 toc5)
        {
            return new Toc5PdfTextRendererElement(toc5);
        }

        if (textElement is Toe toe)
        {
            return new ToePdfTextRendererElement(toe);
        }

        if (textElement is Tof tof)
        {
            return new TofPdfTextRendererElement(tof);
        }

        if (textElement is Tot tot)
        {
            return new TotPdfTextRendererElement(tot);
        }

        if (textElement is Warning warning)
        {
            return new WarningPdfTextRendererElement(warning);
        }

        // ToDo: add all others

        // Inline based elements
        if (textElement is Span span)
        {
            return new SpanPdfTextRendererElement(span);
        }

        if (textElement is Bold bold)
        {
            return new BoldPdfTextRendererElement(bold);
        }

        if (textElement is Italic italic)
        {
            return new ItalicPdfTextRendererElement(italic);
        }

        if (textElement is LineBreak lineBreak)
        {
            return new LineBreakPdfTextRendererElement(lineBreak);
        }

        if (textElement is Hyperlink hyperlink)
        {
            return new HyperlinkPdfTextRendererElement(hyperlink);
        }

        if (textElement is Value value)
        {
            return new ValuePdfTextRendererElement(value);
        }

        // Base styles
        if (textElement is DocumentStyle documentStyle)
        {
            return new DocumentStylePdfTextRendererElement(documentStyle);
        }

        if (textElement is SectionStyle sectionStyle)
        {
            return new SectionStylePdfTextRendererElement(sectionStyle);
        }

        if (textElement is TocSectionStyle tocSectionStyle)
        {
            return new TocSectionStylePdfTextRendererElement(tocSectionStyle);
        }

        if (textElement is ToeSectionStyle toeSectionStyle)
        {
            return new ToeSectionStylePdfTextRendererElement(toeSectionStyle);
        }

        if (textElement is TofSectionStyle tofSectionStyle)
        {
            return new TofSectionStylePdfTextRendererElement(tofSectionStyle);
        }

        if (textElement is TotSectionStyle totSectionStyle)
        {
            return new TotSectionStylePdfTextRendererElement(totSectionStyle);
        }

        if (textElement is CellLeftStyle cellLeftStyle)
        {
            return new CellLeftStylePdfTextRendererElement(cellLeftStyle);
        }

        if (textElement is CellRightStyle cellRightStyle)
        {
            return new CellRightStylePdfTextRendererElement(cellRightStyle);
        }

        if (textElement is CellCenterStyle cellCenterStyle)
        {
            return new CellCenterStylePdfTextRendererElement(cellCenterStyle);
        }

        if (textElement is ColumnStyle columnStyle)
        {
            return new ColumnStylePdfTextRendererElement(columnStyle);
        }

        // Paragraph style based
        if (textElement is CitationStyle citationStyle)
        {
            return new CitationStylePdfTextRendererElement(citationStyle);
        }

        if (textElement is CitationSourceStyle citationSourceStyle)
        {
            return new CitationSourceStylePdfTextRendererElement(citationSourceStyle);
        }

        if (textElement is CodeStyle codeStyle)
        {
            return new CodeStylePdfTextRendererElement(codeStyle);
        }

        if (textElement is DefinitionListStyle definitionListStyle)
        {
            return new DefinitionListStylePdfTextRendererElement(definitionListStyle);
        }

        if (textElement is DefinitionListTermStyle definitionListTermStyle)
        {
            return new DefinitionListTermStylePdfTextRendererElement(definitionListTermStyle);
        }

        if (textElement is DefinitionListItemStyle definitionListItemStyle)
        {
            return new DefinitionListItemStylePdfTextRendererElement(definitionListItemStyle);
        }

        if (textElement is EquationStyle equationStyle)
        {
            return new EquationStylePdfTextRendererElement(equationStyle);
        }

        if (textElement is ErrorStyle errorStyle)
        {
            return new ErrorStylePdfTextRendererElement(errorStyle);
        }

        if (textElement is FigureStyle figureStyle)
        {
            return new FigureStylePdfTextRendererElement(figureStyle);
        }

        if (textElement is FooterStyle footerStyle)
        {
            return new FooterStylePdfTextRendererElement(footerStyle);
        }

        if (textElement is HeaderStyle headerStyle)
        {
            return new HeaderStylePdfTextRendererElement(headerStyle);
        }

        if (textElement is Heading1Style heading1Style)
        {
            return new Heading1StylePdfTextRendererElement(heading1Style);
        }

        if (textElement is Heading2Style heading2Style)
        {
            return new Heading2StylePdfTextRendererElement(heading2Style);
        }

        if (textElement is Heading3Style heading3Style)
        {
            return new Heading3StylePdfTextRendererElement(heading3Style);
        }

        if (textElement is Heading4Style heading4Style)
        {
            return new Heading4StylePdfTextRendererElement(heading4Style);
        }

        if (textElement is Heading5Style heading5Style)
        {
            return new Heading5StylePdfTextRendererElement(heading5Style);
        }

        if (textElement is ImageStyle imageStyle)
        {
            return new ImageStylePdfTextRendererElement(imageStyle);
        }

        if (textElement is InfoStyle infoStyle)
        {
            return new InfoStylePdfTextRendererElement(infoStyle);
        }

        if (textElement is ListItemStyle listItemStyle)
        {
            return new ListItemStylePdfTextRendererElement(listItemStyle);
        }

        if (textElement is ListStyle listStyle)
        {
            return new ListStylePdfTextRendererElement(listStyle);
        }

        if (textElement is ParagraphCenterStyle paragraphCenterStyle)
        {
            return new ParagraphCenterStylePdfTextRendererElement(paragraphCenterStyle);
        }

        if (textElement is ParagraphJustifyStyle paragraphJustifyStyle)
        {
            return new ParagraphJustifyStylePdfTextRendererElement(paragraphJustifyStyle);
        }

        if (textElement is ParagraphRightStyle paragraphRightStyle)
        {
            return new ParagraphRightStylePdfTextRendererElement(paragraphRightStyle);
        }

        if (textElement is ParagraphStyle paragraphStyle)
        {
            return new ParagraphStylePdfTextRendererElement(paragraphStyle);
        }

        if (textElement is RowStyle rowStyle)
        {
            return new RowStylePdfTextRendererElement(rowStyle);
        }

        if (textElement is SectionSubtitleStyle sectionSubtitleStyle)
        {
            return new SectionSubtitleStylePdfTextRendererElement(sectionSubtitleStyle);
        }

        if (textElement is SectionTitleStyle sectionTitleStyle)
        {
            return new SectionTitleStylePdfTextRendererElement(sectionTitleStyle);
        }

        if (textElement is SubtitleStyle subtitleStyle)
        {
            return new SubtitleStylePdfTextRendererElement(subtitleStyle);
        }

        if (textElement is TableStyle tableStyle)
        {
            return new TableStylePdfTextRendererElement(tableStyle);
        }

        if (textElement is TableHeaderLeftStyle tableHeaderLeftStyle)
        {
            return new TableHeaderLeftStylePdfTextRendererElement(tableHeaderLeftStyle);
        }

        if (textElement is TableHeaderRightStyle tableHeaderRightStyle)
        {
            return new TableHeaderRightStylePdfTextRendererElement(tableHeaderRightStyle);
        }

        if (textElement is TableHeaderCenterStyle tableHeaderCenterStyle)
        {
            return new TableHeaderCenterStylePdfTextRendererElement(tableHeaderCenterStyle);
        }

        if (textElement is TableLegendStyle tableLegendStyle)
        {
            return new TableLegendStylePdfTextRendererElement(tableLegendStyle);
        }

        if (textElement is TitleStyle titleStyle)
        {
            return new TitleStylePdfTextRendererElement(titleStyle);
        }

        if (textElement is Toc1Style toc1Style)
        {
            return new Toc1StylePdfTextRendererElement(toc1Style);
        }

        if (textElement is Toc2Style toc2Style)
        {
            return new Toc2StylePdfTextRendererElement(toc2Style);
        }

        if (textElement is Toc3Style toc3Style)
        {
            return new Toc3StylePdfTextRendererElement(toc3Style);
        }

        if (textElement is Toc4Style toc4Style)
        {
            return new Toc4StylePdfTextRendererElement(toc4Style);
        }

        if (textElement is Toc5Style toc5Style)
        {
            return new Toc5StylePdfTextRendererElement(toc5Style);
        }

        if (textElement is ToeStyle toeStyle)
        {
            return new ToeStylePdfTextRendererElement(toeStyle);
        }

        if (textElement is TofStyle tofStyle)
        {
            return new TofStylePdfTextRendererElement(tofStyle);
        }

        if (textElement is TotStyle totStyle)
        {
            return new TotStylePdfTextRendererElement(totStyle);
        }

        if (textElement is TocHeadingStyle tocHeadingStyle)
        {
            return new TocHeadingStylePdfTextRendererElement(tocHeadingStyle);
        }

        if (textElement is TofHeadingStyle tofHeadingStyle)
        {
            return new TofHeadingStylePdfTextRendererElement(tofHeadingStyle);
        }

        if (textElement is ToeHeadingStyle toeHeadingStyle)
        {
            return new ToeHeadingStylePdfTextRendererElement(toeHeadingStyle);
        }


        if (textElement is TotHeadingStyle totHeadingStyle)
        {
            return new TotHeadingStylePdfTextRendererElement(totHeadingStyle);
        }

        if (textElement is WarningStyle warningStyle)
        {
            return new WarningStylePdfTextRendererElement(warningStyle);
        }

        if (textElement is WatermarkStyle watermarkStyle)
        {
            return new WatermarkStylePdfTextRendererElement(watermarkStyle);
        }

        return null;

    }
}