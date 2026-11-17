// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reflection;
using Bodoconsult.App.Abstractions.Typography;

namespace Bodoconsult.App.Test.Typography;

[TestFixture]
internal class BaseTypoPageStyleTests
{
    ///// <summary>
    ///// Value used for float is equal to 0 comparisons
    ///// </summary>
    //private const double CompareToZeroValue =
    //    0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001;

    private const double Tolerance = 0.0001;

    private readonly string _exportFolderName;

    public BaseTypoPageStyleTests()
    {
        var directoryInfo = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent;
        var dir = directoryInfo?.Parent?.Parent?.Parent;

        if (dir != null)
        {
            _exportFolderName = Path.Combine(dir.FullName, "SampleData");
        }
    }

    [Test]
    public void SetMargins_BaseSettings_Successful()
    {
        // Arrange
        var t = new TestTypoPageStyle();

        // Act
        t.SetMargins();

        // Assert
        Assert.That(t.TypeAreaRect.Size.Width - 175 < Tolerance);
        Assert.That(Math.Abs(t.MarginUnit - 0.875) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Left - 1.75) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Right - 1.75) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Top - 0.88) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Bottom - 1.75) < Tolerance);

        var erg = t.GetPixelHeight(4);

        Assert.That(Math.Abs(t.GetWidth(4) - 11.5) < Tolerance);
        Assert.That(Math.Abs(t.GetHeight(4) - 7.11) < Tolerance);

        Assert.That(
            Math.Abs(t.GetPixelWidth(4) - (int)(11.5 * TypographicConstants.InchPerCentimeter * 96)) < Tolerance);
        Assert.That(Math.Abs(t.GetPixelHeight(4) - (int)(7.11 * TypographicConstants.InchPerCentimeter * 96)) <
                    Tolerance);
    }
}