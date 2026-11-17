// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.Reflection;

namespace Bodoconsult.Drawing.SkiaSharp.Tests.Helpers;

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
}