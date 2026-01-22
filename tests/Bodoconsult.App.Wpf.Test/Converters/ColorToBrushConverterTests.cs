// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Globalization;
using System.Windows.Media;
using Bodoconsult.App.Wpf.Converters;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Test.Converters;

[TestFixture]
public class ColorToBrushConverterTests
{
    [Test]
    public void ConvertAndConvertBack_ValidValue_ValueConvertedAndConvertedBack()
    {
        //Arrange
        var converter = new ColorToBrushConverter();
        var input = Colors.Blue;

        // Act
        var erg = (SolidColorBrush)converter.Convert(input, typeof(SolidColorBrush), null, CultureInfo.CurrentUICulture);


        //Assert
        Assert.That(erg, Is.Not.Null);
        Assert.That(erg.Color, Is.EqualTo(input));


        var erg1 = (Color)(converter.ConvertBack(erg, typeof(SolidColorBrush), null, CultureInfo.CurrentUICulture) ?? 0);
        Assert.That(erg1, Is.EqualTo(input));
    }
}