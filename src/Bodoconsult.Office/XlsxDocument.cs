// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using A = DocumentFormat.OpenXml.Drawing;
using Ap = DocumentFormat.OpenXml.ExtendedProperties;
using Vt = DocumentFormat.OpenXml.VariantTypes;
using WorkbookProperties = DocumentFormat.OpenXml.Spreadsheet.WorkbookProperties;

// ReSharper disable PossiblyMistakenUseOfParamsMethod

namespace Bodoconsult.Office;

/// <summary>
/// Represents a OpenXML XLSX document
/// </summary>
public partial class XlsxDocument
{

    //private const uint DefaultColimnWidth = 25U;

    /// <summary>
    /// Get the page margins
    /// </summary>
    /// <returns></returns>
    public static PageMargins GetPageMargins()
    {
        return new PageMargins
        {
            Left = 0.7D,
            Right = 0.7D,
            Top = 0.7D,
            Bottom = 0.7D,
            Header = 0.3D,
            Footer = 0.3D
        };
    }

    ///// <summary>
    ///// Defult ctor
    ///// </summary>
    //public XlsxDocument()
    //{

    //}

    /// <summary>
    /// Numberformat to use for <see cref="double"/> values
    /// </summary>
    public string NumberFormatDouble { get; set; } = "#,##0.00";

    /// <summary>
    /// Creates a <see cref="SpreadsheetDocument"/> instance.
    /// </summary>
    /// <param name="filePath">File to save the spreadsheet in</param>
    /// <returns><see cref="SpreadsheetDocument"/> instance created</returns>
    public SpreadsheetDocument CreatePackage(string filePath)
    {
        var document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
        CreateParts(document);
        return document;
    }
    
    /// <summary>
    /// Adds child parts and generates content of the specified part
    /// </summary>
    /// <param name="document">Current spreadsheet</param>
    private void CreateParts(SpreadsheetDocument document)
    {
        var extendedFilePropertiesPart1 = document.AddNewPart<ExtendedFilePropertiesPart>("rId3");
        GenerateExtendedFilePropertiesPart1Content(extendedFilePropertiesPart1);

        var workbookPart1 = document.AddWorkbookPart();
        GenerateWorkbookPart1Content(workbookPart1);

        var worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId3");
        GenerateWorksheetPartContent(worksheetPart1);

        var worksheetPart2 = workbookPart1.AddNewPart<WorksheetPart>("rId2");
        GenerateWorksheetPartContent(worksheetPart2);

        var worksheetPart3 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
        GenerateWorksheetPartContent(worksheetPart3);

        var workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId5");
        GenerateWorkbookStylesPart1Content(workbookStylesPart1);

        var themePart1 = workbookPart1.AddNewPart<ThemePart>("rId4");
        GenerateThemePart1Content(themePart1);

        SetPackageProperties(document);
    }
    
