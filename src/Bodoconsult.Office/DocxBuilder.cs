// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Extensions;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Diagnostics;
using System.Globalization;
using System.Transactions;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using ParagraphProperties = DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Properties = DocumentFormat.OpenXml.ExtendedProperties.Properties;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Style = DocumentFormat.OpenXml.Wordprocessing.Style;
using Tabs = DocumentFormat.OpenXml.Wordprocessing.Tabs;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;


namespace Bodoconsult.Office;

/// <summary>
/// Create OpenXML DOCX files
/// </summary>
public class DocxBuilder : IDisposable
{
    private int _imageCounter = -1;

    /// <summary>
    /// DOCX document
    /// </summary>
    public WordprocessingDocument Docx { get; private set; }

    /// <summary>
    /// Main part of the document
    /// </summary>
    public MainDocumentPart MainDocumentPart { get; private set; }

    /// <summary>
    /// Current document settings part
    /// </summary>
    public DocumentSettingsPart Settings { get; private set; }

    /// <summary>
    /// Style definition part
    /// </summary>
    public StyleDefinitionsPart StyleDefinitionsPart { get; private set; }

    /// <summary>
    /// Current numbering definition part
    /// </summary>
    public NumberingDefinitionsPart NumberingDefinitionsPart { get; private set; }

    /// <summary>
    /// Current styles in the document
    /// </summary>
    public Styles Styles { get; private set; }

    /// <summary>
    /// Body of the document
    /// </summary>
    public Body Body { get; private set; }

    /// <summary>
    /// Memory stream representing the document. Is only set if <see cref="CreateDocument()"/> was used to create the document
    /// </summary>
    public MemoryStream MemoryStream { get; private set; }

    /// <summary>
    /// All sections in the document
    /// </summary>
    public List<SectionProperties> Sections { get; } = new();

    /// <summary>
    /// Current section in the document
    /// </summary>
    public SectionProperties CurrentSection { get; private set; }

    /// <summary>
    /// Create document in memory
    /// </summary>
    public void CreateDocument()
    {
        MemoryStream = new MemoryStream();
        Docx = WordprocessingDocument.Create(MemoryStream, WordprocessingDocumentType.Document, true);
        LoadBaseData();
    }

