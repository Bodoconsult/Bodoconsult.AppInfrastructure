// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Test.Typography;

[TestFixture]
internal class DefaultPageStyleTests
{
    /// <summary>
    /// Value used for float is equal to 0 comparisons
    /// </summary>
    private const double CompareToZeroValue = 0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001;

    private const double Tolerance = 0.0001;

    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorrectly()
    {
        // Arrange
        var t = new DefaultPageStyle();

        // Act
        t.SetMargins();

        // Assert
        Assert.That(t.TypoPaperFormat.PaperFormatName, Is.EqualTo("A4"));
        Assert.That(t.TypoPaperFormat.Size.Width, Is.EqualTo(21.0));
        Assert.That(t.TypoPaperFormat.Size.Height, Is.EqualTo(29.7));
        Assert.That(Math.Abs(t.TypoMargins.Left - 1.75) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Right - 1.75) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Top - 0.88) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Bottom - 1.75) < Tolerance);
        Assert.That(Math.Abs(t.ColumnWidth - 17.5) < Tolerance);
    }

    [Test]
    public void ColumnWidth_A4PaperLandscape2Columns_PropsSetCorrectly()
    {
        // Arrange
        var t = new DefaultPageStyle
        {
            GridColumnWidth = 1.7,
            ColumnGap = 0.3,
            NumberOfColumns = 2,
            TypoPaperFormat =
            {
                Landscape = true
            }
        };

        // Act
        t.SetMargins();

        // Assert
        Assert.That(t.TypoPaperFormat.PaperFormatName, Is.EqualTo("A4"));
        Assert.That(t.TypoPaperFormat.Size.Width, Is.EqualTo(21.0));
        Assert.That(t.TypoPaperFormat.Size.Height, Is.EqualTo(29.7));
        Assert.That(Math.Abs(t.TypoMargins.Left - 2) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Right - 2) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Top - 1) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Bottom - 2) < Tolerance);
        Assert.That(Math.Abs(t.ColumnWidth - 12.7) < Tolerance);
    }
}