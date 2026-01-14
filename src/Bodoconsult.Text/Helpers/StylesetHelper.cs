// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;

namespace Bodoconsult.Text.Helpers;

/// <summary>
/// Helper class to generate Styleset instances
/// </summary>
public static class StylesetHelper
{
    /// <summary>
    ///  Create a default style set
    /// </summary>
    /// <returns></returns>
    public static Styleset CreateDefaultStyleset()
    {
        Styleset.DefaultFontSize = 12;

        var styleSet = new Styleset
        {
            Name = "Default"
        };

        // Add document style
        styleSet.AddBlock(new DocumentStyle());
        styleSet.AddBlock(new SectionStyle());
        styleSet.AddBlock(new TocSectionStyle());
        styleSet.AddBlock(new TofSectionStyle());
        styleSet.AddBlock(new ToeSectionStyle());
        styleSet.AddBlock(new TotSectionStyle());

        // Add style CitationStyle for Citation instances 
        var citationStyle = new CitationStyle();
        styleSet.AddBlock(citationStyle);

        // Add style CellStyleLeft for Cell instances
        var cellStyleLeft = new CellLeftStyle();
        styleSet.AddBlock(cellStyleLeft);

        // Add style CellStyleRight for Cell instances
        var cellStyleRight = new CellRightStyle();
        styleSet.AddBlock(cellStyleRight);

        // Add style CellStyleCenter for Cell instances
        var cellStyleCenter = new CellCenterStyle();
        styleSet.AddBlock(cellStyleCenter);

        // Add style CitationStyle for Citation instances 
        var citationSourceStyle = new CitationSourceStyle();
        styleSet.AddBlock(citationSourceStyle);

        // Add style CodeStyle for Code instances 
        var codeStyle = new CodeStyle();
        styleSet.AddBlock(codeStyle);

        // Add style ColumnStyle for Column instances 
        var columnStyle = new ColumnStyle();
        styleSet.AddBlock(columnStyle);

        // Add style DefinitionListStyle for DefinitionList instances
        var definitionListStyle = new DefinitionListStyle();
        styleSet.AddBlock(definitionListStyle);

        // Add style DefinitionListTermStyle for DefinitionListTerm instances
        var definitionListTermStyle = new DefinitionListTermStyle();
        styleSet.AddBlock(definitionListTermStyle);

        // Add style DefinitionListItemStyle for DefinitionListItem instances
        var definitionListItemStyle = new DefinitionListItemStyle();
        styleSet.AddBlock(definitionListItemStyle);

        // Add style EquationStyle for Equation instances 
        var equationStyle = new EquationStyle();
        styleSet.AddBlock(equationStyle);

        // Add style ErrorStyle for Error instances 
        var errorStyle = new ErrorStyle();
        styleSet.AddBlock(errorStyle);

        // Add style FigureStyle for Figure instances 
        var figureStyle = new FigureStyle();
        styleSet.AddBlock(figureStyle);

        // Add style FooterStyle for footers
        var footerStyle = new FooterStyle();
        styleSet.AddBlock(footerStyle);

        // Add style HeaderStyle for headers
        var headerStyle = new HeaderStyle();
        styleSet.AddBlock(headerStyle);

        // Add style Heading1Style for Heading1 instances 
        var heading1Style = new Heading1Style();
        styleSet.AddBlock(heading1Style);

        // Add style Heading2Style for Heading2 instances 
        var heading2Style = new Heading2Style();
        styleSet.AddBlock(heading2Style);

        // Add style Heading3Style for Heading3 instances 
        var heading3Style = new Heading3Style();
        styleSet.AddBlock(heading3Style);

        // Add style Heading4Style for Heading4 instances 
        var heading4Style = new Heading4Style();
        styleSet.AddBlock(heading4Style);

        // Add style Heading5Style for Heading5 instances 
        var heading5Style = new Heading5Style();
        styleSet.AddBlock(heading5Style);

        // Add style ImageStyle for Image instances 
        var imageStyle = new ImageStyle();
        styleSet.AddBlock(imageStyle);

        // Add style InfoStyle for Info instances 
        var infoStyle = new InfoStyle();
        styleSet.AddBlock(infoStyle);

        // Add style ListStyle for List instances 
        var listStyle = new ListStyle();
        styleSet.AddBlock(listStyle);

        // Add style ListItemStyle for ListItem instances 
        var listitemStyle = new ListItemStyle();
        styleSet.AddBlock(listitemStyle);

        // Add style ParagraphStyle for Paragraph instances 
        var paragraphStyle = new ParagraphStyle();
        styleSet.AddBlock(paragraphStyle);

        // Add style ParagraphCenterStyle for ParagraphCenter instances 
        var paragraphcenterStyle = new ParagraphCenterStyle();
        styleSet.AddBlock(paragraphcenterStyle);

        // Add style ParagraphJustifyStyle for ParagraphJustify instances 
        var paragraphjustifyStyle = new ParagraphJustifyStyle();
        styleSet.AddBlock(paragraphjustifyStyle);

        // Add style ParagraphRightStyle for ParagraphRight instances 
        var paragraphrightStyle = new ParagraphRightStyle();
        styleSet.AddBlock(paragraphrightStyle);

        // Add style RowStyle for Row instances 
        var rowStyle = new RowStyle();
        styleSet.AddBlock(rowStyle);

        // Add style SectionSubtitleStyle for SectionSubtitle instances 
        var sectionsubtitleStyle = new SectionSubtitleStyle();
        styleSet.AddBlock(sectionsubtitleStyle);

        // Add style SectionTitleStyle for SectionTitle instances 
        var sectiontitleStyle = new SectionTitleStyle();
        styleSet.AddBlock(sectiontitleStyle);

        // Add style SubtitleStyle for Subtitle instances 
        var subtitleStyle = new SubtitleStyle();
        styleSet.AddBlock(subtitleStyle);

        // Add style TableStyle for Table instances 
        var tableStyle = new TableStyle();
        styleSet.AddBlock(tableStyle);

        // Add style TableHeaderLeftStyle for Table instances 
        var tableHeaderLeftStyle = new TableHeaderLeftStyle();
        styleSet.AddBlock(tableHeaderLeftStyle);

        // Add style TableHeaderCenterStyle for Table instances 
        var tableHeaderCenterStyle = new TableHeaderCenterStyle();
        styleSet.AddBlock(tableHeaderCenterStyle);

        // Add style TableHeaderRightStyle for Table instances 
        var tableHeaderRightStyle = new TableHeaderRightStyle();
        styleSet.AddBlock(tableHeaderRightStyle);

        // Add style TableLegendStyle for Table instances 
        var tableLegendStyle = new TableLegendStyle();
        styleSet.AddBlock(tableLegendStyle);

        // Add style TitleStyle for Title instances 
        var titleStyle = new TitleStyle();
        styleSet.AddBlock(titleStyle);

        // Add style Toc1Style for Toc1 instances 
        var toc1Style = new Toc1Style();
        styleSet.AddBlock(toc1Style);

        // Add style Toc2Style for Toc2 instances 
        var toc2Style = new Toc2Style();
        styleSet.AddBlock(toc2Style);

        // Add style Toc3Style for Toc3 instances 
        var toc3Style = new Toc3Style();
        styleSet.AddBlock(toc3Style);

        // Add style Toc4Style for Toc4 instances 
        var toc4Style = new Toc4Style();
        styleSet.AddBlock(toc4Style);

        // Add style Toc5Style for Toc5 instances 
        var toc5Style = new Toc5Style();
        styleSet.AddBlock(toc5Style);

        // Add style TocHeadingStyle for TocHeading instances 
        var tocHeadingStyle = new TocHeadingStyle();
        styleSet.AddBlock(tocHeadingStyle);

        // Add style TofHeadingStyle for TofHeading instances 
        var tofHeadingStyle = new TofHeadingStyle();
        styleSet.AddBlock(tofHeadingStyle);

        // Add style TocHeadingStyle for TocHeading instances 
        var toeHeadingStyle = new ToeHeadingStyle();
        styleSet.AddBlock(toeHeadingStyle);

        // Add style ToeStyle for Toe instances 
        var toeStyle = new ToeStyle();
        styleSet.AddBlock(toeStyle);

        // Add style TofStyle for Toe instances 
        var tofStyle = new TofStyle();
        styleSet.AddBlock(tofStyle);

        // Add style TotStyle for Tot instances 
        var totStyle = new TotStyle();
        styleSet.AddBlock(totStyle);

        // Add style TotHeadingStyle for TotHeading instances 
        var totHeadingStyle = new TotHeadingStyle();
        styleSet.AddBlock(totHeadingStyle);

        // Add style WarningStyle for Warning instances 
        var warningStyle = new WarningStyle();
        styleSet.AddBlock(warningStyle);

        // Add style WatermarkStyle for Watermark instances 
        var watermarkStyle = new WatermarkStyle();
        styleSet.AddBlock(watermarkStyle);

        return styleSet;
    }

