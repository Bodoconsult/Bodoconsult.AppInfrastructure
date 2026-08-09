// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Extensions;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Vml.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Diagnostics;
using System.Globalization;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using W = DocumentFormat.OpenXml.Wordprocessing;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using ParagraphProperties = DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Properties = DocumentFormat.OpenXml.ExtendedProperties.Properties;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Style = DocumentFormat.OpenXml.Wordprocessing.Style;
using Tabs = DocumentFormat.OpenXml.Wordprocessing.Tabs;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using HorizontalAnchorValues = DocumentFormat.OpenXml.Vml.Wordprocessing.HorizontalAnchorValues;
using Lock = DocumentFormat.OpenXml.Vml.Office.Lock;
using VerticalAnchorValues = DocumentFormat.OpenXml.Vml.Wordprocessing.VerticalAnchorValues;

// http://officeopenxml.com/

// https://stackoverflow.com/questions/79451553/add-background-image-in-word-with-openxml

// https://psp-it.com/blogs/open-xml-usage-part-one/

namespace Bodoconsult.Office;

/// <summary>
/// Create OpenXML DOCX files
/// </summary>
public class DocxBuilder : IDisposable
{
    private int _imageCounter = -1;
    private int _bookmarkCounter = -1;

    private readonly Dictionary<SectionProperties, HeaderPart> _headerParts = new();
    private readonly Dictionary<SectionProperties, FooterPart> _footerParts = new();
    private readonly Dictionary<SectionProperties, ITypoPageStyle> _pageStyles = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="documentMetaData">Current document meta data</param>
    public DocxBuilder(ITypoMetaData documentMetaData)
    {
        DocumentMetaData = documentMetaData;
    }

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
    /// Document meta data
    /// </summary>
    public ITypoMetaData DocumentMetaData { get; }

    /// <summary>
    /// Memory stream representing the document. Is only set if <see cref="CreateDocument()"/> was used to create the document
    /// </summary>
    public MemoryStream MemoryStream { get; private set; }

    /// <summary>
    /// All sections in the document
    /// </summary>
    public List<SectionProperties> Sections { get; } = [];

    /// <summary>
    /// Current section in the document
    /// </summary>
    public SectionProperties CurrentSection { get; private set; }

    /// <summary>
    /// The style to use for the watermark. Default: font-family:\"Calibri\";font-size:medium
    /// </summary>
    public string WatermarkStyle { get; set; } = "font-family:\"Calibri\";font-size:medium";

    /// <summary>
    /// Fill color for a watermark. Default: TypoColors.LightGray
    /// </summary>
    public TypoColor WatermarkFillColor { get; set; } = TypoColors.LightGray;

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
        if (DocumentMetaData != null)
        {
            var bProps = Docx.PackageProperties;
            bProps.Title = DocumentMetaData.Title;
            bProps.Creator = DocumentMetaData.Authors;

            // ToDo: make ext props working and add title

            var epPart = Docx.ExtendedFilePropertiesPart ?? Docx.AddExtendedFilePropertiesPart();

            // ReSharper disable once ConstantNullCoalescingCondition
            epPart.Properties ??= new Properties();

            var props = epPart.Properties;

            if (!string.IsNullOrEmpty(DocumentMetaData.Company))
            {
                props.Company = new Company(DocumentMetaData.Company);
            }
        }

        if (MemoryStream is null)
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
        //Settings.Settings.PrependChild(updateFields);
        //Settings.Settings.Save();

        // Add style part
        StyleDefinitionsPart = Docx.MainDocumentPart.StyleDefinitionsPart ?? AddStylesPartToPackage();
        Styles = StyleDefinitionsPart?.Styles;

        NumberingDefinitionsPart = MainDocumentPart.NumberingDefinitionsPart ??
                                   MainDocumentPart.AddNewPart<NumberingDefinitionsPart>("NumberingDefinitionsPart001");

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

        // Background image
        if (!string.IsNullOrEmpty(DocumentMetaData.BackgroundImagePath))
        {
            var fi = new FileInfo(DocumentMetaData.BackgroundImagePath);
            var ip = AddImagePart(MainDocumentPart, DocumentMetaData.BackgroundImagePath, fi.Extension);
            var id = MainDocumentPart.GetIdOfPart(ip);

            var docBg = new DocumentBackground
            {
                Color = "FFFFFF",
                Background = new Background
                {
                    Id = "_background",
                    //BlackWhiteMode = BlackAndWhiteModeValues.White,
                    //TargetScreenSize = ScreenSizeValues.Sz1024x768,
                    Fill = new Fill
                    {
                        RelationshipId = id,
                        Title = "background",
                        Recolor = false,
                        Type = FillTypeValues.Frame,
                        Aspect = ImageAspectValues.AtLeast,
                        AlignShape = true,
                        //Position = new StringValue("0")
                    },
                    Filled = true
                }
            };

            MainDocumentPart.Document.InsertAt(docBg, 0);

            var dbs = new DisplayBackgroundShape { Val = new OnOffValue(true) };
            Settings.Settings.Append(dbs);
        }
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

