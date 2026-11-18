// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Bodoconsult.Drawing.Test.Helpers;

public static class TestHelper
{
    static TestHelper()
    {
        var path = Assembly.GetExecutingAssembly().Location;

        ArgumentNullException.ThrowIfNull(path);

        var dir1 = Path.GetDirectoryName(path);

        ArgumentNullException.ThrowIfNull(dir1);

        var dir = new DirectoryInfo(dir1).Parent?.Parent?.Parent;

        ArgumentNullException.ThrowIfNull(dir);

        GetTestDataFolder = Path.Combine(dir.FullName, "TestData");
    }

    public const string MassText =
        "Lorem ipsum dolor ≥ = &gt; &#61; sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. \r\n\r\nAt vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";


    /// <summary>
    /// Target folder to store files in
    /// </summary>
    public static string TargetFolder { get; } = Path.GetTempPath();

    /// <summary>
    /// Get the folder of the current test data
    /// </summary>
    /// <returns></returns>
    public static string GetTestDataFolder { get; }

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

    /// <summary>
    /// Check if a file is existing and if yes delete it
    /// </summary>
    /// <param name="fileName">File to be deleted</param>
    public static void DeleteFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }
}