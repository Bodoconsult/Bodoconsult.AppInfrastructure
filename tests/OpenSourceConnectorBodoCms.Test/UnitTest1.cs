//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using System.Runtime.InteropServices.ComTypes;
//using NUnit.Framework;
//using OpenSourceConnectorBodoCms.Business.Helpers;
//using OpenSourceConnectorBodoCms.Business.Model;

//namespace OpenSourceConnectorBodoCms.Test;

//public class UnitTests
//{
//    [SetUp]
//    public void Setup()
//    {
//    }

//    [Test]
//    public void Test1()
//    {
//        var s = new AppSettings
//        {
//            SourcePath = @"D:\Daten\Projekte\Tools",
//            FolderFilter = "Bodoconsult.Core",
//            TargetPath = @"D:\temp\OpenSourceConnectorBodoCms"
//        };

//        JsonHelper.SaveAsFile(@"D:\temp\appSettings.json", s);
//    }

//    [Test]
//    public void Test2()
//    {
//        var s = new OpenSourceMetaData
//        {
//            NugetPackage = @"Bodoconsult.Core.Database",
//            Category = "Database"
//        };

//        JsonHelper.SaveAsFile(@"D:\temp\OpenSourceMetaData.json", s);
//    }
//}