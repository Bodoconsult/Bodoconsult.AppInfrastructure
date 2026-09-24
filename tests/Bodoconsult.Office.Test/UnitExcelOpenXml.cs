using System;
using System.Diagnostics;
using System.IO;
using Bodoconsult.Core.Database;
using Bodoconsult.Core.Office.Tests.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Core.Office.Tests
{
    [TestFixture]
    public class UnitTestExcelOpenXml
    {

        [Test]
        public void TestFillDataTable()
        {

            string path = System.IO.Path.Combine(FileHelper.TempPath, "openxml.xlsx");

            if (File.Exists(path)) File.Delete(path);

            var db = SqlClientConnManager.GetConnManager("Data Source=.\\SQLEXPRESS;Initial Catalog=MediaDb;Integrated Security=True");
            var dt = db.GetDataTable("select top 1000 * from settings");

            var oe = new ExcelOpenXml();
            oe.Status += ExcelStatus;
            oe.Error += ExcelError;
            oe.NumberFormatDouble = "#,##0.000000";
            oe.NewWorkbook(path);

            //oe.SelectSheet("Tabelle1");


            oe.NewSheet("Daten");

            //oe.SelectSheetFirst("Daten");
            //oe.SelectSheet(1);
            ////oe.SelectRange("A1");
            oe.SelectRange(1, 1);
            oe.Style = ExcelStyles.Header;
            oe.SetValue("Hallo Welt1");
            oe.FillDataTable(dt, 4, 1);

            oe.NewSheet("Daten2");

            //oe.SelectSheetFirst("Daten");
            //oe.SelectSheet(1);
            ////oe.SelectRange("A1");
            oe.SelectRange(1, 1);
            oe.Style = ExcelStyles.Header;
            oe.SetValue("Hallo Welt2");
            oe.FillDataTable(dt, 4, 1);

            oe.Quit();

            FileHelper.StartExcel(path);
        }


        [Test]
        public void TestFillDataTableSelectSheet()
        {

            string path = System.IO.Path.Combine(FileHelper.TempPath, "openxml.xlsx");

            if (File.Exists(path)) File.Delete(path);

            var db = SqlClientConnManager.GetConnManager("Data Source=.\\SQLEXPRESS;Initial Catalog=MediaDb;Integrated Security=True");
            var dt = db.GetDataTable("select top 1000 * from settings");

            var oe = new ExcelOpenXml();
            oe.Status += ExcelStatus;
            oe.Error += ExcelError;
            oe.NumberFormatDouble = "#,##0.000000";
            oe.NewWorkbook(path);

            //oe.NewSheet("Daten");
            oe.SelectSheet("Tabelle1");




            ////oe.SelectSheetFirst("Daten");
            ////oe.SelectSheet(1);
            //////oe.SelectRange("A1");
            oe.SelectRange(1, 1);
            oe.Style = ExcelStyles.Header;
            oe.SetValue("Hallo Welt1");
            oe.FillDataTable(dt, 4, 1);

            //oe.NewSheet("Daten2");

            //oe.SelectSheetFirst("Daten");
            //oe.SelectSheet(1);
            ////oe.SelectRange("A1");
            oe.SelectRange(1, 1);
            oe.Style = ExcelStyles.Header;
            oe.SetValue("Hallo Welt2");
            oe.FillDataTable(dt, 4, 1);

            oe.Quit();

            FileHelper.StartExcel(path);
        }


        [Test]
        public void TestFillDataArray()
        {

            string path = System.IO.Path.Combine(FileHelper.TempPath, "openxml1.xlsx");

            var data = new double[2, 2];

            data[0, 0] = 1.5;
            data[0, 1] = 2.5;
            data[1, 1] = 3.5;
            data[1, 0] = 4.5;


            var header = new[] {"Column1", "Column2"};

            var oe = new ExcelOpenXml();
            oe.Status += ExcelStatus;
            oe.Error += ExcelError;
            oe.NumberFormatDouble = "#,##0.000000";
            oe.NewWorkbook(path);
            //oe.SelectSheet("Tabelle1");


            oe.NewSheet("Daten1");

            //oe.SelectSheetFirst("Daten");
            //oe.SelectSheet(1);
            ////oe.SelectRange("A1");
            oe.SelectRange(1, 1);
            oe.Style = ExcelStyles.Header;
            oe.SetValue("Hallo Welt1");
            oe.FillDataArray(data, header, 4, 1);

            oe.NewSheet("Daten2");

            //oe.SelectSheetFirst("Daten");
            //oe.SelectSheet(1);
            ////oe.SelectRange("A1");
            oe.SelectRange(1, 1);
            oe.Style = ExcelStyles.Header;
            oe.SetValue("Hallo Welt2");
            oe.FillDataArray(data, header,  4, 1);

            oe.Quit();

            FileHelper.StartExcel(path);

        }





        private static void ExcelError(Exception ex, string message)
        {
            var s = string.Format("Error:{0}:{1}", ex.Message, message);
            Debug.Print(s);
        }

        private static void ExcelStatus(string message)
        {
            Debug.Print(message);
        }
    }
}