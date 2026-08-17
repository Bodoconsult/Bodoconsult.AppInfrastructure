// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Color = DocumentFormat.OpenXml.Spreadsheet.Color;
using DifferentialFormats = DocumentFormat.OpenXml.Spreadsheet.DifferentialFormats;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;

namespace Bodoconsult.Office;

public partial class XlsxDocument
{

    // Generates content of workbookStylesPart1.
    private void GenerateWorkbookStylesPart1Content(WorkbookStylesPart workbookStylesPart1)
    {
        // ReSharper disable PossiblyMistakenUseOfParamsMethod
        var stylesheet1 = new Stylesheet { MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "x14ac" } };
        stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

        var numberingFormats = new NumberingFormats();

        // Datum
        var numberingFormat = new NumberingFormat
        {
            NumberFormatId = 14,
            FormatCode = StringValue.FromString("dd.mm.yyyy")
        };
        numberingFormats.Append(numberingFormat);


        // Double
        var numberingFormatDouble = new NumberingFormat
        {
            NumberFormatId = 4,
            FormatCode = StringValue.FromString(NumberFormatDouble)
        };
        numberingFormats.Append(numberingFormatDouble);

        var fonts = new Fonts { Count = 3U, KnownFonts = true };
        fonts.Append(Font0());
        fonts.Append(Font1());
        fonts.Append(Font2());
        fonts.Append(Font3());

        var fills1 = new Fills { Count = 2U };
        fills1.Append(Fill0());
        fills1.Append(Fill1());
        fills1.Append(Fill2());
        fills1.Append(Fill3());

        var borders1 = new Borders { Count = 2U };
        borders1.Append(Border0());
        borders1.Append(Border1());

        var cellStyleFormats1 = new CellStyleFormats { Count = 1U };
        cellStyleFormats1.Append(CellFormat0());

        var cellFormats1 = new CellFormats { Count = 11U };
        cellFormats1.Append(CellFormat0());
        cellFormats1.Append(CellFormat1());
        cellFormats1.Append(CellFormat2());
        cellFormats1.Append(CellFormat3());
        cellFormats1.Append(CellFormat4());
        cellFormats1.Append(CellFormat5());
        cellFormats1.Append(CellFormat6());
        cellFormats1.Append(CellFormat7());
        cellFormats1.Append(CellFormat8());
        cellFormats1.Append(CellFormat9());
        cellFormats1.Append(CellFormat10());
        cellFormats1.Append(CellFormat11());

        var cellStyles1 = new CellStyles { Count = 1U };
        var cellStyle1 = new CellStyle
        {
            Name = "Standard",
            FormatId = 0U,
            BuiltinId = 0U,
        };

        cellStyles1.Append(cellStyle1);




        var differentialFormats1 = new DifferentialFormats { Count = 0U };
        var tableStyles1 = new TableStyles
        {
            Count = 0U,
            DefaultTableStyle = "TableStyleMedium2",
            DefaultPivotStyle = "PivotStyleLight16"
        };

        var stylesheetExtensionList1 = new StylesheetExtensionList();