    /// <summary>
    /// Add a header to the current section
    /// </summary>
    /// <param name="position">Position of the page number (if &lt;&lt;Page&gt;&gt; is used) in cm relative to typearea</param>
    /// <param name="pageNumberFormat">Page number format</param>
    public void AddHeaderToCurrentSection(double position,
        PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {
        if (string.IsNullOrEmpty(DocumentMetaData.HeaderTemplate))
        {
            return;
        }

        if (!_headerParts.TryGetValue(CurrentSection, out var headerPart))
        {
            return;
        }

        var headerPartId = MainDocumentPart.GetIdOfPart(headerPart);

        const string styleId = "Header";

        var posTwips = MeasurementHelper.GetTwipsFromCm(position);

        var pPr = new ParagraphProperties(new ParagraphStyleId { Val = styleId });

        var para = new Paragraph(pPr);

        var sections = DocumentMetaData.HeaderTemplate.ToLowerInvariant().Split('|');

        // Draw left element
        CreateHeaderFooterElement(headerPart, DocumentMetaData, sections[0], para, 0, true, posTwips, pageNumberFormat,
            styleId);

        // Draw middle element
        CreateHeaderFooterElement(headerPart, DocumentMetaData, sections[1], para, 1, true, posTwips, pageNumberFormat,
            styleId);

        // Draw right element
        CreateHeaderFooterElement(headerPart, DocumentMetaData, sections[2], para, 2, true, posTwips, pageNumberFormat,
            styleId);


        //var para = CreateHeaderFooterParagraph(headerPart, $"\t{headerText}", styleId, posTwips, pageNumberFormat, true);

        headerPart.Header = new Header(para);

        CurrentSection.PrependChild(new HeaderReference
        {
            Id = headerPartId,
            Type = HeaderFooterValues.Default
        });
    }

    private void CreateHeaderFooterElement(OpenXmlPart docPart, ITypoMetaData typoMetaData, string section,
        Paragraph para, int position, bool isHeader, int posTwips, PageNumberFormatEnum pageNumberFormat,
        string styleId)
    {
        if (position == 1)
        {
            // Add a tab position
            para.ParagraphProperties ??= new ParagraphProperties();

            para.ParagraphProperties.Tabs = new Tabs();
            var tabStop = new TabStop
            {
                Val = TabStopValues.Center,
                Position = posTwips / 2
            };
            para.ParagraphProperties.Tabs.Append(tabStop);

            tabStop = new TabStop
            {
                Val = TabStopValues.Right,
                Position = posTwips
            };
            para.ParagraphProperties.Tabs.Append(tabStop);

            // Add the tab now
            var run = CreateRun("\t");
            para.Append(run);
        }

        // Logo
        if (section == ITypography.LogoIndicator && !string.IsNullOrEmpty(DocumentMetaData?.LogoPath))
        {
            _imageCounter++;
            var imageRun = CreateImageRun(docPart, DocumentMetaData.LogoPath,
                MeasurementHelper.GetPxFromCm(DocumentMetaData.LogoWidth), 0, _imageCounter, "Header");
            para.Append(imageRun);
        }

        // Tab
        if (section == "tab")
        {
            // Add a tab position
            para.ParagraphProperties ??= new ParagraphProperties();

            para.ParagraphProperties.Tabs = new Tabs();
            var tabStop = new TabStop
            {
                Val = TabStopValues.Right,
                Position = posTwips
            };
            para.ParagraphProperties.Tabs.Append(tabStop);

            tabStop = new TabStop
            {
                Val = TabStopValues.Center,
                Position = posTwips / 2
            };
            para.ParagraphProperties.Tabs.Append(tabStop);

            // Add the tab now
            var run = CreateRun("\t");
            para.Append(run);
        }

        // Footer / header text
        if (section == ITypography.TextIndicator)
        {
            var text = isHeader ? typoMetaData.HeaderText : typoMetaData.FooterText;

            if (string.IsNullOrEmpty(text))
            {
                text = typoMetaData.Title;
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }
            }

            var run = CreateRun(text);
            para.Append(run);
        }

        // Footer / header text
        if (section == ITypography.CompanyIndicator)
        {
            var text = typoMetaData.Company;

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var run = CreateRun(text);
            para.Append(run);
        }

        // Page number
        if (section == ITypography.PageFieldIndicator)
        {
            var pNf = pageNumberFormat switch
            {
                PageNumberFormatEnum.UpperRoman => "ROMAN",
                PageNumberFormatEnum.LowerRoman => "roman",
                PageNumberFormatEnum.UpperLatin => "ALPHABETIC",
                PageNumberFormatEnum.LowerLatin => "alphabetic",
                PageNumberFormatEnum.Decimal => "Arabic",
                _ => "Arabic"
            };

            if (!string.IsNullOrEmpty(typoMetaData.PageNumberPrefix))
            {
                var run = CreateRun($"{typoMetaData.PageNumberPrefix} ");
                para.Append(run);
            }

            para.Append(new Run(new FieldChar { FieldCharType = FieldCharValues.Begin }));
            para.Append(new Run(new FieldCode
            { Space = SpaceProcessingModeValues.Preserve, Text = $" PAGE  \\* {pNf}  \\* MERGEFORMAT " }));
            para.Append(new Run(new FieldChar { FieldCharType = FieldCharValues.Separate }));
            para.Append(new Run(new RunProperties(new NoProof()), new Text("1")));
            para.Append(new Run(new FieldChar { FieldCharType = FieldCharValues.End }));

        }

        // Date
        if (section == ITypography.DateIndicator)
        {
            var text = DateTime.Now.ToString("d", typoMetaData.CultureInfo);
            var run = CreateRun(text);
            para.Append(run);
        }

        // DateTime
        if (section == ITypography.DateTimeIndicator)
        {
            var text = DateTime.Now.ToString("g", typoMetaData.CultureInfo);
            var run = CreateRun(text);
            para.Append(run);
        }

        if (position == 1)
        {
            // Add the second tab now
            var run = CreateRun("\t");
            para.Append(run);
        }
    }