    /// <summary>
    /// Create document as file
    /// </summary>
    /// <param name="filePath">Full file path to save the document in</param>
    public void CreateDocument(string filePath)
    {
        Docx = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document, true);
        LoadBaseData();
    }

    /// <summary>
    /// Save document as file. Works only if the document was created with Create() method withour filepath
    /// </summary>
    /// <param name="filePath">Full file path to save the document in</param>
    public void SaveDocument(string filePath)
    {
        if (MemoryStream == null)
        {
            Docx.Save();
            return;
        }

        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        Docx.Save();

        MemoryStream.Position = 0;

        using var fis = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
        MemoryStream.CopyTo(fis);
    }


    private void LoadBaseData()
    {
        // Assign a reference to the existing document body.
        MainDocumentPart = Docx.MainDocumentPart ?? Docx.AddMainDocumentPart();
        MainDocumentPart.Document ??= new Document();
        MainDocumentPart.Document.Body ??= MainDocumentPart.Document.AppendChild(new Body());
        Body = Docx.MainDocumentPart!.Document!.Body!;

        // Set to latest OpenXML version
        Settings = MainDocumentPart.AddNewPart<DocumentSettingsPart>();
        Settings.Settings = new Settings();
        var objCompatibility = new Compatibility();
        var objCompatibilitySetting = new CompatibilitySetting
        {
            Name = CompatSettingNameValues.CompatibilityMode,
            Uri = "http://schemas.microsoft.com/office/word",
            Val = "15"
        };
        objCompatibility.Append(objCompatibilitySetting);
        Settings.Settings.Append(objCompatibility);

        //// Create object to update fields on open
        //var updateFields = new UpdateFieldsOnOpen
        //{
        //    Val = new OnOffValue(true)
        //};
        //Settings.Settings.Append(updateFields);

        // Add style part
        StyleDefinitionsPart = Docx.MainDocumentPart.StyleDefinitionsPart ?? AddStylesPartToPackage();
        Styles = StyleDefinitionsPart?.Styles;

        NumberingDefinitionsPart = MainDocumentPart.NumberingDefinitionsPart ?? MainDocumentPart.AddNewPart<NumberingDefinitionsPart>("NumberingDefinitionsPart001");

        // ReSharper disable once ConstantNullCoalescingCondition
        NumberingDefinitionsPart.Numbering ??= new Numbering();

        AddBulletedNumbering(NumberFormatValues.Bullet, "•");
        AddBulletedNumbering(NumberFormatValues.Bullet, "-");

        AddBulletedNumbering(NumberFormatValues.Decimal, null);
        AddBulletedNumbering(NumberFormatValues.LowerRoman, null);
        AddBulletedNumbering(NumberFormatValues.UpperRoman, null);
        AddBulletedNumbering(NumberFormatValues.LowerLetter, null);
        AddBulletedNumbering(NumberFormatValues.UpperLetter, null);

        //Numbering element =
        //    new Numbering(
        //        new AbstractNum(
        //                new Level(
        //                        new NumberingFormat() { Val = NumberFormatValues.Bullet },
        //                        new LevelText() { Val = "•" }
        //                    )
        //                    { LevelIndex = 0 }
        //            )
        //            { AbstractNumberId = 1 },
        //        new NumberingInstance(
        //                new AbstractNumId() { Val = 1 }
        //            )
        //            { NumberID = 1 });
        //element.Save(numberingPart);

        //element =
        //    new Numbering(
        //        new AbstractNum(
        //                new Level(
        //                        new NumberingFormat() { Val = NumberFormatValues.Bullet },
        //                        new LevelText() { Val = "-" }
        //                    )
        //                { LevelIndex = 0 }
        //            )
        //        { AbstractNumberId = 2 },
        //        new NumberingInstance(
        //                new AbstractNumId() { Val = 2 }
        //            )
        //        { NumberID = 2 });
        //element.Save(numberingPart);
    }

    private void AddBulletedNumbering(NumberFormatValues numberFormat, string bullet)
    {
        // https://stackoverflow.com/questions/1940911/openxml-2-sdk-word-document-create-bulleted-list-programmatically

        // https://stackoverflow.com/questions/59093861/how-do-you-create-multi-level-ordered-lists-with-open-xml-in-asp-net

        // Insert an AbstractNum into the numbering part numbering list. The order seems to matter or it will not pass the 
        // Open XML SDK Productity Tools validation test.  AbstractNum comes first and then NumberingInstance and we want to
        // insert this AFTER the last AbstractNum and BEFORE the first NumberingInstance or we will get a validation error.
        var abstractNumberId = NumberingDefinitionsPart.Numbering.Elements<AbstractNum>().Count() + 1;

        Level abstractLevel;

        if (string.IsNullOrEmpty(bullet))
        {
            abstractLevel = new Level(new NumberingFormat { Val = numberFormat }, new LevelText { Val = "%1." })
            {
                LevelIndex = 0,
                StartNumberingValue = new StartNumberingValue { Val = 1 },
            };
        }
        else
        {
            abstractLevel = new Level(new NumberingFormat { Val = numberFormat }, new LevelText { Val = bullet })
            {
                LevelIndex = 0
            };
        }

        var abstractNum1 = new AbstractNum(abstractLevel) { AbstractNumberId = abstractNumberId };

        if (abstractNumberId == 1)
        {
            NumberingDefinitionsPart.Numbering.Append(abstractNum1);
        }
        else
        {
            var lastAbstractNum = NumberingDefinitionsPart.Numbering.Elements<AbstractNum>().Last();
            NumberingDefinitionsPart.Numbering.InsertAfter(abstractNum1, lastAbstractNum);
        }

        // Insert an NumberingInstance into the numbering part numbering list.  The order seems to matter or it will not pass the 
        // Open XML SDK Productity Tools validation test.  AbstractNum comes first and then NumberingInstance and we want to
        // insert this AFTER the last NumberingInstance and AFTER all the AbstractNum entries or we will get a validation error.
        var numberId = NumberingDefinitionsPart.Numbering.Elements<NumberingInstance>().Count() + 1;
        var numberingInstance1 = new NumberingInstance { NumberID = numberId };
        var abstractNumId1 = new AbstractNumId { Val = abstractNumberId };
        numberingInstance1.Append(abstractNumId1);

        if (numberId == 1)
        {
            NumberingDefinitionsPart.Numbering.Append(numberingInstance1);
        }
        else
        {
            var lastNumberingInstance = NumberingDefinitionsPart.Numbering.Elements<NumberingInstance>().Last();
            NumberingDefinitionsPart.Numbering.InsertAfter(numberingInstance1, lastNumberingInstance);
        }
    }


    // Add a StylesDefinitionsPart to the document.  Returns a reference to it.
    private StyleDefinitionsPart AddStylesPartToPackage()
    {
        if (MainDocumentPart is null)
        {
            throw new ArgumentNullException(nameof(DocumentFormat.OpenXml.Packaging.MainDocumentPart));
        }

        var part = MainDocumentPart.AddNewPart<StyleDefinitionsPart>();

        part.Styles = new Styles();

        Styles = part.Styles;

        return part;
    }

    ///// <summary>
    ///// Set basic page properties like width and height and margins
    ///// </summary>
    ///// <param name="pageWidth">Page width in cm</param>
    ///// <param name="pageHeight">Page height in cm</param>
    ///// <param name="marginLeft">Margin left in cm</param>
    ///// <param name="marginTop">Margin top in cm</param>
    ///// <param name="marginRight">Margin right in cm</param>
    ///// <param name="marginBottom">Margin bottom in cm</param>
    //public void SetBasicPageProperties(double pageWidth, double pageHeight, double marginLeft, double marginTop, double marginRight, double marginBottom)
    //{
    //    // Paper size
    //    var width = MeasurementHelper.GetDxaFromCm(pageWidth);
    //    var height = MeasurementHelper.GetDxaFromCm(pageHeight);

    //    // Margins
    //    var left = MeasurementHelper.GetDxaFromCm(marginLeft);
    //    var top = MeasurementHelper.GetDxaFromCm(marginTop);
    //    var right = MeasurementHelper.GetDxaFromCm(marginRight);
    //    var bottom = MeasurementHelper.GetDxaFromCm(marginBottom);

    //    var pgSz = CurrentSection.ChildElements.OfType<PageSize>().FirstOrDefault() ?? CurrentSection.AppendChild(new PageSize { Width = width, Height = height });

    //    pgSz.Orient = pageWidth > pageHeight ? new EnumValue<PageOrientationValues>(PageOrientationValues.Landscape) : new EnumValue<PageOrientationValues>(PageOrientationValues.Portrait);

    //    var pageMargin = new PageMargin { Top = (int)top, Right = right, Bottom = (int)bottom, Left = left };
    //    CurrentSection.Append(pageMargin);
    //}

    /// <summary>
    /// Add common metadata like author, company and title
    /// </summary>
    /// <param name="author">Author of the document</param>
    /// <param name="company">Company</param>
    /// <param name="title">Document title</param>

    public void AddMetadata(string author, string company, string title)
    {

        var bProps = Docx.PackageProperties;

        bProps.Title = title;

        bProps.Creator = author;


        // ToDo: make ext props working and add title

        var epPart = Docx.ExtendedFilePropertiesPart ?? Docx.AddExtendedFilePropertiesPart();

        // ReSharper disable once ConstantNullCoalescingCondition
        epPart.Properties ??= new Properties();

        var props = epPart.Properties;

        if (!string.IsNullOrEmpty(company))
        {
            props.Company = new Company(company);
        }
    }

    /// <summary>
    /// Add a header to the current section
    /// </summary>
    /// <param name="headerText">Header text. May contain &lt;&lt;Page&gt;&gt; being replaced by page number field</param>
    /// <param name="position">Position of the page number (if &lt;&lt;Page&gt;&gt; is used) in cm relative to typearea</param>
    /// <param name="pageNumberFormat">Page number format</param>
    public void AddHeaderToCurrentSection(string headerText, double position, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {

        var headerPart = MainDocumentPart.AddNewPart<HeaderPart>();

        var headerPartId = MainDocumentPart.GetIdOfPart(headerPart);

        const string styleId = "Header";

        var posTwips = MeasurementHelper.GetTwipsFromCm(position);

        var para = CreateHeaderFooterParagraph(headerText, styleId, posTwips, pageNumberFormat);

        headerPart.Header = new Header(para);

        CurrentSection.PrependChild(new HeaderReference
        {
            Id = headerPartId
        });
    }

    private static Paragraph CreateHeaderFooterParagraph(string text, string styleId, int position, PageNumberFormatEnum pageNumberFormat)
    {
        var pPr = new ParagraphProperties(new ParagraphStyleId { Val = styleId });

        var para = new Paragraph(pPr);

        var runs = GetRuns(text, position, pPr, pageNumberFormat);

        foreach (var run in runs)
        {
            para.Append(run);
        }

        return para;
    }

    /// <summary>
    /// Add a footer to the current section
    /// </summary>
    /// <param name="footerText">Footer text. May contain &lt;&lt;Page&gt;&gt; being replaced by page number field</param>
    /// <param name="position">Position of the page number (if &lt;&lt;Page&gt;&gt; is used) in cm relative to typearea</param>
    /// <param name="pageNumberFormat">Page number format</param>
    public void AddFooterToCurrentSection(string footerText, double position, PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {

        var footerPart = MainDocumentPart.AddNewPart<FooterPart>();

        var footerPartId = MainDocumentPart.GetIdOfPart(footerPart);

        const string styleId = "Footer";

        var posTwips = MeasurementHelper.GetTwipsFromCm(position);

        var para = CreateHeaderFooterParagraph(footerText, styleId, posTwips, pageNumberFormat);

        footerPart.Footer = new Footer(para);

        CurrentSection.PrependChild(new FooterReference
        {
            Id = footerPartId
        });
    }

    private static List<Run> GetRuns(string text, int position, ParagraphProperties pPr, PageNumberFormatEnum pageNumberFormat)
    {
        var result = new List<Run>();

        var parts = new List<string>();

        var pNf = string.Empty;

        var i = text.IndexOf(ITypography.PageFieldIndicator, StringComparison.InvariantCultureIgnoreCase);

        if (i < 0)
        {
            parts.Add(text);
        }
        else
        {
            // Add a tab for the page number
            pPr.Tabs = new Tabs();
            var tabStop = new TabStop
            {
                Val = TabStopValues.Right,
                Position = position
            };
            pPr.Tabs.Append(tabStop);

            // Split the text in runs
            var before = text[..i];
            var after = text[(i + ITypography.PageFieldIndicator.Length)..];

            parts.Add(before);
            parts.Add(ITypography.PageFieldIndicator);
            parts.Add(after);

            switch (pageNumberFormat)
            {
                case PageNumberFormatEnum.UpperRoman:
                    pNf = "ROMAN";
                    break;
                case PageNumberFormatEnum.LowerRoman:
                    pNf = "roman";
                    break;
                case PageNumberFormatEnum.UpperLatin:
                    pNf = "ALPHABETIC";
                    break;
                case PageNumberFormatEnum.LowerLatin:
                    pNf = "alphabetic";
                    break;
                case PageNumberFormatEnum.Decimal:
                default:
                    pNf = "Arabic";
                    break;
            }
        }

        foreach (var part in parts)
        {
            if (part == ITypography.PageFieldIndicator)
            {
                result.Add(new Run(new FieldChar { FieldCharType = FieldCharValues.Begin }));
                result.Add(new Run(new FieldCode { Space = SpaceProcessingModeValues.Preserve, Text = $" PAGE  \\* {pNf}  \\* MERGEFORMAT " }));
                result.Add(new Run(new FieldChar { FieldCharType = FieldCharValues.Separate }));
                result.Add(new Run(new RunProperties(new NoProof()), new Text("1")));
                result.Add(new Run(new FieldChar { FieldCharType = FieldCharValues.End }));
            }
            else
            {
                var run = new Run();
                run.Append(new Text(part) { Space = SpaceProcessingModeValues.Preserve });
                result.Add(run);
            }
        }
        return result;
    }


    /// <summary>
    /// Create a new style with the specified styleid and stylename
    /// </summary>
    /// <param name="styleid">Style ID</param>
    /// <param name="stylename">Style name</param>
    /// <param name="styleRunProperties">Run properties for styling</param>
    /// <param name="uiPriority">UI priority</param>
    /// <returns>OpenXML Style</returns>
    public Style AddNewStyle(string styleid, string stylename, StyleRunProperties styleRunProperties, int uiPriority)
    {
        // Create a new paragraph style and specify some of the properties.
        var style = new Style
        {
            Type = StyleValues.Paragraph,
            StyleId = styleid,
            CustomStyle = true
        };
        style.Append(new StyleName { Val = stylename });
        style.Append(new BasedOn { Val = "Normal" });
        style.Append(new NextParagraphStyle { Val = "Normal" });
        style.Append(new UIPriority { Val = uiPriority });
        style.Append(styleRunProperties);

        Styles.Append(style);
        return style;
    }

    /// <summary>
    /// Create a new style with the specified styleid and stylename
    /// </summary>
    /// <param name="styleid">Style ID</param>
    /// <param name="stylename">Style name</param>
    /// <param name="typoStyle">Style to create</param>
    /// <param name="uiPriority">UI priority</param>
    /// <returns>OpenXML Style</returns>
    public Style AddNewStyle(string styleid, string stylename, ITypoParagraphStyle typoStyle, int uiPriority)
    {
        Debug.Print($"{styleid}: Alignment {typoStyle.TextAlignment}");
        Debug.Print($"{styleid}: Bold {typoStyle.Bold}");
        Debug.Print($"{styleid}: L{typoStyle.TypoMargins.Left} T{typoStyle.TypoMargins.Top} R{typoStyle.TypoMargins.Right} B{typoStyle.TypoMargins.Bottom}");
        Debug.Print($"{styleid}: L{typoStyle.TypoPaddings.Left} T{typoStyle.TypoPaddings.Top} R{typoStyle.TypoPaddings.Right} B{typoStyle.TypoPaddings.Bottom}");
        Debug.Print($"{styleid}: L{typoStyle.TypoBorderThickness.Left} T{typoStyle.TypoBorderThickness.Top} R{typoStyle.TypoBorderThickness.Right} B{typoStyle.TypoBorderThickness.Bottom}");

        StyleRunProperties styleRunProperties = new();

        // Create a new paragraph style and specify some of the properties.
        var style = CreateStyle(Styles, styleid, stylename, uiPriority, styleRunProperties);

        var pPr = new ParagraphProperties();
        style.Append(pPr);

        // Margins and indentation
        CreateMargins(typoStyle, pPr);

        // Create paragraph settings like KeepLines, KeepNext etc.
        CreateAdvancedParagraphSettings(typoStyle, pPr);

        // Create borders
        CreateBorders(typoStyle, pPr);

        // Create font settings
        CreateFontSettings(typoStyle, styleRunProperties);

        // Justification
        CreateJustification(typoStyle, pPr);

        return style;
    }

    /// <summary>
    /// Set margins and indentation
    /// </summary>
    /// <param name="typoStyle">Type style</param>
    /// <param name="pPr">Paragraph properties</param>
    private static void CreateMargins(ITypoParagraphStyle typoStyle, ParagraphProperties pPr)
    {
        var left = MeasurementHelper.GetTwipsFromCm(typoStyle.TypoMargins.Left);
        var top = MeasurementHelper.GetTwipsFromCm(typoStyle.TypoMargins.Top);
        var right = MeasurementHelper.GetTwipsFromCm(typoStyle.TypoMargins.Right);
        var bottom = MeasurementHelper.GetTwipsFromCm(typoStyle.TypoMargins.Bottom);
        var leftFirstLine = MeasurementHelper.GetTwipsFromCm(typoStyle.FirstLineIndent);
        var line = MeasurementHelper.GetTwipsFromCm(typoStyle.LineHeight);

        LineSpacingRuleValues lsrv;

        switch (typoStyle.LineSpacingRule)
        {
            case LineSpacingRuleEnum.Exact:
                lsrv = LineSpacingRuleValues.Auto;
                break;
            case LineSpacingRuleEnum.AtLeast:
                lsrv = LineSpacingRuleValues.AtLeast;
                break;
            case LineSpacingRuleEnum.Auto:
            default:
                lsrv = LineSpacingRuleValues.Auto;
                break;
        }

        var spacing = new SpacingBetweenLines
        {
            Before = new StringValue(top.ToString()),
            After = new StringValue(bottom.ToString()),
            BeforeAutoSpacing = OnOffValue.FromBoolean(false),
            AfterAutoSpacing = OnOffValue.FromBoolean(false),
            LineRule = new EnumValue<LineSpacingRuleValues>(lsrv),
            Line = new StringValue(line.ToString())
        };
        pPr.Append(spacing);

        var indentation = new Indentation { Left = new StringValue(left.ToString()), Right = new StringValue(right.ToString()), FirstLine = new StringValue(leftFirstLine.ToString()) };
        pPr.Append(indentation);
    }

    /// <summary>
    /// Create paragraph settings like KeepLines, KeepNext etc.
    /// </summary>
    /// <param name="typoStyle">Type style</param>
    /// <param name="pPr">Paragraph properties</param>
    private static void CreateAdvancedParagraphSettings(ITypoParagraphStyle typoStyle, ParagraphProperties pPr)
    {
        // Keep the paragraph on one page if possible
        var keepLines = new KeepLines
        {
            Val = OnOffValue.FromBoolean(typoStyle.KeepTogether)
        };
        pPr.Append(keepLines);

        // Keep the paragraph with next on one page if possible
        var keepNext = new KeepNext
        {
            Val = OnOffValue.FromBoolean(typoStyle.KeepWithNextParagraph)
        };
        pPr.Append(keepNext);

        // page break before
        var pageBreakBefore = new PageBreakBefore
        {
            Val = OnOffValue.FromBoolean(typoStyle.PageBreakBefore)
        };
        pPr.Append(pageBreakBefore);

    }

    /// <summary>
    /// Create borders
    /// </summary>
    /// <param name="typoStyle">Type style</param>
    /// <param name="pPr">Paragraph properties</param>
    private static void CreateBorders(ITypoParagraphStyle typoStyle, ParagraphProperties pPr)
    {
        if (typoStyle.TypoBorderBrush == null)
        {
            return;
        }

        // Borders
        var tblBorders = new ParagraphBorders();
        pPr.Append(tblBorders);
        var borderColor = (typoStyle.TypoBorderBrush?.Color ?? TypoColors.Black).ToHtml();

        // Top border
        if (typoStyle.TypoBorderThickness.Top > 0)
        {
            var topBorder = new TopBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = borderColor,
                Size = MeasurementHelper.GetDxaFromCm(typoStyle.TypoBorderThickness.Top),
                Space = MeasurementHelper.GetDxaFromCm(typoStyle.TypoPaddings.Top)
            };
            tblBorders.AppendChild(topBorder);
        }

        // Bottom border
        if (typoStyle.TypoBorderThickness.Bottom > 0)
        {
            var bottomBorder = new BottomBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = borderColor,
                Size = MeasurementHelper.GetDxaFromCm(typoStyle.TypoBorderThickness.Bottom),
                Space = MeasurementHelper.GetDxaFromCm(typoStyle.TypoPaddings.Bottom)
            };
            tblBorders.AppendChild(bottomBorder);
        }

        // Right border
        if (typoStyle.TypoBorderThickness.Right > 0)
        {
            var rightBorder = new RightBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = borderColor,
                Size = MeasurementHelper.GetDxaFromCm(typoStyle.TypoBorderThickness.Right),
                Space = MeasurementHelper.GetDxaFromCm(typoStyle.TypoPaddings.Right)
            };
            tblBorders.AppendChild(rightBorder);
        }

        // Left border
        if (typoStyle.TypoBorderThickness.Left > 0)
        {
            var leftBorder = new LeftBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = borderColor,
                Size = MeasurementHelper.GetDxaFromCm(typoStyle.TypoBorderThickness.Left),
                Space = MeasurementHelper.GetDxaFromCm(typoStyle.TypoPaddings.Left)
            };
            tblBorders.AppendChild(leftBorder);
        }
    }

    /// <summary>
    /// Create font settings
    /// </summary>
    /// <param name="typoStyle">Type style</param>
    /// <param name="styleRunProperties">Style run properties to set</param>
    private static void CreateFontSettings(ITypoParagraphStyle typoStyle, StyleRunProperties styleRunProperties)
    {
        // Font color
        var fontColor = typoStyle.TypoFontColor ?? TypoColors.Black;
        var color1 = new Color { Val = fontColor.ToHtml() };
        styleRunProperties.Append(color1);

        // Font size
        // Specify a 16 point size. 16x2 because it’s half-point size
        var fontSize1 = new FontSize
        {
            Val = new StringValue((typoStyle.FontSize * 2).ToString("0"))
        };

        styleRunProperties.Append(fontSize1);

        // Font name
        var font = new RunFonts { Ascii = typoStyle.FontName };
        styleRunProperties.Append(font);

        // Bold
        styleRunProperties.Append(new Bold { Val = OnOffValue.FromBoolean(typoStyle.Bold) });

        // Italic
        styleRunProperties.Append(new Italic { Val = OnOffValue.FromBoolean(typoStyle.Italic) });
    }

    /// <summary>
    /// Create justification
    /// </summary>
    /// <param name="typoStyle">Type style</param>
    /// <param name="paragraphProperties">Style run properties to set</param>
    private static void CreateJustification(ITypoParagraphStyle typoStyle, ParagraphProperties paragraphProperties)
    {
        var justification = new Justification();

        switch (typoStyle.TextAlignment)
        {
            case TypoTextAlignment.Center:
                justification.Val = JustificationValues.Center;
                break;
            case TypoTextAlignment.Justify:
                justification.Val = JustificationValues.Both;
                break;
            case TypoTextAlignment.Right:
                justification.Val = JustificationValues.Right;
                break;
            case TypoTextAlignment.Left:
            default:
                justification.Val = JustificationValues.Left;
                break;
        }

        paragraphProperties.Append(justification);
    }

    /// <summary>
    /// Create a style
    /// </summary>
    /// <param name="styles">Styles list to add the new style</param>
    /// <param name="styleid">Style ID</param>
    /// <param name="stylename">Style name</param>
    /// <param name="uiPriority">UI priority</param>
    /// <param name="styleRunProperties">Current style run properties</param>
    /// <returns></returns>
    private static Style CreateStyle(Styles styles, string styleid, string stylename, int uiPriority, StyleRunProperties styleRunProperties)
    {
        var style = new Style
        {
            Type = StyleValues.Paragraph,
            StyleId = styleid,
            CustomStyle = true
        };
        style.Append(new StyleName { Val = stylename });
        style.Append(new BasedOn { Val = "Normal" });
        style.Append(new NextParagraphStyle { Val = "Normal" });
        style.Append(new UIPriority { Val = uiPriority });
        style.Append(styleRunProperties);

        styles.Append(style);
        return style;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        // Dispose doc first
        Docx?.Dispose();

        // And now the stream if available
        if (MemoryStream == null)
        {
            return;
        }
        MemoryStream.Close();
        MemoryStream.Dispose();
    }

    /// <summary>
    /// Add a paragraphic
    /// </summary>
    /// <param name="text">Text for the paragraph</param>
    /// <param name="styleName">Name of the style fosr the paragraph</param>
    public Paragraph AddParagraph(string text, string styleName)
    {
        var run = CreateRun(text);

        var list = new List<OpenXmlElement> { run };
        return AddParagraph(list, styleName);
    }

    /// <summary>
    /// Add a paragraph
    /// </summary>
    /// <param name="runs">Text parts to add to the paragraph</param>
    /// <param name="styleName">Name of the style for the paragraph</param>
    public Paragraph AddParagraph(IList<OpenXmlElement> runs, string styleName)
    {
        var para = CreateBaseParagraph(styleName);

        foreach (var run in runs)
        {
            para.AppendChild(run);
        }
        return para;
    }

    /// <summary>
    /// Add a paragraph
    /// </summary>
    /// <param name="path">Text for the paragraph</param>
    /// <param name="styleName">Name of the style for the paragraph</param>
    /// <param name="width">Width in pixels</param>
    /// <param name="height">Height in pixels</param>
    public Paragraph AddImage(string path, string styleName, int width, int height)
    {
        _imageCounter++;

        var para = CreateBaseParagraph(styleName);

        //para.ParagraphProperties?.AddChild(new Justification { Val = JustificationValues.Center });

        var xTwips = MeasurementHelper.GetEmuFromPx(width);
        var yTwips = MeasurementHelper.GetEmuFromPx(height);

        //var xTwips = 990000L;
        //var yTwips = 792000L;

        var fi = new FileInfo(path);

        var ext = fi.Extension.ToLowerInvariant();

        var ip = AddImagePart(path, ext);
        var relationshipId = MainDocumentPart.GetIdOfPart(ip);

        var inline = new DW.Inline(
            new DW.Extent { Cx = xTwips, Cy = yTwips },
            new DW.EffectExtent
            {
                LeftEdge = 0L,
                TopEdge = 0L,
                RightEdge = 0L,
                BottomEdge = 0L
            },
            new DW.WrapTopBottom(),
            new DW.HorizontalPosition(new DW.HorizontalAlignment("center"))
            {
                RelativeFrom = DW.HorizontalRelativePositionValues.Margin
            },
            new DW.VerticalPosition(new DW.PositionOffset("0"))
            {
                RelativeFrom = DW.VerticalRelativePositionValues.Paragraph
            },
            new DW.DocProperties
            {
                Id = (uint)_imageCounter,
                Name = $"Image {_imageCounter}"
            },
            new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks { NoChangeAspect = true }),
            new A.Graphic(
                new A.GraphicData(
                        new PIC.Picture(
                            new PIC.NonVisualPictureProperties(
                                new PIC.NonVisualDrawingProperties
                                {
                                    Id = (UInt32Value)0U,
                                    Name = fi.Name
                                },
                                new PIC.NonVisualPictureDrawingProperties()),
                            new PIC.BlipFill(
                                new A.Blip(
                                    new A.BlipExtensionList(
                                        new A.BlipExtension
                                        {
                                            Uri =
                                                "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                        })
                                )
                                {
                                    Embed = relationshipId,
                                    CompressionState =
                                        A.BlipCompressionValues.Print
                                },
                                new A.Stretch(
                                    new A.FillRectangle())),
                            new PIC.ShapeProperties(
                                new A.Transform2D(
                                    new A.Offset { X = 0L, Y = 0L },
                                    new A.Extents { Cx = xTwips, Cy = yTwips }),
                                new A.PresetGeometry(
                                        new A.AdjustValueList()
                                    )
                                { Preset = A.ShapeTypeValues.Rectangle }))
                    )
                { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
        )
        {
            DistanceFromTop = (UInt32Value)0U,
            DistanceFromBottom = (UInt32Value)0U,
            DistanceFromLeft = (UInt32Value)0U,
            DistanceFromRight = (UInt32Value)0U

            //EditId = "50D07946"
        };



        var element = new Drawing(inline);

        var rImg = new Run(element);
        para.Append(rImg);

        return para;
    }

    private Paragraph CreateBaseParagraph(string styleName)
    {
        var para = Body.AppendChild(new Paragraph());

        // If the paragraph has no ParagraphProperties object, create one.
        if (!para.Elements<ParagraphProperties>().Any())
        {
            para.PrependChild(new ParagraphProperties());
        }

        // Get a reference to the ParagraphProperties object.
        para.ParagraphProperties ??= new ParagraphProperties();
        var pPr = para.ParagraphProperties;

        // If a ParagraphStyleId object doesn't exist, create one.
        pPr.ParagraphStyleId ??= new ParagraphStyleId();

        // Set the style of the paragraph.
        pPr.ParagraphStyleId.Val = styleName;
        return para;
    }

    private ImagePart AddImagePart(string path, string ext)
    {
        PartTypeInfo imageType;

        switch (ext)
        {
            case ".png":
                imageType = ImagePartType.Png;
                break;
            case ".gif":
                imageType = ImagePartType.Gif;
                break;
            case ".jp2":
                imageType = ImagePartType.Jp2;
                break;
            case ".svg":
                imageType = ImagePartType.Svg;
                break;
            default:
                imageType = ImagePartType.Jpeg;
                break;
        }

        var imagePart = MainDocumentPart.AddImagePart(imageType);
        using var fis = new FileStream(path, FileMode.Open, FileAccess.Read);
        imagePart.FeedData(fis);
        return imagePart;
    }

    // https://learn.microsoft.com/en-us/dotnet/api/documentformat.openxml.wordprocessing.sectionproperties?view=openxml-3.0.1

    /// <summary>
    /// Add a section
    /// </summary>
    /// <param name="pageStyle">Current page style</param>
    /// <param name="isLastSection">Is the new section the last section. Default: true</param>
    /// <param name="restartPageNumbering">Restart page numbering</param>
    public SectionProperties AddSection(ITypoPageStyle pageStyle, bool isLastSection = true, bool restartPageNumbering = false)
    {

        if (CurrentSection != null)
        {
            var p = Body.Descendants<Paragraph>().LastOrDefault();
            if (p != null)
            {
                var pPr = p.ParagraphProperties;
                if (pPr == null)
                {
                    pPr = new ParagraphProperties();
                    p.Append(pPr);
                }

                pPr.Append(CurrentSection);
            }
        }


        var section = new SectionProperties();

        if (restartPageNumbering)
        {
            var pnt = new PageNumberType { Start = 1 };
            section.Append(pnt);
        }

        var sectionBreakType = new SectionType { Val = SectionMarkValues.NextPage };
        section.Append(sectionBreakType);

        if (pageStyle.NumberOfColumns > 1)
        {
            var spaceString = MeasurementHelper.GetDxaFromCm(pageStyle.Space).ToString("0");
            var columns = new Columns
            {
                EqualWidth = true,
                ColumnCount = (short)pageStyle.NumberOfColumns,
                Space = new StringValue(spaceString)
            };
            section.Append(columns);
        }

        // Paper size
        var width = MeasurementHelper.GetDxaFromCm(pageStyle.TypoPaperFormat.Size .Width);
        var height = MeasurementHelper.GetDxaFromCm(pageStyle.TypoPaperFormat.Size.Height);

        // Margins
        var left = MeasurementHelper.GetDxaFromCm(pageStyle.TypoMargins.Left);
        var top = MeasurementHelper.GetDxaFromCm(pageStyle.TypoMargins.Top);
        var right = MeasurementHelper.GetDxaFromCm(pageStyle.TypoMargins.Right);
        var bottom = MeasurementHelper.GetDxaFromCm(pageStyle.TypoMargins.Bottom);

        var pgSz = section.ChildElements.OfType<PageSize>().FirstOrDefault() ?? section.AppendChild(new PageSize { Width = width, Height = height });

        pgSz.Orient = pageStyle.TypoPaperFormat.Size.Width > pageStyle.TypoPaperFormat.Size.Height ? new EnumValue<PageOrientationValues>(PageOrientationValues.Landscape) : new EnumValue<PageOrientationValues>(PageOrientationValues.Portrait);

        var pageMargin = new PageMargin { Top = (int)top, Right = right, Bottom = (int)bottom, Left = left };
        section.Append(pageMargin);


        Sections.Add(section);
        CurrentSection = section;

        if (isLastSection)
        {
            Body.AddChild(section);
        }

        return section;
    }

    /// <summary>
    /// Create a hyperlink 
    /// </summary>
    /// <param name="url">Url</param>
    /// <param name="text">Text</param>
    /// <param name="mainPart">Current main part. Use <see cref="MainDocumentPart"/> normally</param>
    /// <returns>Hyperlink item</returns>
    public static Hyperlink CreateHyperlink(string url, string text, MainDocumentPart mainPart)
    {
        var hr = mainPart.AddHyperlinkRelationship(new Uri(url), true);
        var hrContactId = hr.Id;
        return new Hyperlink(
                    new ProofError { Type = ProofingErrorValues.GrammarStart },
                    new Run(
                        new RunProperties(
                            new RunStyle { Val = "Hyperlink" },
                            new Color { ThemeColor = ThemeColorValues.Hyperlink }),
                        new Text(text) { Space = SpaceProcessingModeValues.Preserve }
                    ))
        { History = OnOffValue.FromBoolean(true), Id = hrContactId };
    }

    /// <summary>
    /// Create a hyperlink 
    /// </summary>
    /// <param name="url">Url</param>
    /// <param name="runs">Text parts to add to the run</param>
    /// <param name="mainPart">Current main part. Use <see cref="MainDocumentPart"/> normally</param>
    /// <returns>Hyperlink item</returns>
    public static Hyperlink CreateHyperlink(string url, IList<OpenXmlElement> runs, MainDocumentPart mainPart)
    {
        var hr = mainPart.AddHyperlinkRelationship(new Uri(url), true);
        var hrContactId = hr.Id;

        var run = new Run(
            new RunProperties(
                new RunStyle { Val = "Hyperlink" },
                new Color { ThemeColor = ThemeColorValues.Hyperlink }));

        foreach (var subRun in runs)
        {
            //subRun.Space = SpaceProcessingModeValues.Preserve;
            run.AppendChild(subRun);
        }

        return new Hyperlink(
                new ProofError { Type = ProofingErrorValues.GrammarStart },
                run)
        { History = OnOffValue.FromBoolean(true), Id = hrContactId };
    }

    /// <summary>
    /// Create a simple run without formatting
    /// </summary>
    /// <param name="text">Text</param>
    /// <returns>Run object</returns>
    public static Run CreateRun(string text)
    {
        var run = new Run();
        //var rp = new RunProperties
        //{
        //    Bold = new Bold { Val = OnOffValue.FromBoolean(false) },
        //    Italic = new Italic { Val = OnOffValue.FromBoolean(false) }
        //};
        //// Always add properties first
        //run.Append(rp);
        run.AppendChild(new Text(text) { Space = SpaceProcessingModeValues.Preserve });
        return run;
    }

    /// <summary>
    /// Create a run with bold formatting
    /// </summary>
    /// <param name="text">Text</param>
    /// <returns>Run object</returns>
    public static Run CreateRunBold(string text)
    {
        var run = new Run();
        var rp = new RunProperties
        {
            Bold = new Bold { Val = OnOffValue.FromBoolean(true) }
        };
        // Always add properties first
        run.Append(rp);
        run.AppendChild(new Text(text) { Space = SpaceProcessingModeValues.Preserve });
        return run;
    }

    /// <summary>
    /// Create a run with italic formatting
    /// </summary>
    /// <param name="text">Text</param>
    /// <returns>Run object</returns>
    public static Run CreateRunItalic(string text)
    {
        var run = new Run();
        var rp = new RunProperties
        {
            Italic = new Italic { Val = OnOffValue.FromBoolean(true) }
        };
        // Always add properties first
        run.Append(rp);
        run.AppendChild(new Text(text) { Space = SpaceProcessingModeValues.Preserve });
        return run;
    }

    /// <summary>
    /// Create a simple run without formatting
    /// </summary>
    /// <param name="text">Text</param>
    /// <param name="useSpaceProcessingModePreserve">Use SpaceProcessingModeValues.Preserve? Intended mainly for hyperlinks</param>
    /// <returns>Run object</returns>
    public static Run CreateRun(string text, bool useSpaceProcessingModePreserve)
    {
        var run = new Run();
        //var rp = new RunProperties
        //{
        //    Italic = new Italic { Val = OnOffValue.FromBoolean(false) },
        //    Bold = new Bold { Val = OnOffValue.FromBoolean(false) }
        //};
        //// Always add properties first
        //run.Append(rp);
        run.AppendChild(useSpaceProcessingModePreserve
            ? new Text(text) { Space = SpaceProcessingModeValues.Preserve }
            : new Text(text));
        return run;
    }

    /// <summary>
    /// Create a simple run without formatting
    /// </summary>
    /// <param name="runs">Text parts to add to the run</param>
    /// <returns>Run object</returns>
    public static Run CreateRun(IList<OpenXmlElement> runs)
    {
        var run = new Run();
        foreach (var subRun in runs)
        {
            run.AppendChild(subRun);
        }
        return run;
    }

    /// <summary>
    /// Create a simple run with bold formatting
    /// </summary>
    /// <param name="runs">Text parts to add to the run</param>
    /// <returns>Run object</returns>
    public static Run CreateRunBold(IList<OpenXmlElement> runs)
    {
        var run = new Run();
        var rp = new RunProperties
        {
            Bold = new Bold { Val = OnOffValue.FromBoolean(true) }
        };
        // Always add properties first
        run.Append(rp);

        // Now add the sub runs
        foreach (var subRun in runs)
        {
            run.AppendChild(subRun);
        }
        return run;
    }

    /// <summary>
    /// Create a simple run with bold formatting
    /// </summary>
    /// <param name="runs">Text parts to add to the run</param>
    /// <returns>Run object</returns>
    public static Run CreateRunItalic(IList<OpenXmlElement> runs)
    {
        var run = new Run();
        var rp = new RunProperties
        {
            Italic = new Italic { Val = OnOffValue.FromBoolean(true) }
        };
        // Always add properties first
        run.Append(rp);

        // Now add the sub runs
        foreach (var subRun in runs)
        {
            run.AppendChild(subRun);
        }
        return run;
    }

    /// <summary>
    /// Create a line break
    /// </summary>
    /// <returns>Line break run</returns>
    public static Run CreateLineBreak()
    {
        return new Run(new Break { Type = BreakValues.TextWrapping });
    }

    /// <summary>
    /// Create a page break
    /// </summary>
    /// <returns>Page break run</returns>
    public static Run CreatePageBreak()
    {
        return new Run(new Break { Type = BreakValues.Page });
    }

    /// <summary>
    /// Create column break
    /// </summary>
    /// <returns>Column break run</returns>
    public static Run CreateColumnBreak()
    {
        return new Run(new Break { Type = BreakValues.Column });
    }

    /// <summary>
    /// Add a list of paragraphs
    /// </summary>
    /// <param name="listItems">List of paragraph items</param>
    /// <param name="styleId">Style ID to use for the paragraphs</param>
    /// <param name="listStyleType">List style type</param>
    public void AddList(List<List<OpenXmlElement>> listItems, string styleId, ListStyleTypeEnum listStyleType)
    {
        // Paragraph properties
        var sblUl = new SpacingBetweenLines { After = "0" }; // Get rid of space between bullets
        var iUl = new Indentation { Left = "360", Hanging = "360" }; // correct indentation

        var numberingId = 1;

        switch (listStyleType)
        {
            case ListStyleTypeEnum.Circle:
                numberingId = 1;
                break;
            case ListStyleTypeEnum.Square:
                numberingId = 1;
                break;
            case ListStyleTypeEnum.Decimal:
                numberingId = 3;
                break;
            case ListStyleTypeEnum.DecimalLeadingZero:
                break;
            case ListStyleTypeEnum.UpperRoman:
                numberingId = 5;
                break;
            case ListStyleTypeEnum.LowerRoman:
                numberingId = 4;
                break;
            case ListStyleTypeEnum.UpperLatin:
                numberingId = 7;
                break;
            case ListStyleTypeEnum.LowerLatin:
                numberingId = 6;
                break;
            case ListStyleTypeEnum.Customized:
                break;
            case ListStyleTypeEnum.Disc:
            default:
                numberingId = 1;
                break;
        }

        var npl = new NumberingProperties(
            new NumberingLevelReference { Val = 0 },
            new NumberingId { Val = numberingId }
        );

        var pp = new ParagraphProperties(npl, sblUl, iUl)
        {
            ParagraphStyleId = new ParagraphStyleId { Val = styleId }
        };

        foreach (var item in listItems)
        {
            CreateListItem(item, pp.OuterXml);
        }
    }

    private void CreateListItem(List<OpenXmlElement> items, string paragraphPropertiesXml)
    {
        var p1 = new Paragraph
        {
            ParagraphProperties = new ParagraphProperties(paragraphPropertiesXml)
        };

        foreach (var item in items)
        {
            p1.Append(item);
        }

        Body.Append(p1);
    }

    /// <summary>
    /// Add a table
    /// </summary>
    /// <param name="rows">List of rows</param>
    /// <param name="typoTableStyle">Style to use for the table</param>
    public void AddTable(List<DocxTableRow> rows, ITypoTableStyle typoTableStyle)
    {
        var table = new Table();

        // Create a TableProperties object and specify its border information.
        var borderSizeLeft = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.TypoBorderThickness.Left);
        var borderSizeRight = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.TypoBorderThickness.Right);
        var borderSizeTop = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.TypoBorderThickness.Top);
        var borderSizeBottom = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.TypoBorderThickness.Bottom);
        var borderSizeHorizontal = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.InsideHorizontalBorderWidth);
        var borderSizeVertical = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.InsideVerticalBorderWidth);

        var borderValue = BorderValues.Single;

        var tblProp = new TableProperties(
            new TableJustification { Val = TableRowAlignmentValues.Center },
            new TableLayout { Type = TableLayoutValues.Autofit },
            new TableBorders(
                new TopBorder
                {
                    Val = borderValue,
                    Size = borderSizeTop
                },
                new BottomBorder
                {
                    Val = borderValue,
                    Size = borderSizeBottom
                },
                new LeftBorder
                {
                    Val = borderValue,
                    Size = borderSizeLeft
                },
                new RightBorder
                {
                    Val = borderValue,
                    Size = borderSizeRight
                },
                new InsideHorizontalBorder
                {
                    Val = borderValue,
                    Size = borderSizeHorizontal
                },
                new InsideVerticalBorder
                {
                    Val = borderValue,
                    Size = borderSizeVertical
                }
            )
        );

        // Append the TableProperties object to the empty table.
        table.AppendChild(tblProp);


        foreach (var row in rows)
        {
            var tr = new TableRow();

            AddCells(tr, row.Cells);

            table.Append(tr);
        }

        var para = CreateEmptyParagraph(typoTableStyle.TypoMargins.Top);
        Body.AppendChild(para);

        Body.Append(table);
    }

    /// <summary>
    /// Create an empty paragraph with a certain top margin
    /// </summary>
    /// <param name="topMargin">Top margin in cm</param>
    /// <returns>Paragraph</returns>
    public static Paragraph CreateEmptyParagraph(double topMargin)
    {
        var para = new Paragraph();

        // If the paragraph has no ParagraphProperties object, create one.
        if (!para.Elements<ParagraphProperties>().Any())
        {
            para.PrependChild(new ParagraphProperties());
        }

        // Get a reference to the ParagraphProperties object.
        para.ParagraphProperties ??= new ParagraphProperties();
        var pPr = para.ParagraphProperties;

        var top = MeasurementHelper.GetTwipsFromCm(topMargin);

        var spacing = new SpacingBetweenLines
        {
            Before = new StringValue(top.ToString()),
        };
        pPr.Append(spacing);


        para.ParagraphProperties.ParagraphMarkRunProperties = new ParagraphMarkRunProperties();

        var fontSize1 = new FontSize
        {
            Val = new StringValue("2")
        };
        para.ParagraphProperties.ParagraphMarkRunProperties.Append(fontSize1);

        // string.Empty

        var run = new Run();

        var fontSize2 = new FontSize
        {
            Val = new StringValue("2")
        };

        var rp = new RunProperties
        {
            FontSize = fontSize2
        };

        // Always add properties first
        run.Append(rp);
        run.AppendChild(new Text(string.Empty) { Space = SpaceProcessingModeValues.Preserve });

        para.Append(run);

        return para;
    }

    private static void AddCells(TableRow row, List<DocxTableCell> cells)
    {

        foreach (var cell in cells)
        {
            var tc = new TableCell();

            foreach (var runs in cell.Items)
            {
                var para = new Paragraph();

                // If the paragraph has no ParagraphProperties object, create one.
                if (!para.Elements<ParagraphProperties>().Any())
                {
                    para.PrependChild(new ParagraphProperties());
                }

                // Get a reference to the ParagraphProperties object.
                para.ParagraphProperties ??= new ParagraphProperties();
                var pPr = para.ParagraphProperties;

                // If a ParagraphStyleId object doesn't exist, create one.
                pPr.ParagraphStyleId ??= new ParagraphStyleId();

                // Set the style of the paragraph.
                pPr.ParagraphStyleId.Val = cell.StyleId;

                foreach (var run in runs)
                {
                    para.AppendChild(run);
                }

                tc.Append(para);
            }

            row.Append(tc);
        }

    }

    /// <summary>
    /// Add adefinition list
    /// </summary>
    /// <param name="rows">Rows of the definition list</param>
    /// <param name="termWidth">Width of the term column in cm</param>
    /// <param name="itemsWidth">Width of the items column in cm</param>
    public void AddDefinitionList(List<DocxDefinitionListRow> rows, double termWidth, double itemsWidth)
    {
        var table = new Table();

        var leftColWidth = (double)MeasurementHelper.GetDxaFromCm(termWidth);
        var rightColWidth = (double)MeasurementHelper.GetDxaFromCm(itemsWidth);
        var total = leftColWidth + rightColWidth;

        var borderValue = BorderValues.None;
        const uint borderSize = 0u;

        var tblProp = new TableProperties(
            new TableWidth
            {
                Width = total.ToString("0"), 
                Type = TableWidthUnitValues.Dxa
            },
            new TableJustification
            {
                Val = TableRowAlignmentValues.Left
            },
            new TableBorders(
                new TopBorder
                {
                    Val = borderValue,
                    Size = borderSize
                },
                new BottomBorder
                {
                    Val = borderValue,
                    Size = borderSize
                },
                new LeftBorder
                {
                    Val = borderValue,
                    Size = borderSize
                },
                new RightBorder
                {
                    Val = borderValue,
                    Size = borderSize
                },
                new InsideHorizontalBorder
                {
                    Val = borderValue,
                    Size = borderSize
                },
                new InsideVerticalBorder
                {
                    Val = borderValue,
                    Size = borderSize
                }
            )
        );

        // Append the TableProperties object to the empty table.
        table.AppendChild(tblProp);

        // Append rows
        foreach (var row in rows)
        {
            var tr = new TableRow();

            // Left column
            var leftCell = CreateLeftColumn(row, leftColWidth/total);
            tr.Append(leftCell);

            // Right column
            var rightCell = CreateRightColumn(row, rightColWidth / total);
            tr.Append(rightCell);

            table.Append(tr);
        }

        Body.Append(table);

    }

    private static TableCell CreateLeftColumn(DocxDefinitionListRow row, double width)
    {
        //Debug.Print(width.ToString("P", CultureInfo.InvariantCulture));

        var tc = new TableCell();
        var tcp = new TableCellProperties
        {
            TableCellWidth = new TableCellWidth
            {
                Width = width.ToString("P", CultureInfo.InvariantCulture).Replace(" ", ""), 
                Type = TableWidthUnitValues.Pct
            }
        };
        tc.Append(tcp);

        var para = new Paragraph();

        // If the paragraph has no ParagraphProperties object, create one.
        if (!para.Elements<ParagraphProperties>().Any())
        {
            para.PrependChild(new ParagraphProperties());
        }

        // Get a reference to the ParagraphProperties object.
        para.ParagraphProperties ??= new ParagraphProperties();
        var pPr = para.ParagraphProperties;

        // If a ParagraphStyleId object doesn't exist, create one.
        pPr.ParagraphStyleId ??= new ParagraphStyleId();

        // Set the style of the paragraph.
        pPr.ParagraphStyleId.Val = row.TermStyleId;

        foreach (var run in row.Term)
        {
            para.AppendChild(run);
        }

        tc.Append(para);
        return tc;
    }

    private static TableCell CreateRightColumn(DocxDefinitionListRow row, double width)
    {
        var tc = new TableCell();
        var tcp = new TableCellProperties
        {
            TableCellWidth = new TableCellWidth
            {
                Width = width.ToString("P", CultureInfo.InvariantCulture).Replace(" ", ""), 
                Type = TableWidthUnitValues.Pct
            }
        };
        tc.Append(tcp);

        foreach (var item in row.Items)
        {

            var para = new Paragraph();

            // If the paragraph has no ParagraphProperties object, create one.
            if (!para.Elements<ParagraphProperties>().Any())
            {
                para.PrependChild(new ParagraphProperties());
            }

            // Get a reference to the ParagraphProperties object.
            para.ParagraphProperties ??= new ParagraphProperties();
            var pPr = para.ParagraphProperties;

            // If a ParagraphStyleId object doesn't exist, create one.
            pPr.ParagraphStyleId ??= new ParagraphStyleId();

            // Set the style of the paragraph.
            pPr.ParagraphStyleId.Val = row.ItemsStyleId;

            foreach (var run in item)
            {
                para.AppendChild(run);
            }

            tc.Append(para);
        }

        return tc;
    }
}