    /// <summary>
    /// Generates required content of extendedFilePropertiesPart1
    /// </summary>
    /// <param name="extendedFilePropertiesPart1"></param>
    private static void GenerateExtendedFilePropertiesPart1Content(ExtendedFilePropertiesPart extendedFilePropertiesPart1)
    {
        var properties1 = new Ap.Properties();
        properties1.AddNamespaceDeclaration("vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
        var totalTime1 = new Ap.TotalTime { Text = "0" };
        var application1 = new Ap.Application { Text = "Microsoft Excel" };
        var documentSecurity1 = new Ap.DocumentSecurity { Text = "0" };
        var scaleCrop1 = new Ap.ScaleCrop { Text = "false" };

        var headingPairs1 = new Ap.HeadingPairs();

        var vTVector1 = new Vt.VTVector { BaseType = Vt.VectorBaseValues.Variant, Size = 2U };

        var variant1 = new Vt.Variant();
        var vTlpstr1 = new Vt.VTLPSTR { Text = "Arbeitsblätter" };

        variant1.Append(vTlpstr1);

        var variant2 = new Vt.Variant();
        var vTInt321 = new Vt.VTInt32 { Text = "3" };

        variant2.Append(vTInt321);

        vTVector1.Append(variant1);
        vTVector1.Append(variant2);

        headingPairs1.Append(vTVector1);

        var titlesOfParts1 = new Ap.TitlesOfParts();

        var vTVector2 = new Vt.VTVector { BaseType = Vt.VectorBaseValues.Lpstr, Size = 3U };
        var vTlpstr2 = new Vt.VTLPSTR { Text = "Tabelle1" };
        var vTlpstr3 = new Vt.VTLPSTR { Text = "Tabelle2" };
        var vTlpstr4 = new Vt.VTLPSTR { Text = "Tabelle3" };

        vTVector2.Append(vTlpstr2);
        vTVector2.Append(vTlpstr3);
        vTVector2.Append(vTlpstr4);

        titlesOfParts1.Append(vTVector2);
        var company1 = new Ap.Company { Text = "Bodoconsult EDV-Dienstleistungen GmbH" };
        var linksUpToDate1 = new Ap.LinksUpToDate { Text = "false" };
        var sharedDocument1 = new Ap.SharedDocument { Text = "false" };
        var hyperlinksChanged1 = new Ap.HyperlinksChanged { Text = "false" };
        var applicationVersion1 = new Ap.ApplicationVersion { Text = "14.0300" };

        properties1.Append(totalTime1);
        properties1.Append(application1);
        properties1.Append(documentSecurity1);
        properties1.Append(scaleCrop1);
        properties1.Append(headingPairs1);
        properties1.Append(titlesOfParts1);
        properties1.Append(company1);
        properties1.Append(linksUpToDate1);
        properties1.Append(sharedDocument1);
        properties1.Append(hyperlinksChanged1);
        properties1.Append(applicationVersion1);

        extendedFilePropertiesPart1.Properties = properties1;
    }
    
    /// <summary>
    /// Generates content of workbookPart1
    /// </summary>
    /// <param name="workbookPart1">Current workbook part 1</param>
    private static void GenerateWorkbookPart1Content(WorkbookPart workbookPart1)
    {
        var workbook1 = new Workbook();

        workbook1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        var fileVersion1 = new FileVersion { ApplicationName = "xl", LastEdited = "5", LowestEdited = "5", BuildVersion = "9303" };
        var workbookProperties1 = new WorkbookProperties { DefaultThemeVersion = 124226U };

        var bookViews1 = new BookViews();
        var workbookView1 = new WorkbookView { XWindow = 120, YWindow = 45, WindowWidth = 23715U, WindowHeight = 10035U };

        bookViews1.Append(workbookView1);

        var sheets1 = new Sheets();
        var sheet1 = new Sheet { Name = "Tabelle1", SheetId = 1U, Id = "rId1" };
        var sheet2 = new Sheet { Name = "Tabelle2", SheetId = 2U, Id = "rId2" };
        var sheet3 = new Sheet { Name = "Tabelle3", SheetId = 3U, Id = "rId3" };

        sheets1.Append(sheet1);
        sheets1.Append(sheet2);
        sheets1.Append(sheet3);
        var calculationProperties1 = new CalculationProperties { CalculationId = 145621U };

        workbook1.Append(fileVersion1);
        workbook1.Append(workbookProperties1);
        workbook1.Append(bookViews1);
        workbook1.Append(sheets1);
        workbook1.Append(calculationProperties1);

        workbookPart1.Workbook = workbook1;
    }

    /// <summary>
    /// Generates content of worksheetPart1
    /// </summary>
    /// <param name="worksheetPart">Current worksheet part</param>
    private static void GenerateWorksheetPartContent(WorksheetPart worksheetPart)
    {
        var worksheet = new Worksheet { MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "x14ac" } };
        worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");


        //var sheetDimension1 = new SheetDimension { Reference = "A1" };

        //var sheetViews1 = new SheetViews();
        //var sheetView1 = new SheetView { WorkbookViewId = 0U,  };

        //sheetViews1.Append(sheetView1);
        //var sheetFormatProperties1 = new SheetFormatProperties { BaseColumnWidth = 10U, DefaultRowHeight = 15D, DyDescent = 0.25D };


        var sheetData1 = new SheetData();
        //var pageMargins1 = new PageMargins { Left = 0.7D, Right = 0.7D, Top = 0.78740157499999996D, Bottom = 0.78740157499999996D, Header = 0.3D, Footer = 0.3D };

        //var sheetProtection1 = new SheetProtection { Sheet = false, Objects = false, Scenarios = false, FormatCells = true, FormatColumns = true, FormatRows = true, InsertColumns = true, InsertRows = true, InsertHyperlinks = true, DeleteColumns = true, DeleteRows = true };


        //worksheet1.Append(sheetDimension1);
        //worksheet1.Append(sheetViews1);
        //worksheet1.Append(sheetFormatProperties1);
        worksheet.Append(sheetData1);
        worksheet.Append(GetPageMargins());
        //worksheet1.Append(sheetProtection1);

        worksheetPart.Worksheet = worksheet;
    }