    /// <summary>
    /// Add a page refernece for a bookmark
    /// </summary>
    /// <param name="para">Paragraph to add the reference</param>
    /// <param name="bookmark">Name of the bookmark</param>
    public static void AddBookmarkRef(Paragraph para, string bookmark)
    {
        para.Append(new Run(new FieldChar { FieldCharType = FieldCharValues.Begin, Dirty = true }));
        para.Append(new Run(new FieldCode
        {
            Space = SpaceProcessingModeValues.Preserve,
            Text = $" PAGEREF {bookmark} \\# \"0\"  \\* MERGEFORMAT "
        }));
        para.Append(new Run(new FieldChar { FieldCharType = FieldCharValues.Separate }));
        para.Append(new Run(new RunProperties(new NoProof())));
        para.Append(new Run(new FieldChar { FieldCharType = FieldCharValues.End }));
    }


    /// <summary>
    /// Add a footer to the current section
    /// </summary>
    /// <param name="position">Position of the page number (if &lt;&lt;Page&gt;&gt; is used) in cm relative to typearea</param>
    /// <param name="pageNumberFormat">Page number format</param>
    public void AddFooterToCurrentSection(double position,
        PageNumberFormatEnum pageNumberFormat = PageNumberFormatEnum.Decimal)
    {

        if (!_footerParts.TryGetValue(CurrentSection, out var footerPart))
        {
            return;
        }

        var footerPartId = MainDocumentPart.GetIdOfPart(footerPart );

        const string styleId = "Footer";

        var posTwips = MeasurementHelper.GetTwipsFromCm(position);

        var pPr = new ParagraphProperties(new ParagraphStyleId { Val = styleId });

        var para = new Paragraph(pPr);

        var sections = DocumentMetaData.FooterTemplate.ToLowerInvariant().Split('|');

        // Draw left element
        CreateHeaderFooterElement(footerPart, DocumentMetaData, sections[0], para, 0, false, posTwips, pageNumberFormat,
            styleId);

        // Draw middle element
        CreateHeaderFooterElement(footerPart, DocumentMetaData, sections[1], para, 1, false, posTwips, pageNumberFormat,
            styleId);

        // Draw right element
        CreateHeaderFooterElement(footerPart, DocumentMetaData, sections[2], para, 2, false, posTwips, pageNumberFormat,
            styleId);

        footerPart.Footer = new Footer(para);

        CurrentSection.PrependChild(new FooterReference
        {
            Id = footerPartId,
            Type = HeaderFooterValues.Default
        });
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
        Debug.Print(
            $"{styleid}: L{typoStyle.TypoMargins.Left} T{typoStyle.TypoMargins.Top} R{typoStyle.TypoMargins.Right} B{typoStyle.TypoMargins.Bottom}");
        Debug.Print(
            $"{styleid}: L{typoStyle.TypoPaddings.Left} T{typoStyle.TypoPaddings.Top} R{typoStyle.TypoPaddings.Right} B{typoStyle.TypoPaddings.Bottom}");
        Debug.Print(
            $"{styleid}: L{typoStyle.TypoBorderThickness.Left} T{typoStyle.TypoBorderThickness.Top} R{typoStyle.TypoBorderThickness.Right} B{typoStyle.TypoBorderThickness.Bottom}");

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

        var lsrv = typoStyle.LineSpacingRule switch
        {
            LineSpacingRuleEnum.Exact => LineSpacingRuleValues.Auto,
            LineSpacingRuleEnum.AtLeast => LineSpacingRuleValues.AtLeast,
            LineSpacingRuleEnum.Auto => LineSpacingRuleValues.Auto,
            _ => LineSpacingRuleValues.Auto
        };

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

        Debug.Print(left.ToString());


        var indentation = new Indentation
        {
            Left = new StringValue(left.ToString()),
            //Start = new StringValue(left.ToString()),
            Right = new StringValue(right.ToString()),

        };

        if (leftFirstLine < 0)
        {
            indentation.Hanging = new StringValue(Math.Abs(leftFirstLine).ToString());
        }
        else
        {
            indentation.FirstLine = new StringValue(leftFirstLine.ToString());
        }

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

        // widow control
        var widowControl = new WidowControl
        {
            Val = OnOffValue.FromBoolean(typoStyle.WidowControl)
        };
        pPr.Append(widowControl);

        // Shading
        if (typoStyle.TypoShading != null)
        {
            var shading = new Shading
            {
                Fill = typoStyle.TypoShading.ToHtml2()
            };
            pPr.Append(shading);
        }
    }

