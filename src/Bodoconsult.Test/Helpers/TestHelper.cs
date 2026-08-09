// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Bodoconsult.Test.Helpers;

/// <summary>
/// Test helper class
/// </summary>
public class TestHelper
{

    private static readonly Assembly Ass;

    static TestHelper()
    {
        Ass = Assembly.GetExecutingAssembly();
        AppPath = new FileInfo(Ass.Location).Directory.FullName;
        TestDataPath = Path.Combine(AppPath, "TestData");
    }

    /// <summary>
    /// Current app path
    /// </summary>
    public static string AppPath { get; }

    /// <summary>
    /// Test data path
    /// </summary>
    public static string TestDataPath { get; set; }


    /// <summary>
    /// Get a text from an embedded resource file
    /// </summary>
    /// <param name="resourceName">resource name = plain file name without extension and path</param>
    /// <returns></returns>
    internal static string GetTextResource(string resourceName)
    {

        resourceName = $"Bodoconsult.Test.Resources.{resourceName}.txt";

        var str = Ass.GetManifestResourceStream(resourceName);

        if (str is null) return null;

        string s;

        using (var file = new StreamReader(str))
        {
            s = file.ReadToEnd();
        }

        return s;
    }

    /// <summary>
    /// Opens a file in a separate process. Executable not required
    /// </summary>
    /// <param name="path">File to open</param>
    public static void OpenFile(string path)
    {
        if (!Debugger.IsAttached)
        {
            return;
        }

        var p = new Process
        {
            StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            }
        };
        p.Start();
    }
}