    // Generates content of themePart1.
    private static void GenerateThemePart1Content(ThemePart themePart1)
    {
        var theme1 = new A.Theme { Name = "Larissa" };
        theme1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

        var themeElements1 = new A.ThemeElements();

        var colorScheme1 = new A.ColorScheme { Name = "Larissa" };

        var dark1Color1 = new A.Dark1Color();
        var systemColor1 = new A.SystemColor { Val = A.SystemColorValues.WindowText, LastColor = "000000" };

        dark1Color1.Append(systemColor1);

        var light1Color1 = new A.Light1Color();
        var systemColor2 = new A.SystemColor { Val = A.SystemColorValues.Window, LastColor = "FFFFFF" };

        light1Color1.Append(systemColor2);

        var dark2Color1 = new A.Dark2Color();
        var rgbColorModelHex1 = new A.RgbColorModelHex { Val = "1F497D" };

        dark2Color1.Append(rgbColorModelHex1);

        var light2Color1 = new A.Light2Color();
        var rgbColorModelHex2 = new A.RgbColorModelHex { Val = "EEECE1" };

        light2Color1.Append(rgbColorModelHex2);

        var accent1Color1 = new A.Accent1Color();
        var rgbColorModelHex3 = new A.RgbColorModelHex { Val = "4F81BD" };

        accent1Color1.Append(rgbColorModelHex3);

        var accent2Color1 = new A.Accent2Color();
        var rgbColorModelHex4 = new A.RgbColorModelHex { Val = "C0504D" };

        accent2Color1.Append(rgbColorModelHex4);

        var accent3Color1 = new A.Accent3Color();
        var rgbColorModelHex5 = new A.RgbColorModelHex { Val = "9BBB59" };

        accent3Color1.Append(rgbColorModelHex5);

        var accent4Color1 = new A.Accent4Color();
        var rgbColorModelHex6 = new A.RgbColorModelHex { Val = "8064A2" };

        accent4Color1.Append(rgbColorModelHex6);

        var accent5Color1 = new A.Accent5Color();
        var rgbColorModelHex7 = new A.RgbColorModelHex { Val = "4BACC6" };

        accent5Color1.Append(rgbColorModelHex7);

        var accent6Color1 = new A.Accent6Color();
        var rgbColorModelHex8 = new A.RgbColorModelHex { Val = "F79646" };

        accent6Color1.Append(rgbColorModelHex8);

        var hyperlink1 = new A.Hyperlink();
        var rgbColorModelHex9 = new A.RgbColorModelHex { Val = "0000FF" };

        hyperlink1.Append(rgbColorModelHex9);

        var followedHyperlinkColor1 = new A.FollowedHyperlinkColor();
        var rgbColorModelHex10 = new A.RgbColorModelHex { Val = "800080" };

        followedHyperlinkColor1.Append(rgbColorModelHex10);

        colorScheme1.Append(dark1Color1);
        colorScheme1.Append(light1Color1);
        colorScheme1.Append(dark2Color1);
        colorScheme1.Append(light2Color1);
        colorScheme1.Append(accent1Color1);
        colorScheme1.Append(accent2Color1);
        colorScheme1.Append(accent3Color1);
        colorScheme1.Append(accent4Color1);
        colorScheme1.Append(accent5Color1);
        colorScheme1.Append(accent6Color1);
        colorScheme1.Append(hyperlink1);
        colorScheme1.Append(followedHyperlinkColor1);

        var fontScheme2 = new A.FontScheme { Name = "Larissa" };

        var majorFont1 = new A.MajorFont();
        var latinFont1 = new A.LatinFont { Typeface = "Cambria" };
        var eastAsianFont1 = new A.EastAsianFont { Typeface = string.Empty };
        var complexScriptFont1 = new A.ComplexScriptFont { Typeface = string.Empty };
        var supplementalFont1 = new A.SupplementalFont { Script = "Jpan", Typeface = "ＭＳ Ｐゴシック" };
        var supplementalFont2 = new A.SupplementalFont { Script = "Hang", Typeface = "맑은 고딕" };
        var supplementalFont3 = new A.SupplementalFont { Script = "Hans", Typeface = "宋体" };
        var supplementalFont4 = new A.SupplementalFont { Script = "Hant", Typeface = "新細明體" };
        var supplementalFont5 = new A.SupplementalFont { Script = "Arab", Typeface = "Times New Roman" };
        var supplementalFont6 = new A.SupplementalFont { Script = "Hebr", Typeface = "Times New Roman" };
        var supplementalFont7 = new A.SupplementalFont { Script = "Thai", Typeface = "Tahoma" };
        var supplementalFont8 = new A.SupplementalFont { Script = "Ethi", Typeface = "Nyala" };
        var supplementalFont9 = new A.SupplementalFont { Script = "Beng", Typeface = "Vrinda" };
        var supplementalFont10 = new A.SupplementalFont { Script = "Gujr", Typeface = "Shruti" };
        var supplementalFont11 = new A.SupplementalFont { Script = "Khmr", Typeface = "MoolBoran" };
        var supplementalFont12 = new A.SupplementalFont { Script = "Knda", Typeface = "Tunga" };
        var supplementalFont13 = new A.SupplementalFont { Script = "Guru", Typeface = "Raavi" };
        var supplementalFont14 = new A.SupplementalFont { Script = "Cans", Typeface = "Euphemia" };
        var supplementalFont15 = new A.SupplementalFont { Script = "Cher", Typeface = "Plantagenet Cherokee" };
        var supplementalFont16 = new A.SupplementalFont { Script = "Yiii", Typeface = "Microsoft Yi Baiti" };
        var supplementalFont17 = new A.SupplementalFont { Script = "Tibt", Typeface = "Microsoft Himalaya" };
        var supplementalFont18 = new A.SupplementalFont { Script = "Thaa", Typeface = "MV Boli" };
        var supplementalFont19 = new A.SupplementalFont { Script = "Deva", Typeface = "Mangal" };
        var supplementalFont20 = new A.SupplementalFont { Script = "Telu", Typeface = "Gautami" };
        var supplementalFont21 = new A.SupplementalFont { Script = "Taml", Typeface = "Latha" };
        var supplementalFont22 = new A.SupplementalFont { Script = "Syrc", Typeface = "Estrangelo Edessa" };
        var supplementalFont23 = new A.SupplementalFont { Script = "Orya", Typeface = "Kalinga" };
        var supplementalFont24 = new A.SupplementalFont { Script = "Mlym", Typeface = "Kartika" };
        var supplementalFont25 = new A.SupplementalFont { Script = "Laoo", Typeface = "DokChampa" };
        var supplementalFont26 = new A.SupplementalFont { Script = "Sinh", Typeface = "Iskoola Pota" };
        var supplementalFont27 = new A.SupplementalFont { Script = "Mong", Typeface = "Mongolian Baiti" };
        var supplementalFont28 = new A.SupplementalFont { Script = "Viet", Typeface = "Times New Roman" };
        var supplementalFont29 = new A.SupplementalFont { Script = "Uigh", Typeface = "Microsoft Uighur" };
        var supplementalFont30 = new A.SupplementalFont { Script = "Geor", Typeface = "Sylfaen" };

        majorFont1.Append(latinFont1);
        majorFont1.Append(eastAsianFont1);
        majorFont1.Append(complexScriptFont1);
        majorFont1.Append(supplementalFont1);
        majorFont1.Append(supplementalFont2);
        majorFont1.Append(supplementalFont3);
        majorFont1.Append(supplementalFont4);
        majorFont1.Append(supplementalFont5);
        majorFont1.Append(supplementalFont6);
        majorFont1.Append(supplementalFont7);
        majorFont1.Append(supplementalFont8);
        majorFont1.Append(supplementalFont9);
        majorFont1.Append(supplementalFont10);
        majorFont1.Append(supplementalFont11);
        majorFont1.Append(supplementalFont12);
        majorFont1.Append(supplementalFont13);
        majorFont1.Append(supplementalFont14);
        majorFont1.Append(supplementalFont15);
        majorFont1.Append(supplementalFont16);
        majorFont1.Append(supplementalFont17);
        majorFont1.Append(supplementalFont18);
        majorFont1.Append(supplementalFont19);
        majorFont1.Append(supplementalFont20);
        majorFont1.Append(supplementalFont21);
        majorFont1.Append(supplementalFont22);
        majorFont1.Append(supplementalFont23);
        majorFont1.Append(supplementalFont24);
        majorFont1.Append(supplementalFont25);
        majorFont1.Append(supplementalFont26);
        majorFont1.Append(supplementalFont27);
        majorFont1.Append(supplementalFont28);
        majorFont1.Append(supplementalFont29);
        majorFont1.Append(supplementalFont30);

        var minorFont1 = new A.MinorFont();
        var latinFont2 = new A.LatinFont { Typeface = "Aptos" };
        var eastAsianFont2 = new A.EastAsianFont { Typeface = string.Empty };
        var complexScriptFont2 = new A.ComplexScriptFont { Typeface = string.Empty };
        var supplementalFont31 = new A.SupplementalFont { Script = "Jpan", Typeface = "ＭＳ Ｐゴシック" };
        var supplementalFont32 = new A.SupplementalFont { Script = "Hang", Typeface = "맑은 고딕" };
        var supplementalFont33 = new A.SupplementalFont { Script = "Hans", Typeface = "宋体" };
        var supplementalFont34 = new A.SupplementalFont { Script = "Hant", Typeface = "新細明體" };
        var supplementalFont35 = new A.SupplementalFont { Script = "Arab", Typeface = "Aptos" };
        var supplementalFont36 = new A.SupplementalFont { Script = "Hebr", Typeface = "Aptos" };
        var supplementalFont37 = new A.SupplementalFont { Script = "Thai", Typeface = "Tahoma" };
        var supplementalFont38 = new A.SupplementalFont { Script = "Ethi", Typeface = "Nyala" };
        var supplementalFont39 = new A.SupplementalFont { Script = "Beng", Typeface = "Vrinda" };
        var supplementalFont40 = new A.SupplementalFont { Script = "Gujr", Typeface = "Shruti" };
        var supplementalFont41 = new A.SupplementalFont { Script = "Khmr", Typeface = "DaunPenh" };
        var supplementalFont42 = new A.SupplementalFont { Script = "Knda", Typeface = "Tunga" };
        var supplementalFont43 = new A.SupplementalFont { Script = "Guru", Typeface = "Raavi" };
        var supplementalFont44 = new A.SupplementalFont { Script = "Cans", Typeface = "Euphemia" };
        var supplementalFont45 = new A.SupplementalFont { Script = "Cher", Typeface = "Plantagenet Cherokee" };
        var supplementalFont46 = new A.SupplementalFont { Script = "Yiii", Typeface = "Microsoft Yi Baiti" };
        var supplementalFont47 = new A.SupplementalFont { Script = "Tibt", Typeface = "Microsoft Himalaya" };
        var supplementalFont48 = new A.SupplementalFont { Script = "Thaa", Typeface = "MV Boli" };
        var supplementalFont49 = new A.SupplementalFont { Script = "Deva", Typeface = "Mangal" };
        var supplementalFont50 = new A.SupplementalFont { Script = "Telu", Typeface = "Gautami" };
        var supplementalFont51 = new A.SupplementalFont { Script = "Taml", Typeface = "Latha" };
        var supplementalFont52 = new A.SupplementalFont { Script = "Syrc", Typeface = "Estrangelo Edessa" };
        var supplementalFont53 = new A.SupplementalFont { Script = "Orya", Typeface = "Kalinga" };
        var supplementalFont54 = new A.SupplementalFont { Script = "Mlym", Typeface = "Kartika" };
        var supplementalFont55 = new A.SupplementalFont { Script = "Laoo", Typeface = "DokChampa" };
        var supplementalFont56 = new A.SupplementalFont { Script = "Sinh", Typeface = "Iskoola Pota" };
        var supplementalFont57 = new A.SupplementalFont { Script = "Mong", Typeface = "Mongolian Baiti" };
        var supplementalFont58 = new A.SupplementalFont { Script = "Viet", Typeface = "Aptos" };
        var supplementalFont59 = new A.SupplementalFont { Script = "Uigh", Typeface = "Microsoft Uighur" };
        var supplementalFont60 = new A.SupplementalFont { Script = "Geor", Typeface = "Sylfaen" };

        minorFont1.Append(latinFont2);
        minorFont1.Append(eastAsianFont2);
        minorFont1.Append(complexScriptFont2);
        minorFont1.Append(supplementalFont31);
        minorFont1.Append(supplementalFont32);
        minorFont1.Append(supplementalFont33);
        minorFont1.Append(supplementalFont34);
        minorFont1.Append(supplementalFont35);
        minorFont1.Append(supplementalFont36);
        minorFont1.Append(supplementalFont37);
        minorFont1.Append(supplementalFont38);
        minorFont1.Append(supplementalFont39);
        minorFont1.Append(supplementalFont40);
        minorFont1.Append(supplementalFont41);
        minorFont1.Append(supplementalFont42);
        minorFont1.Append(supplementalFont43);
        minorFont1.Append(supplementalFont44);
        minorFont1.Append(supplementalFont45);
        minorFont1.Append(supplementalFont46);
        minorFont1.Append(supplementalFont47);
        minorFont1.Append(supplementalFont48);
        minorFont1.Append(supplementalFont49);
        minorFont1.Append(supplementalFont50);
        minorFont1.Append(supplementalFont51);
        minorFont1.Append(supplementalFont52);
        minorFont1.Append(supplementalFont53);
        minorFont1.Append(supplementalFont54);
        minorFont1.Append(supplementalFont55);
        minorFont1.Append(supplementalFont56);
        minorFont1.Append(supplementalFont57);
        minorFont1.Append(supplementalFont58);
        minorFont1.Append(supplementalFont59);
        minorFont1.Append(supplementalFont60);

        fontScheme2.Append(majorFont1);
        fontScheme2.Append(minorFont1);

        var formatScheme1 = new A.FormatScheme { Name = "Larissa" };

        var fillStyleList1 = new A.FillStyleList();

        var solidFill1 = new A.SolidFill();
        var schemeColor1 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };

        solidFill1.Append(schemeColor1);

        var gradientFill1 = new A.GradientFill { RotateWithShape = true };

        var gradientStopList1 = new A.GradientStopList();

        var gradientStop1 = new A.GradientStop { Position = 0 };

        var schemeColor2 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var tint1 = new A.Tint { Val = 50000 };
        var saturationModulation1 = new A.SaturationModulation { Val = 300000 };

        schemeColor2.Append(tint1);
        schemeColor2.Append(saturationModulation1);

        gradientStop1.Append(schemeColor2);

        var gradientStop2 = new A.GradientStop { Position = 35000 };

        var schemeColor3 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var tint2 = new A.Tint { Val = 37000 };
        var saturationModulation2 = new A.SaturationModulation { Val = 300000 };

        schemeColor3.Append(tint2);
        schemeColor3.Append(saturationModulation2);

        gradientStop2.Append(schemeColor3);

        var gradientStop3 = new A.GradientStop { Position = 100000 };

        var schemeColor4 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var tint3 = new A.Tint { Val = 15000 };
        var saturationModulation3 = new A.SaturationModulation { Val = 350000 };

        schemeColor4.Append(tint3);
        schemeColor4.Append(saturationModulation3);

        gradientStop3.Append(schemeColor4);

        gradientStopList1.Append(gradientStop1);
        gradientStopList1.Append(gradientStop2);
        gradientStopList1.Append(gradientStop3);
        var linearGradientFill1 = new A.LinearGradientFill { Angle = 16200000, Scaled = true };

