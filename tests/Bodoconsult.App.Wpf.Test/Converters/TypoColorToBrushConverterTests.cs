// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using System.Windows.Media;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Converters;
using Bodoconsult.App.Wpf.Extensions;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Test.Converters;

[TestFixture]
public class TypoColorToBrushConverterTests
{
    [Test]
    public void ConvertAndConvertBack_ValidValue_ValueConvertedAndConvertedBack()
    {
        //Arrange
        var converter = new TypoColorToBrushConverter();
        var input = TypoColors.Blue;

        // Act
        var erg = (SolidColorBrush)converter.Convert(input, typeof(SolidColorBrush), null, CultureInfo.CurrentUICulture);

        //Assert
        Assert.That(erg, Is.Not.Null);
        Assert.That(erg.Color.IsEqualTo(input));


        var erg1 = (TypoColor)(converter.ConvertBack(erg, typeof(SolidColorBrush), null, CultureInfo.CurrentUICulture) ?? 0);
        Assert.That(erg1.Equals(input));
    }
}