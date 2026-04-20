// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.IO;
using System.Reflection;

namespace Bodoconsult.Text.Test.Helpers;

public class TestHelper
{

    static TestHelper()
    {

        Assembly = typeof(TestHelper).Assembly;

        var fi = new FileInfo(Assembly.Location);

        AppPath = fi.DirectoryName;

        ArgumentNullException.ThrowIfNull(AppPath);
        ArgumentNullException.ThrowIfNull(fi.Directory?.Parent?.Parent?.Parent?.Parent?.Parent);

        if (AppPath.Contains("\\windows\\"))
        {
            TestDataPath = Path.Combine(fi.Directory.Parent.Parent.Parent.Parent.Parent.FullName, "Bodoconsult.Text.Test", "TestData");
        }
        else
        {
            TestDataPath = Path.Combine(fi.Directory.Parent.Parent.Parent.Parent.FullName, "Bodoconsult.Text.Test", "TestData");
        }

        TestChartImage = Path.Combine(TestDataPath, "chart3d.png");

        TestEquationImage = Path.Combine(TestDataPath, "equation.png");

        TestDistributionImage = Path.Combine(TestDataPath, "NormalDistribution.de.png");

        TestLogoImage = Path.Combine(TestDataPath, "logo.jpg");

        TestLogoImage2 = Path.Combine(TestDataPath, "logo_bre.png");

        TestBackgroundImage = Path.Combine(TestDataPath, "background.png");

        TempPath = Path.GetTempPath();

        if (!Directory.Exists(TempPath))
        {
            Directory.CreateDirectory(TempPath);
        }
    }


    /// <summary>
    /// Folder path for temporary files
    /// </summary>
    public static string TempPath { get; }

    /// <summary>
    /// Current assembly
    /// </summary>
    public static Assembly Assembly;

    /// <summary>
    /// Current app path
    /// </summary>
    public static string AppPath { get; }

    /// <summary>
    /// Current test data path
    /// </summary>
    public static string TestDataPath { get; }

    /// <summary>
    /// A chart image
    /// </summary>
    public static string TestChartImage { get; }

    /// <summary>
    /// An equiation image image
    /// </summary>
    public static string TestEquationImage { get; }

    /// <summary>
    /// An image of a normal distribution
    /// </summary>
    public static string TestDistributionImage { get; }

    /// <summary>
    /// A logo image
    /// </summary>
    public static string TestLogoImage { get; }

    /// <summary>
    /// A logo image 2
    /// </summary>
    public static string TestLogoImage2 { get; }

    /// <summary>
    /// An image as backgrounf
    /// </summary>
    public static string TestBackgroundImage { get; }
}