    /// <summary>
    /// Create borders
    /// </summary>
    /// <param name="typoStyle">Type style</param>
    /// <param name="pPr">Paragraph properties</param>
    private static void CreateBorders(ITypoParagraphStyle typoStyle, ParagraphProperties pPr)
    {
        if (typoStyle.TypoBorderBrush is null)
        {
            return;
        }

        // Borders
        var tblBorders = new ParagraphBorders();
        pPr.Append(tblBorders);
        var borderColor = new StringValue
        { Value = (typoStyle.TypoBorderBrush?.TypoColor ?? TypoColors.Black).ToHtml2() };

        // Top border
        if (typoStyle.TypoBorderThickness.Top > 0)
        {
            var topBorder = new W.TopBorder
            {
                Val = new EnumValue<W.BorderValues>(W.BorderValues.Thick),
                Color = borderColor,
                Size = GetValidPtValue(typoStyle.TypoBorderThickness.Top),
                Space = GetValidPtValue(typoStyle.TypoPaddings.Top)
            };
            tblBorders.AppendChild(topBorder);
        }

        // Bottom border
        if (typoStyle.TypoBorderThickness.Bottom > 0)
        {
            var bottomBorder = new W.BottomBorder
            {
                Val = new EnumValue<W.BorderValues>(W.BorderValues.Thick),
                Color = borderColor,
                Size = GetValidPtValue(typoStyle.TypoBorderThickness.Bottom),
                Space = GetValidPtValue(typoStyle.TypoPaddings.Bottom)
            };
            tblBorders.AppendChild(bottomBorder);
        }

        // Right border
        if (typoStyle.TypoBorderThickness.Right > 0)
        {
            var rightBorder = new W.RightBorder
            {
                Val = new EnumValue<W.BorderValues>(W.BorderValues.Thick),
                Color = borderColor,
                Size = GetValidPtValue(typoStyle.TypoBorderThickness.Right),
                Space = GetValidPtValue(typoStyle.TypoPaddings.Right)
            };
            tblBorders.AppendChild(rightBorder);
        }

        // Left border
        if (typoStyle.TypoBorderThickness.Left > 0)
        {
            var leftBorder = new W.LeftBorder
            {
                Val = new EnumValue<W.BorderValues>(W.BorderValues.Thick),
                Color = borderColor,
                Size = GetValidPtValue(typoStyle.TypoBorderThickness.Left),
                Space = GetValidPtValue(typoStyle.TypoPaddings.Left)
            };
            tblBorders.AppendChild(leftBorder);
        }
    }