    /// <summary>
    ///  Create a default style set
    /// </summary>
    /// <returns></returns>
    public static Styleset CreateTestStyleset()
    {
        Styleset.DefaultFontSize = 12;

        var styleSet = new Styleset
        {
            Name = "Default"
        };

        // Add document style
        styleSet.AddBlock(new DocumentStyle());
        styleSet.AddBlock(new SectionStyle());
        styleSet.AddBlock(new TocSectionStyle());
        styleSet.AddBlock(new TofSectionStyle());
        styleSet.AddBlock(new ToeSectionStyle());
        styleSet.AddBlock(new TotSectionStyle());

        // Add style CitationStyle for Citation instances 
        var citationStyle = new CitationStyle();
        styleSet.AddBlock(citationStyle);

        // Add style CitationSourceStyle for Citation source instances 
        var citationSourceStyle = new CitationSourceStyle();
        styleSet.AddBlock(citationSourceStyle);

        // Add style CodeStyle for Code instances 
        var codeStyle = new CodeStyle();
        styleSet.AddBlock(codeStyle);

        // Add style ColumnStyle for Column instances 
        var columnStyle = new ColumnStyle();
        styleSet.AddBlock(columnStyle);

        // Add style CellStyleLeft for Cell instances
        var cellStyleLeft = new CellLeftStyle();
        styleSet.AddBlock(cellStyleLeft);

        // Add style CellStyleRight for Cell instances
        var cellStyleRight = new CellRightStyle();
        styleSet.AddBlock(cellStyleRight);

        // Add style CellStyleCenter for Cell instances
        var cellStyleCenter = new CellCenterStyle();
        styleSet.AddBlock(cellStyleCenter);

        // Add style DefinitionListStyle for DefinitionList instances
        var definitionListStyle = new DefinitionListStyle();
        styleSet.AddBlock(definitionListStyle);

        // Add style DefinitionListTermStyle for DefinitionListTerm instances
        var definitionListTermStyle = new DefinitionListTermStyle();
        styleSet.AddBlock(definitionListTermStyle);

        // Add style DefinitionListItemStyle for DefinitionListItem instances
        var definitionListItemStyle = new DefinitionListItemStyle();
        styleSet.AddBlock(definitionListItemStyle);

        // Add style EquationStyle for Equation instances 
        var equationStyle = new EquationStyle();
        styleSet.AddBlock(equationStyle);

        // Add style ErrorStyle for Error instances 
        var errorStyle = new ErrorStyle();
        errorStyle.Shading = TypoColors.Red.ToLdmlColor();
        styleSet.AddBlock(errorStyle);

        // Add style FigureStyle for Figure instances 
        var figureStyle = new FigureStyle();
        styleSet.AddBlock(figureStyle);

        // Add style FooterStyle for footers
        var footerStyle = new FooterStyle();
        styleSet.AddBlock(footerStyle);

        // Add style HeaderStyle for headers
        var headerStyle = new HeaderStyle();
        styleSet.AddBlock(headerStyle);

        // Add style Heading1Style for Heading1 instances 
        var heading1Style = new Heading1Style
        {
            BorderBrush = new SolidColorBrush(TypoColors.Aqua.ToLdmlColor())
        };
        styleSet.AddBlock(heading1Style);

        // Add style Heading2Style for Heading2 instances 
        var heading2Style = new Heading2Style();
        styleSet.AddBlock(heading2Style);

        // Add style Heading3Style for Heading3 instances 
        var heading3Style = new Heading3Style();
        styleSet.AddBlock(heading3Style);

        // Add style Heading4Style for Heading4 instances 
        var heading4Style = new Heading4Style();
        styleSet.AddBlock(heading4Style);

        // Add style Heading5Style for Heading5 instances 
        var heading5Style = new Heading5Style();
        styleSet.AddBlock(heading5Style);

        // Add style ImageStyle for Image instances 
        var imageStyle = new ImageStyle();
        styleSet.AddBlock(imageStyle);

        // Add style InfoStyle for Info instances 
        var infoStyle = new InfoStyle();
        styleSet.AddBlock(infoStyle);

        // Add style ListStyle for List instances 
        var listStyle = new ListStyle();
        styleSet.AddBlock(listStyle);

        // Add style ListItemStyle for ListItem instances 
        var listitemStyle = new ListItemStyle();
        styleSet.AddBlock(listitemStyle);

        // Add style ParagraphStyle for Paragraph instances 
        var paragraphStyle = new ParagraphStyle();
        styleSet.AddBlock(paragraphStyle);

        // Add style ParagraphCenterStyle for ParagraphCenter instances 
        var paragraphcenterStyle = new ParagraphCenterStyle();
        styleSet.AddBlock(paragraphcenterStyle);

        // Add style ParagraphJustifyStyle for ParagraphJustify instances 
        var paragraphjustifyStyle = new ParagraphJustifyStyle();
        styleSet.AddBlock(paragraphjustifyStyle);

        // Add style ParagraphRightStyle for ParagraphRight instances 
        var paragraphrightStyle = new ParagraphRightStyle();
        styleSet.AddBlock(paragraphrightStyle);

        // Add style RowStyle for Row instances 
        var rowStyle = new RowStyle();
        styleSet.AddBlock(rowStyle);

        // Add style SectionSubtitleStyle for SectionSubtitle instances 
        var sectionsubtitleStyle = new SectionSubtitleStyle();
        styleSet.AddBlock(sectionsubtitleStyle);

        // Add style SectionTitleStyle for SectionTitle instances 
        var sectiontitleStyle = new SectionTitleStyle();
        styleSet.AddBlock(sectiontitleStyle);

        // Add style SubtitleStyle for Subtitle instances 
        var subtitleStyle = new SubtitleStyle();
        styleSet.AddBlock(subtitleStyle);

        // Add style TableStyle for Table instances 
        var tableStyle = new TableStyle();
        styleSet.AddBlock(tableStyle);

        // Add style TableHeaderLeftStyle for Table instances 
        var tableHeaderLeftStyle = new TableHeaderLeftStyle();
        styleSet.AddBlock(tableHeaderLeftStyle);

        // Add style TableHeaderCenterStyle for Table instances 
        var tableHeaderCenterStyle = new TableHeaderCenterStyle();
        styleSet.AddBlock(tableHeaderCenterStyle);

        // Add style TableHeaderRightStyle for Table instances 
        var tableHeaderRightStyle = new TableHeaderRightStyle();
        styleSet.AddBlock(tableHeaderRightStyle);

        // Add style TableLegendStyle for Table instances 
        var tableLegendStyle = new TableLegendStyle();
        styleSet.AddBlock(tableLegendStyle);

        // Add style TitleStyle for Title instances 
        var titleStyle = new TitleStyle();
        styleSet.AddBlock(titleStyle);

        // Add style Toc1Style for Toc1 instances 
        var toc1Style = new Toc1Style();
        styleSet.AddBlock(toc1Style);

        // Add style Toc2Style for Toc2 instances 
        var toc2Style = new Toc2Style();
        styleSet.AddBlock(toc2Style);

        // Add style Toc3Style for Toc3 instances 
        var toc3Style = new Toc3Style();
        styleSet.AddBlock(toc3Style);

        // Add style Toc4Style for Toc4 instances 
        var toc4Style = new Toc4Style();
        styleSet.AddBlock(toc4Style);

        // Add style Toc5Style for Toc5 instances 
        var toc5Style = new Toc5Style();
        styleSet.AddBlock(toc5Style);

        // Add style ToeStyle for Toe instances 
        var toeStyle = new ToeStyle();
        styleSet.AddBlock(toeStyle);

        // Add style TofStyle for Toe instances 
        var tofStyle = new TofStyle();
        styleSet.AddBlock(tofStyle);

        // Add style TotStyle for Toe instances 
        var totStyle = new TotStyle();
        styleSet.AddBlock(totStyle);

        // Add style TocHeadingStyle for TocHeading instances 
        var tocHeadingStyle = new TocHeadingStyle();
        styleSet.AddBlock(tocHeadingStyle);

        // Add style TofHeadingStyle for TofHeading instances 
        var tofHeadingStyle = new TofHeadingStyle();
        styleSet.AddBlock(tofHeadingStyle);

        // Add style ToeHeadingStyle for ToeHeading instances 
        var toeHeadingStyle = new ToeHeadingStyle();
        styleSet.AddBlock(toeHeadingStyle);

        // Add style TotHeadingStyle for TotHeading instances 
        var totHeadingStyle = new TotHeadingStyle();
        styleSet.AddBlock(totHeadingStyle);

        // Add style WarningStyle for Warning instances 
        var warningStyle = new WarningStyle();
        styleSet.AddBlock(warningStyle);

        // Add style WatermarkStyle for Watermark instances 
        var watermarkStyle = new WatermarkStyle();
        styleSet.AddBlock(watermarkStyle);

        return styleSet;
    }

