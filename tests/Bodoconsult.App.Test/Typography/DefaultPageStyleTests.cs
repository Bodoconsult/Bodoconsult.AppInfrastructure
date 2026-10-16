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

        // Act
        var t = new DefaultPageStyle();

        // Assert
        Assert.That(t.TypoPaperFormat.PaperFormatName, Is.EqualTo("A4"));
        Assert.That(t.TypoPaperFormat.Size.Width, Is.EqualTo(21.0));
        Assert.That(t.TypoPaperFormat.Size.Height, Is.EqualTo(29.7));
        Assert.That(Math.Abs(t.TypoMargins.Left - 2) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Right - 1.3) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Top - 1.5) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Bottom - 1.2) < Tolerance);
        Assert.That(Math.Abs(t.ColumnWidth - 17.7) < Tolerance);
    }

    [Test]
    public void ColumnWidth_A4Paper2Columns_PropsSetCorrectly()
    {
        // Arrange

        // Act
        var t = new DefaultPageStyle
        {
            ColumnGap = 0.7,
            NumberOfColumns = 2
        };

        // Assert
        Assert.That(t.TypoPaperFormat.PaperFormatName, Is.EqualTo("A4"));
        Assert.That(t.TypoPaperFormat.Size.Width, Is.EqualTo(21.0));
        Assert.That(t.TypoPaperFormat.Size.Height, Is.EqualTo(29.7));
        Assert.That(Math.Abs(t.TypoMargins.Left - 2) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Right - 1.3) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Top - 1.5) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Bottom - 1.2) < Tolerance);
        Assert.That(Math.Abs(t.ColumnWidth - 8.5) < Tolerance);
    }
}