// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Bodoconsult.Office.Test.Helpers;

public static class TestHelper
{
    public const string MassText =
        "Lorem ipsum dolor ≥ = &gt; &#61; sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";


    private static string _testDataPath;

    public static string TestDataPath
    {
        get
        {

            if (!string.IsNullOrEmpty(_testDataPath))
            {
                return _testDataPath;
            }

            var path = new DirectoryInfo(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName).Parent.Parent.Parent.FullName;

            _testDataPath = Path.Combine(path, "TestData");

            if (!Directory.Exists(_testDataPath))
            {
                Directory.CreateDirectory(_testDataPath);
            }

            return _testDataPath;
        }
    }

    /// <summary>
    /// Start an app by file name
    /// </summary>
    /// <param name="fileName"></param>
    public static void StartFile(string fileName)
    {

        if (!Debugger.IsAttached)
        {
            return;
        }

        Assert.That(File.Exists(fileName));

        var p = new Process { StartInfo = new ProcessStartInfo { UseShellExecute = true, FileName = fileName } };

        p.Start();

    }




    public static DataTable GetDataTable(string fileName)
    {
        var path = Path.Combine(TestDataPath, fileName);

        return JsonXmlHelper.GetDataTableFromXml(path);
    }


}