        gradientFill1.Append(gradientStopList1);
        gradientFill1.Append(linearGradientFill1);

        var gradientFill2 = new A.GradientFill { RotateWithShape = true };

        var gradientStopList2 = new A.GradientStopList();

        var gradientStop4 = new A.GradientStop { Position = 0 };

        var schemeColor5 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var shade1 = new A.Shade { Val = 51000 };
        var saturationModulation4 = new A.SaturationModulation { Val = 130000 };

        schemeColor5.Append(shade1);
        schemeColor5.Append(saturationModulation4);

        gradientStop4.Append(schemeColor5);

        var gradientStop5 = new A.GradientStop { Position = 80000 };

        var schemeColor6 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var shade2 = new A.Shade { Val = 93000 };
        var saturationModulation5 = new A.SaturationModulation { Val = 130000 };

        schemeColor6.Append(shade2);
        schemeColor6.Append(saturationModulation5);

        gradientStop5.Append(schemeColor6);

        var gradientStop6 = new A.GradientStop { Position = 100000 };

        var schemeColor7 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var shade3 = new A.Shade { Val = 94000 };
        var saturationModulation6 = new A.SaturationModulation { Val = 135000 };

        schemeColor7.Append(shade3);
        schemeColor7.Append(saturationModulation6);

        gradientStop6.Append(schemeColor7);

