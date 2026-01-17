// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Globalization;
using Bodoconsult.App.Wpf.Converters;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.App.Wpf.Test.Converters;

[TestFixture]
public class PercentageConverterTests
{
    private readonly double _tolerance = 0.00000000001;
    [Test]
    public void ConvertAndConvertBack_ValidValue_ValueConvertedAndConvertedBack()
    {
        //Arrange
        var converter = new PercentageConverter();
        const double input = 0.05;

        // Act
        var erg = (string)converter.Convert(input, typeof(string), null, CultureInfo.CurrentUICulture);


        //Assert
        Assert.That(erg, Is.Not.Null);
        Assert.That(erg, Is.EqualTo("5,00 %"));


        var erg1 = (double)(converter.ConvertBack(erg, typeof(string), null, CultureInfo.CurrentUICulture) ?? 0);

        Assert.That(Math.Abs(erg1 - input) < _tolerance);
    }


    [Test]
    public void ConvertAndConvertBack_ValidValueBigNumber_ValueConvertedAndConvertedBack()
    {
        //Arrange
        var converter = new PercentageConverter();
        const double input = 100000.05;

        // Act
        var erg = (string)converter.Convert(input, typeof(string), null, CultureInfo.CurrentUICulture);


        //Assert
        Assert.That(erg, Is.EqualTo( "10.000.005,00 %"));


        var erg1 = (double)(converter.ConvertBack(erg, typeof(string), null, CultureInfo.CurrentUICulture) ?? 0);

        Assert.That(Math.Abs(erg1 - input) < _tolerance);
    }
}