    /// <summary>
    ///  Create a default style set
    /// </summary>
    /// <returns></returns>
    public static Styleset CreateTestStylesetLandscape3Columns()
    {
        Styleset.DefaultFontSize = 10;

        var styleSet = new Styleset
        {
            Name = "Default"
        };

        // Add document style
        var docStyle = new DocumentStyle
        {
            PaperFormat =
            {
                Size = new Size(29.7, 21)
            },
            Margins =
            {
                Left = 2,
                Top = 3, 
                Right = 2, 
                Bottom = 2
            },
            NumberOfColumns = 3,
            ColumnGap = 1
        };
        styleSet.AddBlock(docStyle);

        styleSet.AddBlock(new SectionStyle());
        styleSet.AddBlock(new TocSectionStyle());
        styleSet.AddBlock(new TofSectionStyle());
        styleSet.AddBlock(new ToeSectionStyle());
        styleSet.AddBlock(new TotSectionStyle());

        // Add style CitationStyle for Citation instances 
        var citationStyle = new CitationStyle();
        styleSet.AddBlock(citationStyle);

        // Add style CitationSourceStyle for Citation source instances 
        var citationSourceStyle = new CitationSourceStyle();
        styleSet.AddBlock(citationSourceStyle);

        // Add style CodeStyle for Code instances 
        var codeStyle = new CodeStyle();
        styleSet.AddBlock(codeStyle);

        // Add style ColumnStyle for Column instances 
        var columnStyle = new ColumnStyle();
        styleSet.AddBlock(columnStyle);

        // Add style CellStyleLeft for Cell instances
        var cellStyleLeft = new CellLeftStyle();
        styleSet.AddBlock(cellStyleLeft);

        // Add style CellStyleRight for Cell instances
        var cellStyleRight = new CellRightStyle();
        styleSet.AddBlock(cellStyleRight);

        // Add style CellStyleCenter for Cell instances
        var cellStyleCenter = new CellCenterStyle();
        styleSet.AddBlock(cellStyleCenter);

        // Add style DefinitionListStyle for DefinitionList instances
        var definitionListStyle = new DefinitionListStyle();
        styleSet.AddBlock(definitionListStyle);

        // Add style DefinitionListTermStyle for DefinitionListTerm instances
        var definitionListTermStyle = new DefinitionListTermStyle();
        styleSet.AddBlock(definitionListTermStyle);

        // Add style DefinitionListItemStyle for DefinitionListItem instances
        var definitionListItemStyle = new DefinitionListItemStyle();
        styleSet.AddBlock(definitionListItemStyle);

        // Add style EquationStyle for Equation instances 
        var equationStyle = new EquationStyle();
        styleSet.AddBlock(equationStyle);

        // Add style ErrorStyle for Error instances 
        var errorStyle = new ErrorStyle();
        styleSet.AddBlock(errorStyle);

        // Add style FigureStyle for Figure instances 
        var figureStyle = new FigureStyle();
        styleSet.AddBlock(figureStyle);

        // Add style FooterStyle for footers
        var footerStyle = new FooterStyle();
        styleSet.AddBlock(footerStyle);

        // Add style HeaderStyle for headers
        var headerStyle = new HeaderStyle();
        styleSet.AddBlock(headerStyle);

        // Add style Heading1Style for Heading1 instances 
        var heading1Style = new Heading1Style
        {
            BorderBrush = new SolidColorBrush(TypoColors.Aqua.ToLdmlColor())
        };
        styleSet.AddBlock(heading1Style);

        // Add style Heading2Style for Heading2 instances 
        var heading2Style = new Heading2Style();
        styleSet.AddBlock(heading2Style);

        // Add style Heading3Style for Heading3 instances 
        var heading3Style = new Heading3Style();
        styleSet.AddBlock(heading3Style);

        // Add style Heading4Style for Heading4 instances 
        var heading4Style = new Heading4Style();
        styleSet.AddBlock(heading4Style);

        // Add style Heading5Style for Heading5 instances 
        var heading5Style = new Heading5Style();
        styleSet.AddBlock(heading5Style);

        // Add style ImageStyle for Image instances 
        var imageStyle = new ImageStyle();
        styleSet.AddBlock(imageStyle);

        // Add style InfoStyle for Info instances 
        var infoStyle = new InfoStyle();
        styleSet.AddBlock(infoStyle);

        // Add style ListStyle for List instances 
        var listStyle = new ListStyle();
        styleSet.AddBlock(listStyle);

        // Add style ListItemStyle for ListItem instances 
        var listitemStyle = new ListItemStyle();
        styleSet.AddBlock(listitemStyle);

        // Add style ParagraphStyle for Paragraph instances 
        var paragraphStyle = new ParagraphStyle();
        styleSet.AddBlock(paragraphStyle);

        // Add style ParagraphCenterStyle for ParagraphCenter instances 
        var paragraphcenterStyle = new ParagraphCenterStyle();
        styleSet.AddBlock(paragraphcenterStyle);

        // Add style ParagraphJustifyStyle for ParagraphJustify instances 
        var paragraphjustifyStyle = new ParagraphJustifyStyle();
        styleSet.AddBlock(paragraphjustifyStyle);

        // Add style ParagraphRightStyle for ParagraphRight instances 
        var paragraphrightStyle = new ParagraphRightStyle();
        styleSet.AddBlock(paragraphrightStyle);

        // Add style RowStyle for Row instances 
        var rowStyle = new RowStyle();
        styleSet.AddBlock(rowStyle);

        // Add style SectionSubtitleStyle for SectionSubtitle instances 
        var sectionsubtitleStyle = new SectionSubtitleStyle();
        styleSet.AddBlock(sectionsubtitleStyle);

        // Add style SectionTitleStyle for SectionTitle instances 
        var sectiontitleStyle = new SectionTitleStyle();
        styleSet.AddBlock(sectiontitleStyle);

        // Add style SubtitleStyle for Subtitle instances 
        var subtitleStyle = new SubtitleStyle();
        styleSet.AddBlock(subtitleStyle);

        // Add style TableStyle for Table instances 
        var tableStyle = new TableStyle();
        styleSet.AddBlock(tableStyle);

        // Add style TableHeaderLeftStyle for Table instances 
        var tableHeaderLeftStyle = new TableHeaderLeftStyle();
        styleSet.AddBlock(tableHeaderLeftStyle);

        // Add style TableHeaderCenterStyle for Table instances 
        var tableHeaderCenterStyle = new TableHeaderCenterStyle();
        styleSet.AddBlock(tableHeaderCenterStyle);

        // Add style TableHeaderRightStyle for Table instances 
        var tableHeaderRightStyle = new TableHeaderRightStyle();
        styleSet.AddBlock(tableHeaderRightStyle);

        // Add style TableLegendStyle for Table instances 
        var tableLegendStyle = new TableLegendStyle();
        styleSet.AddBlock(tableLegendStyle);

        // Add style TitleStyle for Title instances 
        var titleStyle = new TitleStyle();
        styleSet.AddBlock(titleStyle);

        // Add style Toc1Style for Toc1 instances 
        var toc1Style = new Toc1Style();
        styleSet.AddBlock(toc1Style);

        // Add style Toc2Style for Toc2 instances 
        var toc2Style = new Toc2Style();
        styleSet.AddBlock(toc2Style);

        // Add style Toc3Style for Toc3 instances 
        var toc3Style = new Toc3Style();
        styleSet.AddBlock(toc3Style);

        // Add style Toc4Style for Toc4 instances 
        var toc4Style = new Toc4Style();
        styleSet.AddBlock(toc4Style);

        // Add style Toc5Style for Toc5 instances 
        var toc5Style = new Toc5Style();
        styleSet.AddBlock(toc5Style);

        // Add style ToeStyle for Toe instances 
        var toeStyle = new ToeStyle();
        styleSet.AddBlock(toeStyle);

        // Add style TofStyle for Toe instances 
        var tofStyle = new TofStyle();
        styleSet.AddBlock(tofStyle);

        // Add style TotStyle for Toe instances 
        var totStyle = new TotStyle();
        styleSet.AddBlock(totStyle);

        // Add style TocHeadingStyle for TocHeading instances 
        var tocHeadingStyle = new TocHeadingStyle();
        styleSet.AddBlock(tocHeadingStyle);

        // Add style TofHeadingStyle for TofHeading instances 
        var tofHeadingStyle = new TofHeadingStyle();
        styleSet.AddBlock(tofHeadingStyle);

        // Add style ToeHeadingStyle for ToeHeading instances 
        var toeHeadingStyle = new ToeHeadingStyle();
        styleSet.AddBlock(toeHeadingStyle);

        // Add style TotHeadingStyle for TotHeading instances 
        var totHeadingStyle = new TotHeadingStyle();
        styleSet.AddBlock(totHeadingStyle);

        // Add style WarningStyle for Warning instances 
        var warningStyle = new WarningStyle();
        styleSet.AddBlock(warningStyle);

        // Add style WatermarkStyle for Watermark instances 
        var watermarkStyle = new WatermarkStyle();
        styleSet.AddBlock(watermarkStyle);

        return styleSet;
    }