        gradientStopList2.Append(gradientStop4);
        gradientStopList2.Append(gradientStop5);
        gradientStopList2.Append(gradientStop6);
        var linearGradientFill2 = new A.LinearGradientFill { Angle = 16200000, Scaled = false };

        gradientFill2.Append(gradientStopList2);
        gradientFill2.Append(linearGradientFill2);

        fillStyleList1.Append(solidFill1);
        fillStyleList1.Append(gradientFill1);
        fillStyleList1.Append(gradientFill2);

        var lineStyleList1 = new A.LineStyleList();

        var outline1 = new A.Outline { Width = 9525, CapType = A.LineCapValues.Flat, CompoundLineType = A.CompoundLineValues.Single, Alignment = A.PenAlignmentValues.Center };

        var solidFill2 = new A.SolidFill();

        var schemeColor8 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var shade4 = new A.Shade { Val = 95000 };
        var saturationModulation7 = new A.SaturationModulation { Val = 105000 };

        schemeColor8.Append(shade4);
        schemeColor8.Append(saturationModulation7);

        solidFill2.Append(schemeColor8);
        var presetDash1 = new A.PresetDash { Val = A.PresetLineDashValues.Solid };

        outline1.Append(solidFill2);
        outline1.Append(presetDash1);

        var outline2 = new A.Outline { Width = 25400, CapType = A.LineCapValues.Flat, CompoundLineType = A.CompoundLineValues.Single, Alignment = A.PenAlignmentValues.Center };

