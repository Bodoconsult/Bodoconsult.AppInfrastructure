// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using Bodoconsult.Office.Interfaces;
using Bodoconsult.Office.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Office.Test;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
public class XlsxBuilderTests
{

    [Test]
    public void FillDataTable_ValidDataTable_ExcelFileCreated()
    {

        var path = Path.Combine(FileHelper.TempPath, "openxml.xlsx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var dt = TestHelper.GetDataTable("LineChart.xml");

        var oe = new XlsxBuilder();
        oe.Status += ShowStatus;
        oe.Error += ShowError;
        oe.NumberFormatDouble = "#,##0.000000";
        oe.NewWorkbook(path);

        //oe.SelectSheet("Tabelle1");


        oe.NewSheet("Daten");

        //oe.SelectSheetFirst("Daten");
        //oe.SelectSheet(1);
        ////oe.SelectRange("A1");
        oe.SelectRange(1, 1);
        oe.Style = SpreadsheetStyles.Header;
        oe.SetValue("Hallo Welt1");
        oe.FillDataTable(dt, 4, 1);

        oe.NewSheet("Daten2");

        //oe.SelectSheetFirst("Daten");
        //oe.SelectSheet(1);
        ////oe.SelectRange("A1");
        oe.SelectRange(1, 1);
        oe.Style = SpreadsheetStyles.Header;
        oe.SetValue("Hallo Welt2");
        oe.FillDataTable(dt, 4, 1);

        oe.Quit();

        FileHelper.StartExcel(path);
    }


    [Test]
    public void SelectSheet_ValidDataTable_ExcelFileCreatedOnSelectedSheet()
    {

        var path = Path.Combine(FileHelper.TempPath, "openxml.xlsx");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var dt = TestHelper.GetDataTable("LineChart.xml");

        var oe = new XlsxBuilder();
        oe.Status += ShowStatus;
        oe.Error += ShowError;
        oe.NumberFormatDouble = "#,##0.000000";
        oe.NewWorkbook(path);

        //oe.NewSheet("Daten");
        oe.SelectSheet("Tabelle1");

        ////oe.SelectSheetFirst("Daten");
        ////oe.SelectSheet(1);
        //////oe.SelectRange("A1");
        oe.SelectRange(1, 1);
        oe.Style = SpreadsheetStyles.Header;
        oe.SetValue("Hallo Welt1");
        oe.FillDataTable(dt, 4, 1);

        //oe.NewSheet("Daten2");

        //oe.SelectSheetFirst("Daten");
        //oe.SelectSheet(1);
        ////oe.SelectRange("A1");
        oe.SelectRange(1, 1);
        oe.Style = SpreadsheetStyles.Header;
        oe.SetValue("Hallo Welt2");
        oe.FillDataTable(dt, 4, 1);

        oe.Quit();

        FileHelper.StartExcel(path);
    }


    [Test]
    public void FillDataArray_ValidArray_ExcelFileCreated()
    {

        var path = Path.Combine(FileHelper.TempPath, "openxml1.xlsx");

        var data = new double[2, 2];

        data[0, 0] = 1.5;
        data[0, 1] = 2.5;
        data[1, 1] = 3.5;
        data[1, 0] = 4.5;


        var header = new[] {"Column1", "Column2"};

        var oe = new XlsxBuilder();
        oe.Status += ShowStatus;
        oe.Error += ShowError;
        oe.NumberFormatDouble = "#,##0.000000";
        oe.NewWorkbook(path);
        //oe.SelectSheet("Tabelle1");


        oe.NewSheet("Daten1");

        //oe.SelectSheetFirst("Daten");
        //oe.SelectSheet(1);
        ////oe.SelectRange("A1");
        oe.SelectRange(1, 1);
        oe.Style = SpreadsheetStyles.Header;
        oe.SetValue("Hallo Welt1");
        oe.FillDataArray(data, header, 4, 1);

        oe.NewSheet("Daten2");

        //oe.SelectSheetFirst("Daten");
        //oe.SelectSheet(1);
        ////oe.SelectRange("A1");
        oe.SelectRange(1, 1);
        oe.Style = SpreadsheetStyles.Header;
        oe.SetValue("Hallo Welt2");
        oe.FillDataArray(data, header,  4, 1);

        oe.Quit();

        FileHelper.StartExcel(path);
    }

    private static void ShowError(Exception ex, string message)
    {
        var s = $"Error:{ex.Message}:{message}";
        Debug.Print(s);
    }

    private static void ShowStatus(string message)
    {
        Debug.Print(message);
    }
}