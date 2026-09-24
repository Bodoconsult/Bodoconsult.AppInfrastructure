// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.Runtime.Versioning;
using Bodoconsult.Office.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Office.Test;

[TestFixture]
[NonParallelizable]
[SingleThreaded]
[SupportedOSPlatform("windows")]
public class XlsxLateBindingBuilderTests
{
    [Test]
    public void FillDataTable_ValidDataTable_FileCreated()
    {
        // Assert
        var dt = TestHelper.GetDataTable("LineChart.xml");

        var excel = new XlsxLateBindingBuilder();
        excel.Status += ShowStatus;
        excel.NewWorkbook();
        //if (e.ErrorCode != 0) return;

        //excel.NewSheet("Daten");
        excel.SelectSheetFirst("TransactionData");
        excel.Header("Test");
        excel.NumberFormat = "#,##0.000000";

        // Act
        excel.FillDataTable(dt, 4, 1);

        excel.NewSheet("Daten2");
        excel.Header("Test2");
        excel.NumberFormat = "#,##0.00";
        excel.FillDataTable(dt, 4, 1);

        excel.Quit();

        // Assert
    }

    //private static void ShowError(Exception ex, string message)
    //{
    //    var s = $"Error:{ex.Message}:{message}";
    //    Debug.Print(s);
    //}

    private static void ShowStatus(string message)
    {
        Debug.Print(message);
    }
}