        var solidFill3 = new A.SolidFill();
        var schemeColor9 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };

        solidFill3.Append(schemeColor9);
        var presetDash2 = new A.PresetDash { Val = A.PresetLineDashValues.Solid };

        outline2.Append(solidFill3);
        outline2.Append(presetDash2);

        var outline3 = new A.Outline { Width = 38100, CapType = A.LineCapValues.Flat, CompoundLineType = A.CompoundLineValues.Single, Alignment = A.PenAlignmentValues.Center };

        var solidFill4 = new A.SolidFill();
        var schemeColor10 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };

        solidFill4.Append(schemeColor10);
        var presetDash3 = new A.PresetDash { Val = A.PresetLineDashValues.Solid };

        outline3.Append(solidFill4);
        outline3.Append(presetDash3);

        lineStyleList1.Append(outline1);
        lineStyleList1.Append(outline2);
        lineStyleList1.Append(outline3);

        var effectStyleList1 = new A.EffectStyleList();

        var effectStyle1 = new A.EffectStyle();

        var effectList1 = new A.EffectList();

        var outerShadow1 = new A.OuterShadow { BlurRadius = 40000L, Distance = 20000L, Direction = 5400000, RotateWithShape = false };

        var rgbColorModelHex11 = new A.RgbColorModelHex { Val = "000000" };
        var alpha1 = new A.Alpha { Val = 38000 };

        rgbColorModelHex11.Append(alpha1);

        outerShadow1.Append(rgbColorModelHex11);

        effectList1.Append(outerShadow1);

        effectStyle1.Append(effectList1);

        var effectStyle2 = new A.EffectStyle();

        var effectList2 = new A.EffectList();

        var outerShadow2 = new A.OuterShadow { BlurRadius = 40000L, Distance = 23000L, Direction = 5400000, RotateWithShape = false };

        var rgbColorModelHex12 = new A.RgbColorModelHex { Val = "000000" };
        var alpha2 = new A.Alpha { Val = 35000 };

        rgbColorModelHex12.Append(alpha2);

        outerShadow2.Append(rgbColorModelHex12);

        effectList2.Append(outerShadow2);

        effectStyle2.Append(effectList2);

        var effectStyle3 = new A.EffectStyle();

        var effectList3 = new A.EffectList();

        var outerShadow3 = new A.OuterShadow { BlurRadius = 40000L, Distance = 23000L, Direction = 5400000, RotateWithShape = false };

        var rgbColorModelHex13 = new A.RgbColorModelHex { Val = "000000" };
        var alpha3 = new A.Alpha { Val = 35000 };

        rgbColorModelHex13.Append(alpha3);

        outerShadow3.Append(rgbColorModelHex13);

        effectList3.Append(outerShadow3);

        var scene3DType1 = new A.Scene3DType();

        var camera1 = new A.Camera { Preset = A.PresetCameraValues.OrthographicFront };
        var rotation1 = new A.Rotation { Latitude = 0, Longitude = 0, Revolution = 0 };

        camera1.Append(rotation1);

        var lightRig1 = new A.LightRig { Rig = A.LightRigValues.ThreePoints, Direction = A.LightRigDirectionValues.Top };
        var rotation2 = new A.Rotation { Latitude = 0, Longitude = 0, Revolution = 1200000 };

        lightRig1.Append(rotation2);

        scene3DType1.Append(camera1);
        scene3DType1.Append(lightRig1);

        var shape3DType1 = new A.Shape3DType();
        var bevelTop1 = new A.BevelTop { Width = 63500L, Height = 25400L };

        shape3DType1.Append(bevelTop1);

        effectStyle3.Append(effectList3);
        effectStyle3.Append(scene3DType1);
        effectStyle3.Append(shape3DType1);

        effectStyleList1.Append(effectStyle1);
        effectStyleList1.Append(effectStyle2);
        effectStyleList1.Append(effectStyle3);

        var backgroundFillStyleList1 = new A.BackgroundFillStyleList();

        var solidFill5 = new A.SolidFill();
        var schemeColor11 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };

        solidFill5.Append(schemeColor11);

        var gradientFill3 = new A.GradientFill { RotateWithShape = true };

        var gradientStopList3 = new A.GradientStopList();

        var gradientStop7 = new A.GradientStop { Position = 0 };

        var schemeColor12 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var tint4 = new A.Tint { Val = 40000 };
        var saturationModulation8 = new A.SaturationModulation { Val = 350000 };

        schemeColor12.Append(tint4);
        schemeColor12.Append(saturationModulation8);

        gradientStop7.Append(schemeColor12);

        var gradientStop8 = new A.GradientStop { Position = 40000 };

        var schemeColor13 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var tint5 = new A.Tint { Val = 45000 };
        var shade5 = new A.Shade { Val = 99000 };
        var saturationModulation9 = new A.SaturationModulation { Val = 350000 };

        schemeColor13.Append(tint5);
        schemeColor13.Append(shade5);
        schemeColor13.Append(saturationModulation9);

        gradientStop8.Append(schemeColor13);

        var gradientStop9 = new A.GradientStop { Position = 100000 };

        var schemeColor14 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var shade6 = new A.Shade { Val = 20000 };
        var saturationModulation10 = new A.SaturationModulation { Val = 255000 };

        schemeColor14.Append(shade6);
        schemeColor14.Append(saturationModulation10);

        gradientStop9.Append(schemeColor14);

        gradientStopList3.Append(gradientStop7);
        gradientStopList3.Append(gradientStop8);
        gradientStopList3.Append(gradientStop9);

        var pathGradientFill1 = new A.PathGradientFill { Path = A.PathShadeValues.Circle };
        var fillToRectangle1 = new A.FillToRectangle { Left = 50000, Top = -80000, Right = 50000, Bottom = 180000 };

        pathGradientFill1.Append(fillToRectangle1);

        gradientFill3.Append(gradientStopList3);
        gradientFill3.Append(pathGradientFill1);

        var gradientFill4 = new A.GradientFill { RotateWithShape = true };

        var gradientStopList4 = new A.GradientStopList();

        var gradientStop10 = new A.GradientStop { Position = 0 };

        var schemeColor15 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var tint6 = new A.Tint { Val = 80000 };
        var saturationModulation11 = new A.SaturationModulation { Val = 300000 };

        schemeColor15.Append(tint6);
        schemeColor15.Append(saturationModulation11);

        gradientStop10.Append(schemeColor15);

        var gradientStop11 = new A.GradientStop { Position = 100000 };

        var schemeColor16 = new A.SchemeColor { Val = A.SchemeColorValues.PhColor };
        var shade7 = new A.Shade { Val = 30000 };
        var saturationModulation12 = new A.SaturationModulation { Val = 200000 };

        schemeColor16.Append(shade7);
        schemeColor16.Append(saturationModulation12);

        gradientStop11.Append(schemeColor16);

        gradientStopList4.Append(gradientStop10);
        gradientStopList4.Append(gradientStop11);

        var pathGradientFill2 = new A.PathGradientFill { Path = A.PathShadeValues.Circle };
        var fillToRectangle2 = new A.FillToRectangle { Left = 50000, Top = 50000, Right = 50000, Bottom = 50000 };

        pathGradientFill2.Append(fillToRectangle2);

        gradientFill4.Append(gradientStopList4);
        gradientFill4.Append(pathGradientFill2);

        backgroundFillStyleList1.Append(solidFill5);
        backgroundFillStyleList1.Append(gradientFill3);
        backgroundFillStyleList1.Append(gradientFill4);

        formatScheme1.Append(fillStyleList1);
        formatScheme1.Append(lineStyleList1);
        formatScheme1.Append(effectStyleList1);
        formatScheme1.Append(backgroundFillStyleList1);

        themeElements1.Append(colorScheme1);
        themeElements1.Append(fontScheme2);
        themeElements1.Append(formatScheme1);
        var objectDefaults1 = new A.ObjectDefaults();
        var extraColorSchemeList1 = new A.ExtraColorSchemeList();

        theme1.Append(themeElements1);
        theme1.Append(objectDefaults1);
        theme1.Append(extraColorSchemeList1);

        themePart1.Theme = theme1;
    }

    private static void SetPackageProperties(OpenXmlPackage document)
    {
        var domain = Environment.UserDomainName;

        document.PackageProperties.Creator = string.IsNullOrEmpty(domain) 
            ? Environment.UserName 
            : $"{domain}\\{Environment.UserName}";

        document.PackageProperties.Created = DateTime.Now;
        document.PackageProperties.Modified = null;
        document.PackageProperties.LastModifiedBy = null;
    }


}