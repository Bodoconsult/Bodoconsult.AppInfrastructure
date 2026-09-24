//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Reflection;
//using Bodoconsult.Database;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;

//namespace Bodoconsult.Office.Test
//{
//    [TestClass]
//    public class UnitTestExcelReader
//    {
//        [Test]
//        public void TestReadExcelFile()
//        {
//            var dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.Parent.Parent;
//            if (dir is null) return;

//            var fileName = Path.Combine(dir.FullName, "TestData\\TestDataExcelReader.xlsx");

//            Assert.IsTrue(File.Exists(fileName));

//            var r = new ExcelReader {FileName = fileName};
//            r.OpenFile();

//            Assert.IsTrue(r.SheetNames.Length>0);

//            // Correct sheetname
//            var dt = r.GetSheet("Master$");
//            Assert.IsTrue(dt.Rows.Count>0);

//            // Missing $ in sheetname
//             dt = r.GetSheet("Master");
//            Assert.IsTrue(dt.Rows.Count > 0);

//            // Sheet by index
//            dt = r.GetSheet(0);
//            Assert.IsTrue(dt.Rows.Count > 0);

//        }




//    }
//}
