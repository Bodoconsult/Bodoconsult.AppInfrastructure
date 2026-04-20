// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using System.Reflection;

namespace Bodoconsult.App.Wpf.Documents.Test.Helpers;

internal static class TestHelper
{

    static TestHelper()
    {
        Assembly = typeof(TestHelper).Assembly;

        var fi = new FileInfo(Assembly.Location);

        AppPath = fi.DirectoryName;

        ArgumentNullException.ThrowIfNull(fi.Directory?.Parent?.Parent?.Parent?.Parent);

        TestDataPath = Path.Combine(fi.Directory.Parent.Parent.Parent.Parent.FullName, "TestData");

        TestChartImage = Path.Combine(TestDataPath, "chart3d.png");

        TestDistributionImage = Path.Combine(TestDataPath, "NormalDistribution.de.png");

        TestLogoImage = Path.Combine(TestDataPath, "logo.jpg");
    }

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
    /// An image of a normal distribution
    /// </summary>
    public static string TestDistributionImage { get; }

    /// <summary>
    /// A logo image
    /// </summary>
    public static string TestLogoImage { get; }



}