        var stylesheetExtension1 = new StylesheetExtension
        {
            Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}"
        };
        stylesheetExtension1.AddNamespaceDeclaration("x14","http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
        var slicerStyles1 = new X14.SlicerStyles { DefaultSlicerStyle = "SlicerStyleLight1" };

        stylesheetExtension1.Append(slicerStyles1);

        stylesheetExtensionList1.Append(stylesheetExtension1);

        stylesheet1.Append(numberingFormats);
        stylesheet1.Append(fonts);
        stylesheet1.Append(fills1);
        stylesheet1.Append(borders1);
        stylesheet1.Append(cellStyleFormats1);
        stylesheet1.Append(cellFormats1);
        stylesheet1.Append(cellStyles1);
        stylesheet1.Append(differentialFormats1);
        stylesheet1.Append(tableStyles1);
        stylesheet1.Append(stylesheetExtensionList1);

        workbookStylesPart1.Stylesheet = stylesheet1;

        // ReSharper restore PossiblyMistakenUseOfParamsMethod
    }

    private static Fill Fill0()
    {
        // ReSharper disable PossiblyMistakenUseOfParamsMethod

        var fill1 = new Fill();
        var patternFill1 = new PatternFill {PatternType = PatternValues.None};
        fill1.Append(patternFill1);

        // ReSharper restore PossiblyMistakenUseOfParamsMethod
        return fill1;
    }


    private static Fill Fill1()
    {
        // ReSharper disable PossiblyMistakenUseOfParamsMethod

        var fill1 = new Fill();
        var patternFill1 = new PatternFill { PatternType = PatternValues.Gray125 };
        fill1.Append(patternFill1);

        // ReSharper restore PossiblyMistakenUseOfParamsMethod
        return fill1;
    }

    private static Fill Fill2()
    {
        // ReSharper disable PossiblyMistakenUseOfParamsMethod
        // #B8CCE4
        var fill1 = new Fill();
        var patternFill1 = new PatternFill { PatternType = PatternValues.Solid };
        var foregroundColor1 = new ForegroundColor { Rgb = "DCE6F1" };
        var backgroundColor1 = new BackgroundColor { Indexed = 64U };

        patternFill1.Append(foregroundColor1);
        patternFill1.Append(backgroundColor1);

        fill1.Append(patternFill1);
        return fill1;


        // ReSharper restore PossiblyMistakenUseOfParamsMethod
    }

    private static Fill Fill3()
    {
        // ReSharper disable PossiblyMistakenUseOfParamsMethod
        // 
        var fill1 = new Fill();
        var patternFill1 = new PatternFill { PatternType = PatternValues.Solid };
        var foregroundColor1 = new ForegroundColor { Rgb = "B8CCE4" };
        var backgroundColor1 = new BackgroundColor { Indexed = 64U };

        patternFill1.Append(foregroundColor1);
        patternFill1.Append(backgroundColor1);

        fill1.Append(patternFill1);
        return fill1;


        // ReSharper restore PossiblyMistakenUseOfParamsMethod
    }

    private static Border Border0()
    {
        // ReSharper disable PossiblyMistakenUseOfParamsMethod

        var border1 = new Border();
        var leftBorder1 = new LeftBorder();
        var rightBorder1 = new RightBorder();
        var topBorder1 = new TopBorder();
        var bottomBorder1 = new BottomBorder();
        var diagonalBorder1 = new DiagonalBorder();

        border1.Append(leftBorder1);
        border1.Append(rightBorder1);
        border1.Append(topBorder1);
        border1.Append(bottomBorder1);
        border1.Append(diagonalBorder1);
        return border1;

        // ReSharper restore PossiblyMistakenUseOfParamsMethod
    }


    private static Border Border1()
    {
        // ReSharper disable PossiblyMistakenUseOfParamsMethod

        var border1 = new Border();
            

        var leftBorder1 = new LeftBorder { Style = BorderStyleValues.Thin };
        var color1 = new Color { Indexed = 64U };
        leftBorder1.Append(color1);

        var rightBorder1 = new RightBorder { Style = BorderStyleValues.Thin };
        color1 = new Color { Indexed = 64U };
        rightBorder1.Append(color1);

        var topBorder1 = new TopBorder { Style = BorderStyleValues.Thin };
        color1 = new Color { Indexed = 64U };
        topBorder1.Append(color1);

        var bottomBorder1 = new BottomBorder { Style = BorderStyleValues.Thin };
        color1 = new Color { Indexed = 64U };
        bottomBorder1.Append(color1);

        var diagonalBorder1 = new DiagonalBorder();

        border1.Append(leftBorder1);
        border1.Append(rightBorder1);
        border1.Append(topBorder1);
        border1.Append(bottomBorder1);
        border1.Append(diagonalBorder1);
        return border1;

        // ReSharper restore PossiblyMistakenUseOfParamsMethod
    }


    private static CellFormat CellFormat0()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 0U,
            FontId = 0U,
            FillId = 0U,
            BorderId = 0U
        };
        return cellFormat1;
    }

    private static CellFormat CellFormat1()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 0U,
            FontId = 1U,
            FillId = 0U,
            BorderId = 0U
        };
        return cellFormat1;
    }

    private static CellFormat CellFormat2()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 0U,
            FontId = 2U,
            FillId = 3U,
            BorderId = 1U
        };
        return cellFormat1;
    }


    private static CellFormat CellFormat3()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 0U,
            FontId = 0U,
            FillId = 0U,
            BorderId = 1U
        };
        return cellFormat1;
    }


    private static CellFormat CellFormat4()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 14U,
            FontId = 0U,
            FillId = 0U,
            BorderId = 1U
        };
        return cellFormat1;
    }

    private static CellFormat CellFormat5()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 4U,
            FontId = 0U,
            FillId = 0U,
            BorderId = 1U
        };
        return cellFormat1;
    }

    private static CellFormat CellFormat6()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 3U,
            FontId = 0U,
            FillId = 0U,
            BorderId = 1U
        };
        return cellFormat1;
    }

    private static CellFormat CellFormat7()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 0U,
            FontId = 0U,
            FillId = 2U,
            BorderId = 1U
        };
        return cellFormat1;
    }

    private static CellFormat CellFormat8()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 14U,
            FontId = 0U,
            FillId = 2U,
            BorderId = 1U
        };
        return cellFormat1;
    }

    private static CellFormat CellFormat9()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 4U,
            FontId = 0U,
            FillId = 2U,
            BorderId = 1U
        };
        return cellFormat1;
    }

    private static CellFormat CellFormat10()
    {
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = 3U,
            FontId = 0U,
            FillId = 2U,
            BorderId = 1U
        };
        return cellFormat1;
    }

    private static CellFormat CellFormat11()
    {
        var cellFormat = new CellFormat
        {
            NumberFormatId = 3U,
            FontId = 0U,
            FillId = 0U,
            BorderId = 0U
        };
        return cellFormat;
    }


    private static Font Font0()
    {


        var font = new Font();
        var fontSize1 = new FontSize { Val = 11D };
        var color1 = new Color { Theme = 1U };
        var fontName1 = new FontName { Val = "Aptos" };
        var fontFamilyNumbering1 = new FontFamilyNumbering { Val = 2 };
        var fontScheme1 = new FontScheme { Val = FontSchemeValues.Minor };

        // ReSharper disable PossiblyMistakenUseOfParamsMethod
        font.Append(fontSize1);

        font.Append(color1);
        font.Append(fontName1);
        font.Append(fontFamilyNumbering1);
        font.Append(fontScheme1);

        // ReSharper restore PossiblyMistakenUseOfParamsMethod
        return font;
    }


    private static Font Font1()
    {


        var font = new Font();
        var fontSize1 = new FontSize { Val = 15D };
        var color1 = new Color { Theme = 1U };
        var fontName1 = new FontName { Val = "Aptos" };
        var fontFamilyNumbering1 = new FontFamilyNumbering { Val = 2 };
        var fontScheme1 = new FontScheme { Val = FontSchemeValues.Minor };
        var bold1 = new Bold();

        // ReSharper disable PossiblyMistakenUseOfParamsMethod
            
        font.Append(bold1);
        font.Append(fontSize1);
        font.Append(color1);
        font.Append(fontName1);
        font.Append(fontFamilyNumbering1);
        font.Append(fontScheme1);

        // ReSharper restore PossiblyMistakenUseOfParamsMethod
        return font;
    }

    private static Font Font2()
    {


        var font = new Font();
        var fontSize1 = new FontSize { Val = 11D };
        var color1 = new Color { Theme = 1U };
        var fontName1 = new FontName { Val = "Aptos" };
        var fontFamilyNumbering1 = new FontFamilyNumbering { Val = 2 };
        var fontScheme1 = new FontScheme { Val = FontSchemeValues.Minor };
        var bold1 = new Bold();


        // ReSharper disable PossiblyMistakenUseOfParamsMethod
        font.Append(bold1);
        font.Append(fontSize1);
        font.Append(color1);
        font.Append(fontName1);
        font.Append(fontFamilyNumbering1);
        font.Append(fontScheme1);

        // ReSharper restore PossiblyMistakenUseOfParamsMethod
        return font;
    }

    private static Font Font3()
    {


        var font = new Font();
        var fontSize1 = new FontSize { Val = 13D };
        var color1 = new Color { Theme = 1U };
        var fontName1 = new FontName { Val = "Aptos" };
        var fontFamilyNumbering1 = new FontFamilyNumbering { Val = 2 };
        var fontScheme1 = new FontScheme { Val = FontSchemeValues.Minor };
        var bold1 = new Bold();


        // ReSharper disable PossiblyMistakenUseOfParamsMethod
        font.Append(bold1);
        font.Append(fontSize1);
        font.Append(color1);
        font.Append(fontName1);
        font.Append(fontFamilyNumbering1);
        font.Append(fontScheme1);

        // ReSharper restore PossiblyMistakenUseOfParamsMethod
        return font;
    }

}