    /// <summary>
    /// Get the maximum width and height of images from document
    /// </summary>
    /// <param name="styleset">Current styleset</param>
    /// <param name="maxWidth">Maximum width in cm</param>
    /// <param name="maxHeight">Maximum height in cm</param>
    public static void GetMaxWidthAndHeightInCm(Styleset styleset, out double maxWidth, out double maxHeight)
    {
        var style = (DocumentStyle)styleset.FindStyle(nameof(DocumentStyle));

        maxWidth = style.MaxImageWidth;
        maxHeight = style.MaxImageHeight;
    }

    /// <summary>
    /// Get the maximum width and height of images from document
    /// </summary>
    /// <param name="styleset">Current styleset</param>
    /// <param name="maxWidth">Maximum width in twips</param>
    /// <param name="maxHeight">Maximum height in twips</param>
    public static void GetMaxWidthAndHeightInTwips(Styleset styleset, out int maxWidth, out int maxHeight)
    {
        var style = (DocumentStyle)styleset.FindStyle(nameof(DocumentStyle));

        maxWidth = MeasurementHelper.GetTwipsFromCm(style.MaxImageWidth);
        maxHeight = MeasurementHelper.GetTwipsFromCm(style.MaxImageHeight);
    }

    /// <summary>
    /// Get the current width and height of an image
    /// </summary>
    /// <param name="originalWidth">Original width in twips</param>
    /// <param name="originalHeight">Original width in twips</param>
    /// <param name="maxWidth">Maximum width in twips</param>
    /// <param name="maxHeight">Maximum height in twips</param>
    /// <param name="width">Current width in twips</param>
    /// <param name="height">Current height in twips</param>
    public static void GetWidthAndHeightInTwips(int originalWidth, int originalHeight, int maxWidth,
        int maxHeight, out int width, out int height)
    {
        if (originalHeight > originalWidth) // Portrait
        {
            width = originalWidth > maxWidth ? maxWidth : originalWidth;
            height = (int)(originalHeight / (double)originalWidth * width);

            if (originalHeight <= maxHeight)
            {
                return;
            }
            height = maxHeight;
            width = (int)(originalWidth / (double)originalHeight * maxHeight);
        }
        else // Landscape
        {
            height = originalHeight > maxHeight ? maxHeight : originalHeight;
            width = (int)(originalWidth / (double)originalHeight * height);

            if (originalWidth <= maxWidth)
            {
                return;
            }
            width = maxWidth;
            height = (int)(originalHeight / (double)originalWidth * maxWidth);
        }
    }

    /// <summary>
    /// Get the current width and height of an image
    /// </summary>
    /// <param name="originalWidth">Original width in cm</param>
    /// <param name="originalHeight">Original width in cm</param>
    /// <param name="maxWidth">Maximum width in cm</param>
    /// <param name="maxHeight">Maximum height in cm</param>
    /// <param name="width">Current width in cm</param>
    /// <param name="height">Current height in cm</param>
    public static void GetWidthAndHeightInCm(double originalWidth, double originalHeight, double maxWidth,
        double maxHeight, out double width, out double height)
    {
        if (originalHeight > originalWidth) // Portrait
        {
            width = originalWidth > maxWidth ? maxWidth : originalWidth;
            height = (int)(originalHeight / originalWidth * width);

            if (originalHeight <= maxHeight)
            {
                return;
            }
            height = maxHeight;
            width = (int)(originalWidth / originalHeight * maxHeight);
        }
        else // Landscape
        {
            height = originalHeight > maxHeight ? maxHeight : originalHeight;
            width = (int)(originalWidth / originalHeight * height);

            if (originalWidth <= maxWidth)
            {
                return;
            }
            width = maxWidth;
            height = (int)(originalHeight / originalWidth * maxWidth);
        }
    }
}