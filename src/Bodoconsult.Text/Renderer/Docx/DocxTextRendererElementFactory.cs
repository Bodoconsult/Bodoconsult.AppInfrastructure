// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Renderer.Docx.Blocks;
using Bodoconsult.Text.Renderer.Docx.Inlines;
using Bodoconsult.Text.Renderer.Docx.Styles;

namespace Bodoconsult.Text.Renderer.Docx;

/// <summary>
/// <see cref="ITextRendererElementFactory"/> implementation for Docx text rendering
/// </summary>
public class DocxTextRendererElementFactory : IDocxTextRendererElementFactory
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
    /// Create an instance of an <see cref="IDocxTextRendererElement"/> for a given <see cref="TextElement"/>
    /// </summary>
    /// <param name="textElement">Given text element</param>
    /// <returns>Instance of an <see cref="IDocxTextRendererElement"/></returns>
    public IDocxTextRendererElement CreateInstanceDocx(DocumentElement textElement)
    {
        // Base elements
        if (textElement is Document document)
        {
            return new DocumentDocxTextRendererElement(document);
        }

        if (textElement is DocumentMetaData documentMetaData)
        {
            return new DocumentMetaDataDocxTextRendererElement(documentMetaData);
        }

        if (textElement is Styleset styleset)
        {
            return new StylesetDocxTextRendererElement(styleset);
        }

        if (textElement is Section section)
        {
            return new SectionDocxTextRendererElement(section);
        }

        if (textElement is TocSection tocSection)
        {
            return new TocSectionDocxTextRendererElement(tocSection);
        }

        if (textElement is ToeSection toeSection)
        {
            return new ToeSectionDocxTextRendererElement(toeSection);
        }

        if (textElement is TofSection tofSection)
        {
            return new TofSectionDocxTextRendererElement(tofSection);
        }

        if (textElement is TotSection totSection)
        {
            return new TotSectionDocxTextRendererElement(totSection);
        }

        // ParagraphBase based elements
        if (textElement is Citation citation)
        {
            return new CitationDocxTextRendererElement(citation);
        }

        if (textElement is Code code)
        {
            return new CodeDocxTextRendererElement(code);
        }

        if (textElement is Cell cell)
        {
            return new CellDocxTextRendererElement(cell);
        }

        if (textElement is Column column)
        {
            return new ColumnDocxTextRendererElement(column);
        }

        if (textElement is DefinitionList definitionList)
        {
            return new DefinitionListDocxTextRendererElement(definitionList);
        }

        if (textElement is DefinitionListTerm definitionListTerm)
        {
            return new DefinitionListTermDocxTextRendererElement(definitionListTerm);
        }

        if (textElement is DefinitionListItem definitionListItem)
        {
            return new DefinitionListItemDocxTextRendererElement(definitionListItem);
        }

        if (textElement is Equation equation)
        {
            return new EquationDocxTextRendererElement(equation);
        }

        if (textElement is Error error)
        {
            return new ErrorDocxTextRendererElement(error);
        }

        if (textElement is Figure figure)
        {
            return new FigureDocxTextRendererElement(figure);
        }

        if (textElement is Heading1 heading1)
        {
            return new Heading1DocxTextRendererElement(heading1);
        }

        if (textElement is Heading2 heading2)
        {
            return new Heading2DocxTextRendererElement(heading2);
        }

        if (textElement is Heading3 heading3)
        {
            return new Heading3DocxTextRendererElement(heading3);
        }

        if (textElement is Heading4 heading4)
        {
            return new Heading4DocxTextRendererElement(heading4);
        }

        if (textElement is Heading5 heading5)
        {
            return new Heading5DocxTextRendererElement(heading5);
        }

        if (textElement is Image image)
        {
            return new ImageDocxTextRendererElement(image);
        }

        if (textElement is Info info)
        {
            return new InfoDocxTextRendererElement(info);
        }

        if (textElement is List list)
        {
            return new ListDocxTextRendererElement(list);
        }

        if (textElement is ListItem listItem)
        {
            return new ListItemDocxTextRendererElement(listItem);
        }

        if (textElement is Paragraph paragraph)
        {
            return new ParagraphDocxTextRendererElement(paragraph);
        }

        if (textElement is ParagraphCenter paragraphCenter)
        {
            return new ParagraphCenterDocxTextRendererElement(paragraphCenter);
        }

        if (textElement is ParagraphJustify paragraphJustify)
        {
            return new ParagraphJustifyDocxTextRendererElement(paragraphJustify);
        }

        if (textElement is ParagraphRight paragraphRight)
        {
            return new ParagraphRightDocxTextRendererElement(paragraphRight);
        }

        if (textElement is Row row)
        {
            return new RowDocxTextRendererElement(row);
        }

        if (textElement is SectionSubtitle sectionSubtitle)
        {
            return new SectionSubtitleDocxTextRendererElement(sectionSubtitle);
        }

        if (textElement is SectionTitle sectionTitle)
        {
            return new SectionTitleDocxTextRendererElement(sectionTitle);
        }

        if (textElement is Subtitle subtitle)
        {
            return new SubtitleDocxTextRendererElement(subtitle);
        }

        if (textElement is Table table)
        {
            return new TableDocxTextRendererElement(table);
        }

        if (textElement is Title title)
        {
            return new TitleDocxTextRendererElement(title);
        }

        if (textElement is Toc1 toc1)
        {
            return new Toc1DocxTextRendererElement(toc1);
        }

        if (textElement is Toc2 toc2)
        {
            return new Toc2DocxTextRendererElement(toc2);
        }

        if (textElement is Toc3 toc3)
        {
            return new Toc3DocxTextRendererElement(toc3);
        }

        if (textElement is Toc4 toc4)
        {
            return new Toc4DocxTextRendererElement(toc4);
        }

        if (textElement is Toc5 toc5)
        {
            return new Toc5DocxTextRendererElement(toc5);
        }

        if (textElement is Toe toe)
        {
            return new ToeDocxTextRendererElement(toe);
        }

        if (textElement is Tof tof)
        {
            return new TofDocxTextRendererElement(tof);
        }

        if (textElement is Tot tot)
        {
            return new TotDocxTextRendererElement(tot);
        }

        if (textElement is Warning warning)
        {
            return new WarningDocxTextRendererElement(warning);
        }

        // ToDo: add all others

        // Inline based elements
        if (textElement is Span span)
        {
            return new SpanDocxTextRendererElement(span);
        }

        if (textElement is Bold bold)
        {
            return new BoldDocxTextRendererElement(bold);
        }

        if (textElement is Italic italic)
        {
            return new ItalicDocxTextRendererElement(italic);
        }

        if (textElement is LineBreak lineBreak)
        {
            return new LineBreakDocxTextRendererElement(lineBreak);
        }

        if (textElement is Hyperlink hyperlink)
        {
            return new HyperlinkDocxTextRendererElement(hyperlink);
        }

        if (textElement is Value value)
        {
            return new ValueDocxTextRendererElement(value);
        }

        // Base styles
        if (textElement is DocumentStyle documentStyle)
        {
            return new DocumentStyleDocxTextRendererElement(documentStyle);
        }

        if (textElement is SectionStyle sectionStyle)
        {
            return new SectionStyleDocxTextRendererElement(sectionStyle);
        }

        if (textElement is TocSectionStyle tocSectionStyle)
        {
            return new TocSectionStyleDocxTextRendererElement(tocSectionStyle);
        }

        if (textElement is ToeSectionStyle toeSectionStyle)
        {
            return new ToeSectionStyleDocxTextRendererElement(toeSectionStyle);
        }

        if (textElement is TofSectionStyle tofSectionStyle)
        {
            return new TofSectionStyleDocxTextRendererElement(tofSectionStyle);
        }

        if (textElement is TotSectionStyle totSectionStyle)
        {
            return new TotSectionStyleDocxTextRendererElement(totSectionStyle);
        }

        if (textElement is CellLeftStyle cellLeftStyle)
        {
            return new CellLeftStyleDocxTextRendererElement(cellLeftStyle);
        }

        if (textElement is CellRightStyle cellRightStyle)
        {
            return new CellRightStyleDocxTextRendererElement(cellRightStyle);
        }

        if (textElement is CellCenterStyle cellCenterStyle)
        {
            return new CellCenterStyleDocxTextRendererElement(cellCenterStyle);
        }

        if (textElement is ColumnStyle columnStyle)
        {
            return new ColumnStyleDocxTextRendererElement(columnStyle);
        }

        // Paragraph style based
        if (textElement is CitationStyle citationStyle)
        {
            return new CitationStyleDocxTextRendererElement(citationStyle);
        }

        if (textElement is CitationSourceStyle citationSourceStyle)
        {
            return new CitationSourceStyleDocxTextRendererElement(citationSourceStyle);
        }

        if (textElement is CodeStyle codeStyle)
        {
            return new CodeStyleDocxTextRendererElement(codeStyle);
        }

        if (textElement is WatermarkStyle watermarkStyle)
        {
            return new WatermarkStyleDocxTextRendererElement(watermarkStyle);
        }

        if (textElement is DefinitionListStyle definitionListStyle)
        {
            return new DefinitionListStyleDocxTextRendererElement(definitionListStyle);
        }

        if (textElement is DefinitionListTermStyle definitionListTermStyle)
        {
            return new DefinitionListTermStyleDocxTextRendererElement(definitionListTermStyle);
        }

        if (textElement is DefinitionListItemStyle definitionListItemStyle)
        {
            return new DefinitionListItemStyleDocxTextRendererElement(definitionListItemStyle);
        }

        if (textElement is EquationStyle equationStyle)
        {
            return new EquationStyleDocxTextRendererElement(equationStyle);
        }

        if (textElement is ErrorStyle errorStyle)
        {
            return new ErrorStyleDocxTextRendererElement(errorStyle);
        }

        if (textElement is FigureStyle figureStyle)
        {
            return new FigureStyleDocxTextRendererElement(figureStyle);
        }

        if (textElement is FooterStyle footerStyle)
        {
            return new FooterStyleDocxTextRendererElement(footerStyle);
        }

        if (textElement is HeaderStyle headerStyle)
        {
            return new HeaderStyleDocxTextRendererElement(headerStyle);
        }

        if (textElement is Heading1Style heading1Style)
        {
            return new Heading1StyleDocxTextRendererElement(heading1Style);
        }

        if (textElement is Heading2Style heading2Style)
        {
            return new Heading2StyleDocxTextRendererElement(heading2Style);
        }

        if (textElement is Heading3Style heading3Style)
        {
            return new Heading3StyleDocxTextRendererElement(heading3Style);
        }

        if (textElement is Heading4Style heading4Style)
        {
            return new Heading4StyleDocxTextRendererElement(heading4Style);
        }

        if (textElement is Heading5Style heading5Style)
        {
            return new Heading5StyleDocxTextRendererElement(heading5Style);
        }

        if (textElement is ImageStyle imageStyle)
        {
            return new ImageStyleDocxTextRendererElement(imageStyle);
        }

        if (textElement is InfoStyle infoStyle)
        {
            return new InfoStyleDocxTextRendererElement(infoStyle);
        }

        if (textElement is ListItemStyle listItemStyle)
        {
            return new ListItemStyleDocxTextRendererElement(listItemStyle);
        }

        if (textElement is ListStyle listStyle)
        {
            return new ListStyleDocxTextRendererElement(listStyle);
        }

        if (textElement is ParagraphCenterStyle paragraphCenterStyle)
        {
            return new ParagraphCenterStyleDocxTextRendererElement(paragraphCenterStyle);
        }

        if (textElement is ParagraphJustifyStyle paragraphJustifyStyle)
        {
            return new ParagraphJustifyStyleDocxTextRendererElement(paragraphJustifyStyle);
        }

        if (textElement is ParagraphRightStyle paragraphRightStyle)
        {
            return new ParagraphRightStyleDocxTextRendererElement(paragraphRightStyle);
        }

        if (textElement is ParagraphStyle paragraphStyle)
        {
            return new ParagraphStyleDocxTextRendererElement(paragraphStyle);
        }

        if (textElement is RowStyle rowStyle)
        {
            return new RowStyleDocxTextRendererElement(rowStyle);
        }

        if (textElement is SectionSubtitleStyle sectionSubtitleStyle)
        {
            return new SectionSubtitleStyleDocxTextRendererElement(sectionSubtitleStyle);
        }

        if (textElement is SectionTitleStyle sectionTitleStyle)
        {
            return new SectionTitleStyleDocxTextRendererElement(sectionTitleStyle);
        }

        if (textElement is SubtitleStyle subtitleStyle)
        {
            return new SubtitleStyleDocxTextRendererElement(subtitleStyle);
        }

        if (textElement is TableStyle tableStyle)
        {
            return new TableStyleDocxTextRendererElement(tableStyle);
        }

        if (textElement is TableHeaderLeftStyle tableHeaderLeftStyle)
        {
            return new TableHeaderLeftStyleDocxTextRendererElement(tableHeaderLeftStyle);
        }

        if (textElement is TableHeaderRightStyle tableHeaderRightStyle)
        {
            return new TableHeaderRightStyleDocxTextRendererElement(tableHeaderRightStyle);
        }

        if (textElement is TableHeaderCenterStyle tableHeaderCenterStyle)
        {
            return new TableHeaderCenterStyleDocxTextRendererElement(tableHeaderCenterStyle);
        }

        if (textElement is TableLegendStyle tableLegendStyle)
        {
            return new TableLegendStyleDocxTextRendererElement(tableLegendStyle);
        }

        if (textElement is TitleStyle titleStyle)
        {
            return new TitleStyleDocxTextRendererElement(titleStyle);
        }

        if (textElement is Toc1Style toc1Style)
        {
            return new Toc1StyleDocxTextRendererElement(toc1Style);
        }

        if (textElement is Toc2Style toc2Style)
        {
            return new Toc2StyleDocxTextRendererElement(toc2Style);
        }

        if (textElement is Toc3Style toc3Style)
        {
            return new Toc3StyleDocxTextRendererElement(toc3Style);
        }

        if (textElement is Toc4Style toc4Style)
        {
            return new Toc4StyleDocxTextRendererElement(toc4Style);
        }

        if (textElement is Toc5Style toc5Style)
        {
            return new Toc5StyleDocxTextRendererElement(toc5Style);
        }

        if (textElement is ToeStyle toeStyle)
        {
            return new ToeStyleDocxTextRendererElement(toeStyle);
        }

        if (textElement is TofStyle tofStyle)
        {
            return new TofStyleDocxTextRendererElement(tofStyle);
        }

        if (textElement is TotStyle totStyle)
        {
            return new TotStyleDocxTextRendererElement(totStyle);
        }

        if (textElement is TocHeadingStyle tocHeadingStyle)
        {
            return new TocHeadingStyleDocxTextRendererElement(tocHeadingStyle);
        }

        if (textElement is TofHeadingStyle tofHeadingStyle)
        {
            return new TofHeadingStyleDocxTextRendererElement(tofHeadingStyle);
        }

        if (textElement is ToeHeadingStyle toeHeadingStyle)
        {
            return new ToeHeadingStyleDocxTextRendererElement(toeHeadingStyle);
        }


        if (textElement is TotHeadingStyle totHeadingStyle)
        {
            return new TotHeadingStyleDocxTextRendererElement(totHeadingStyle);
        }

        if (textElement is WarningStyle warningStyle)
        {
            return new WarningStyleDocxTextRendererElement(warningStyle);
        }

        return null;

    }
}