    private static UInt32Value GetValidPtValue(double value)
    {
        var result = MeasurementHelper.GetPtFromCm(value);
        //if (result > 31)
        //{
        //    result = 31;
        //}

        return new UInt32Value((uint)result);
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
        var color1 = new Color { Val = fontColor.ToHtml2() };
        styleRunProperties.Append(color1);

        // Font size
        // Specify a 16 point size. 16x2 because it’s half-point size
        var fontSize1 = new FontSize
        {
            Val = new StringValue((typoStyle.FontSize * 2).ToString("0"))
        };

        styleRunProperties.Append(fontSize1);

        //// Font name
        //var font = new RunFonts { Ascii = typoStyle.FontName };
        //styleRunProperties.Append(font);

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
    private static Style CreateStyle(Styles styles, string styleid, string stylename, int uiPriority,
        StyleRunProperties styleRunProperties)
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
        if (MemoryStream is null)
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
    /// Add a paragraphic
    /// </summary>
    /// <param name="text">Text for the paragraph</param>
    /// <param name="styleName">Name of the style fosr the paragraph</param>
    /// <param name="bookmark">Name of an empty bookmark to add</param>
    public Paragraph AddParagraph(string text, string styleName, string bookmark)
    {
        _bookmarkCounter++;

        var run = CreateRun(text);

        var list = new List<OpenXmlElement> { run };
        var para = AddParagraph(list, styleName);

        var bstart = new BookmarkStart { Id = _bookmarkCounter.ToString(), Name = bookmark };
        para.Append(bstart);

        var bend = new BookmarkEnd { Id = _bookmarkCounter.ToString() };
        para.Append(bend);
        return para;
    }

    /// <summary>
    /// Add a paragraph
    /// </summary>
    /// <param name="runs">Text parts to add to the paragraph</param>
    /// <param name="styleName">Name of the style for the paragraph</param>
    /// <param name="bookmark">Name of an empty bookmark to add</param>
    public Paragraph AddParagraph(IList<OpenXmlElement> runs, string styleName, string bookmark)
    {
        _bookmarkCounter++;

        var para = CreateBaseParagraph(styleName);

        var bstart = new BookmarkStart { Id = _bookmarkCounter.ToString(), Name = bookmark };
        para.Append(bstart);

        var bend = new BookmarkEnd { Id = _bookmarkCounter.ToString() };
        para.Append(bend);

        foreach (var run in runs)
        {
            para.AppendChild(run);
        }

        return para;
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
        var para = CreateBaseParagraph(styleName);

        //para.ParagraphProperties?.AddChild(new Justification { Val = JustificationValues.Center });

        _imageCounter++;
        var rImg = CreateImageRun(MainDocumentPart, path, width, height, _imageCounter);
        para.Append(rImg);

        return para;
    }

    /// <summary>
    /// Create a run with an image
    /// </summary>
    /// <param name="mainDocumentPart">Main document part</param>
    /// <param name="path">Path to the image</param>
    /// <param name="width">Width in px</param>
    /// <param name="height">Height in px</param>
    /// <param name="imageCounter">Current image counter</param>
    /// <param name="prefix">Prefix to separate header / footer images</param>
    /// <returns>Run with the image</returns>
    public static Run CreateImageRun(OpenXmlPart mainDocumentPart, string path, int width, int height, int imageCounter,
        string prefix = null)
    {
        Debug.Print($"Image {imageCounter}");

        var xTwips = MeasurementHelper.GetEmuFromPx(width);

        if (height == 0)
        {
            height = (int)(width / TypographicConstants.GoldenerSchnittRatio);
        }

        var yTwips = MeasurementHelper.GetEmuFromPx(height);

        //var xTwips = 990000L;
        //var yTwips = 792000L;

        var fi = new FileInfo(path);

        var ext = fi.Extension.ToLowerInvariant();

        var ip = AddImagePart(mainDocumentPart, path, ext);
        var relationshipId = mainDocumentPart.GetIdOfPart(ip);

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
                Id = (uint)imageCounter,
                Name = $"{prefix}Image {imageCounter}"
            },
            new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks { NoChangeAspect = true }),
            new A.Graphic(
                new A.GraphicData(
                        new PIC.Picture(
                            new PIC.NonVisualPictureProperties(
                                new PIC.NonVisualDrawingProperties
                                {
                                    Id = (uint)imageCounter,
                                    Name = fi.Name
                                },
                                new PIC.NonVisualPictureDrawingProperties()),
                            new PIC.BlipFill(
                                new A.Blip(
                                    new A.BlipExtensionList(
                                        new A.BlipExtension
                                        {
                                            Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                        })
                                )
                                {
                                    Embed = relationshipId,
                                    CompressionState = A.BlipCompressionValues.Print
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
        return rImg;
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

    private static ImagePart AddImagePart(OpenXmlPart docPart, string path, string ext)
    {
        var imageType = ext switch
        {
            ".png" => ImagePartType.Png,
            ".gif" => ImagePartType.Gif,
            ".jp2" => ImagePartType.Jp2,
            ".svg" => ImagePartType.Svg,
            _ => ImagePartType.Jpeg
        };

        switch (docPart)
        {
            case MainDocumentPart m:
                {
                    var imagePart = m.AddImagePart(imageType);
                    using var fis = new FileStream(path, FileMode.Open, FileAccess.Read);
                    imagePart.FeedData(fis);
                    fis.Close();
                    return imagePart;
                }
            case HeaderPart h:
                {
                    var imagePart = h.AddImagePart(imageType);
                    using var fis = new FileStream(path, FileMode.Open, FileAccess.Read);
                    imagePart.FeedData(fis);
                    fis.Close();
                    return imagePart;
                }
            case FooterPart f:
                {
                    var imagePart = f.AddImagePart(imageType);
                    using var fis = new FileStream(path, FileMode.Open, FileAccess.Read);
                    imagePart.FeedData(fis);
                    fis.Close();
                    return imagePart;
                }
            default:
                {
                    return null;
                }
        }
    }

    // https://learn.microsoft.com/en-us/dotnet/api/documentformat.openxml.wordprocessing.sectionproperties?view=openxml-3.0.1

    /// <summary>
    /// Add a section
    /// </summary>
    /// <param name="pageStyle">Current page style</param>
    /// <param name="isLastSection">Is the new section the last section. Default: true</param>
    /// <param name="restartPageNumbering">Restart page numbering</param>
    public SectionProperties AddSection(ITypoPageStyle pageStyle, bool isLastSection = true,
        bool restartPageNumbering = false)
    {

        if (CurrentSection != null)
        {
            var p = Body.Descendants<Paragraph>().LastOrDefault();
            if (p != null)
            {
                var pPr = p.ParagraphProperties;
                if (pPr is null)
                {
                    pPr = new ParagraphProperties();
                    p.Append(pPr);
                }

                pPr.Append(CurrentSection);
            }
        }


        var section = new SectionProperties();
        _pageStyles.Add(section, pageStyle);

        if (restartPageNumbering)
        {
            var pnt = new PageNumberType { Start = 1 };
            section.Append(pnt);
        }

        var sectionBreakType = new SectionType { Val = SectionMarkValues.NextPage };
        section.Append(sectionBreakType);

        if (pageStyle.NumberOfColumns > 1)
        {
            var spaceString = MeasurementHelper.GetDxaFromCm(pageStyle.ColumnGap).ToString("0");
            var columns = new Columns
            {
                EqualWidth = true,
                ColumnCount = (short)pageStyle.NumberOfColumns,
                Space = new StringValue(spaceString)
            };
            section.Append(columns);
        }

        // Paper size
        var width = MeasurementHelper.GetDxaFromCm(pageStyle.TypoPaperFormat.Size.Width);
        var height = MeasurementHelper.GetDxaFromCm(pageStyle.TypoPaperFormat.Size.Height);

        // Margins
        var left = MeasurementHelper.GetDxaFromCm(pageStyle.TypoMargins.Left);
        var top = MeasurementHelper.GetDxaFromCm(pageStyle.TypoMargins.Top);
        var right = MeasurementHelper.GetDxaFromCm(pageStyle.TypoMargins.Right);
        var bottom = MeasurementHelper.GetDxaFromCm(pageStyle.TypoMargins.Bottom);

        var pgSz = section.ChildElements.OfType<PageSize>().FirstOrDefault() ??
                   section.AppendChild(new PageSize { Width = width, Height = height });

        pgSz.Orient = pageStyle.TypoPaperFormat.Size.Width > pageStyle.TypoPaperFormat.Size.Height
            ? new EnumValue<PageOrientationValues>(PageOrientationValues.Landscape)
            : new EnumValue<PageOrientationValues>(PageOrientationValues.Portrait);

        var pageMargin = new PageMargin
        {
            Top = (int)top,
            Right = right,
            Bottom = (int)bottom,
            Left = left,
            Header = (uint)(top * 0.25)
        };
        section.Append(pageMargin);


        // Footer part
        if (!string.IsNullOrEmpty(DocumentMetaData.FooterTemplate) || string.IsNullOrEmpty(DocumentMetaData.WatermarkText))
        {
            var fp = MainDocumentPart.AddNewPart<FooterPart>();
            _footerParts.Add(section, fp);
        }

        if (!string.IsNullOrEmpty(DocumentMetaData.HeaderTemplate))
        {
            var hp = MainDocumentPart.AddNewPart<HeaderPart>();
            _headerParts.Add(section, hp);
        }

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
                        new Color { Val = new StringValue(TypoColors.Blue.ToHtml2()) }),
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
                new Color { Val = TypoColors.Blue.ToHtml2() }));

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
        var iUl = new Indentation
        {
            Left = new StringValue { Value = "360" },
            //Start = new StringValue { Value = "360" },
            Hanging = new StringValue { Value = "360" }
        }; // correct indentation

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
    /// <param name="bookmark">Name of an empty bookmark to add</param>
    public void AddTable(List<DocxTableRow> rows, ITypoTableStyle typoTableStyle, string bookmark)
    {
        var table = new Table();

        // Create a TableProperties object and specify its border information.
        var borderSizeLeft = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.TypoBorderThickness.Left);
        var borderSizeRight = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.TypoBorderThickness.Right);
        var borderSizeTop = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.TypoBorderThickness.Top);
        var borderSizeBottom = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.TypoBorderThickness.Bottom);
        var borderSizeHorizontal = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.InsideHorizontalBorderWidth);
        var borderSizeVertical = (uint)MeasurementHelper.GetTwipsFromCm(typoTableStyle.InsideVerticalBorderWidth);

        var borderValue = W.BorderValues.Single;

        var color = new StringValue { Value = typoTableStyle.TypoBorderBrush.TypoColor.ToHtml2() };

        var tblProp = new TableProperties(
            new TableJustification { Val = TableRowAlignmentValues.Center },
            new TableLayout { Type = TableLayoutValues.Autofit },
            new TableBorders(
                new W.TopBorder
                {
                    Val = borderValue,
                    Size = borderSizeTop,
                    Color = color
                },
                new W.BottomBorder
                {
                    Val = borderValue,
                    Size = borderSizeBottom,
                    Color = color
                },
                new W.LeftBorder
                {
                    Val = borderValue,
                    Size = borderSizeLeft,
                    Color = color
                },
                new W.RightBorder
                {
                    Val = borderValue,
                    Size = borderSizeRight,
                    Color = color
                },
                new W.InsideHorizontalBorder
                {
                    Val = borderValue,
                    Size = borderSizeHorizontal,
                    Color = color
                },
                new W.InsideVerticalBorder
                {
                    Val = borderValue,
                    Size = borderSizeVertical,
                    Color = color
                }
            )
        );

        // Append the TableProperties object to the empty table.
        table.AppendChild(tblProp);

        // Add a grid
        var grid = new TableGrid();
        table.Append(grid);

        var headerColor = typoTableStyle.TypoTableHeaderBackColor.ToHtml2();
        var backColor = typoTableStyle.TypoTableBackColor.ToHtml2();
        var alternateBackColor = typoTableStyle.TypoTableAlternateBackColor.ToHtml2();

        // Add rows now
        for (var index = 0; index < rows.Count; index++)
        {
            var row = rows[index];
            var tr = new TableRow();

            string cellColor;

            if (index == 0)
            {
                cellColor = headerColor;
            }
            else
            {
                cellColor = index % 2.0 < 0.01 ? alternateBackColor : backColor;
            }

            AddCells(tr, row.Cells, cellColor);

            table.Append(tr);
        }

        var para = CreateEmptyParagraph(typoTableStyle.TypoMargins.Top);
        if (!string.IsNullOrEmpty(bookmark))
        {
            _bookmarkCounter++;

            var bstart = new BookmarkStart { Id = _bookmarkCounter.ToString(), Name = bookmark };
            para.Append(bstart);

            var bend = new BookmarkEnd { Id = _bookmarkCounter.ToString() };
            para.Append(bend);
        }


        Body.AppendChild(para);

        Body.Append(table);
    }

    /// <summary>
    /// Add a table
    /// </summary>
    /// <param name="rows">List of rows</param>
    /// <param name="typoTableStyle">Style to use for the table</param>
    public void AddTable(List<DocxTableRow> rows, ITypoTableStyle typoTableStyle)
    {
        AddTable(rows, typoTableStyle, null);
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

    private static void AddCells(TableRow row, List<DocxTableCell> cells, string cellColor)
    {

        foreach (var cell in cells)
        {
            var tc = new TableCell
            {
                TableCellProperties = new TableCellProperties
                {
                    Shading = new Shading { Fill = cellColor }
                }
            };

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

        var borderValue = W.BorderValues.None;
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
                new W.TopBorder
                {
                    Val = borderValue,
                    Size = borderSize
                },
                new W.BottomBorder
                {
                    Val = borderValue,
                    Size = borderSize
                },
                new W.LeftBorder
                {
                    Val = borderValue,
                    Size = borderSize
                },
                new W.RightBorder
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
                })
            );

        // Append the TableProperties object to the empty table.
        table.Append(tblProp);

        var grid = new TableGrid();
        table.Append(grid);

        // Append rows
        foreach (var row in rows)
        {
            var tr = new TableRow();

            // Left column
            var leftCell = CreateLeftColumn(row, leftColWidth / total);
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
                Width = $"{Math.Round(width * 100, 0).ToString("0", CultureInfo.InvariantCulture).Replace(" ", "")}",
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
                Width = $"{Math.Round(width * 100, 0).ToString("0", CultureInfo.InvariantCulture).Replace(" ", "")}",
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

    /// <summary>
    /// Adds the watermark if given WatermarkText in <see cref="DocumentMetaData"/> is given
    /// </summary>
    public void AddWatermark()
    {
        var id = 0;

        foreach (var kvp in _headerParts)
        {
            var headerPart = kvp.Value;
            var section = kvp.Key;

            if (!_pageStyles.TryGetValue(section, out var pageStyle))
            {
                continue;
            }

            var sdtBlock1 = new SdtBlock();
            var sdtProperties1 = new SdtProperties();
            var sdtId1 = new SdtId { Val = 87908844 };
            var sdtContentDocPartObject1 = new SdtContentDocPartObject();
            var docPartGallery1 = new DocPartGallery { Val = "Watermarks" };
            var docPartUnique1 = new DocPartUnique();

            sdtContentDocPartObject1.Append(docPartGallery1);
            sdtContentDocPartObject1.Append(docPartUnique1);
            sdtProperties1.Append(sdtId1);
            sdtProperties1.Append(sdtContentDocPartObject1);

            var sdtContentBlock1 = new SdtContentBlock();
            var paragraph2 = new Paragraph
            {
                RsidParagraphAddition = "00656E18",
                RsidRunAdditionDefault = "00656E18"
            };

            var paragraphProperties2 = new ParagraphProperties();
            var paragraphStyleId2 = new ParagraphStyleId { Val = "Header" };
            paragraphProperties2.Append(paragraphStyleId2);

            var run1 = new Run();
            var runProperties1 = new RunProperties();
            var noProof1 = new NoProof();
            var languages1 = new Languages { EastAsia = "en-US" };
            runProperties1.Append(noProof1);
            runProperties1.Append(languages1);
            var picture1 = new Picture();
            var shapetype1 = new Shapetype
            {
                Id = "_x0000_t136",
                CoordinateSize = "21600,21600",
                OptionalNumber = 136,
                Adjustment = "10800",
                EdgePath = "m@7,l@8,m@5,21600l@6,21600e"
            };

            var formulas1 = new Formulas();
            var formula1 = new Formula { Equation = "sum #0 0 10800" };
            var formula2 = new Formula { Equation = "prod #0 2 1" };
            var formula3 = new Formula { Equation = "sum 21600 0 @1" };
            var formula4 = new Formula { Equation = "sum 0 0 @2" };
            var formula5 = new Formula { Equation = "sum 21600 0 @3" };
            var formula6 = new Formula { Equation = "if @0 @3 0" };
            var formula7 = new Formula { Equation = "if @0 21600 @1" };
            var formula8 = new Formula { Equation = "if @0 0 @2" };
            var formula9 = new Formula { Equation = "if @0 @4 21600" };
            var formula10 = new Formula { Equation = "mid @5 @6" };
            var formula11 = new Formula { Equation = "mid @8 @5" };
            var formula12 = new Formula { Equation = "mid @7 @8" };
            var formula13 = new Formula { Equation = "mid @6 @7" };
            var formula14 = new Formula { Equation = "sum @6 0 @5" };

            formulas1.Append(formula1);
            formulas1.Append(formula2);
            formulas1.Append(formula3);
            formulas1.Append(formula4);
            formulas1.Append(formula5);
            formulas1.Append(formula6);
            formulas1.Append(formula7);
            formulas1.Append(formula8);
            formulas1.Append(formula9);
            formulas1.Append(formula10);
            formulas1.Append(formula11);
            formulas1.Append(formula12);
            formulas1.Append(formula13);
            formulas1.Append(formula14);

            var path1 = new DocumentFormat.OpenXml.Vml.Path
            {
                AllowTextPath = true,
                ConnectionPointType = ConnectValues.Custom,
                ConnectionPoints = "@9,0;@10,10800;@11,21600;@12,10800",
                ConnectAngles = "270,180,90,0"
            };

            var textPath1 = new TextPath
            {
                On = true,
                FitShape = true
            };

            var shapeHandles1 = new ShapeHandles();

            var shapeHandle1 = new ShapeHandle { Position = "#0,bottomRight", XRange = "6629,14971" };

            shapeHandles1.Append(shapeHandle1);

            var lock1 = new Lock
            {
                Extension = ExtensionHandlingBehaviorValues.Edit,
                TextLock = true,
                ShapeType = true
            };

            shapetype1.Append(formulas1);
            shapetype1.Append(path1);
            shapetype1.Append(textPath1);
            shapetype1.Append(shapeHandles1);
            shapetype1.Append(lock1);

            //// Create the diagonal watermark shape
            var width = MeasurementHelper.GetPtFromCm(pageStyle.TypoPaperFormat.Size.Width - pageStyle.TypoMargins.Left -
                                                  pageStyle.TypoMargins.Right);
            var shape1 = new Shape
            {
                Id = $"WMO{id}",
                Style = $"position:absolute;left:0;text-align:left;margin-left:0;margin-top:0;width:{width}pt;height:{MeasurementHelper.GetPtFromCm(pageStyle.TypoPaperFormat.Size.Height)}pt;rotation:315;z-index:-251656192;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin",
                OptionalString = "_x0000_s2049",
                AllowInCell = true,
                FillColor = WatermarkFillColor.ToHtml(),
                Stroked = false,
                Type = "#_x0000_t136"
            };

            //// Switches on the transparency
            var fill1 = new Fill { Opacity = ".5" };

            //// Font Style and display text
            var textPath2 = new TextPath
            {
                Style = WatermarkStyle,
                String = DocumentMetaData.WatermarkText
            };

            var textWrap1 = new TextWrap
            {
                AnchorX = HorizontalAnchorValues.Margin,
                AnchorY = VerticalAnchorValues.Margin
            };

            shape1.Append(fill1);
            shape1.Append(textPath2);
            shape1.Append(textWrap1);
            picture1.Append(shapetype1);
            picture1.Append(shape1);
            run1.Append(runProperties1);
            run1.Append(picture1);
            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run1);
            sdtContentBlock1.Append(paragraph2);
            sdtBlock1.Append(sdtProperties1);
            sdtBlock1.Append(sdtContentBlock1);

            headerPart.Header ??= new Header();
            headerPart.Header.Append(sdtBlock1);

            id++;
        }
    }
}
