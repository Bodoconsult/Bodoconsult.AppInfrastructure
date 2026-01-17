// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Globalization;
using Bodoconsult.App.Wpf.Converters;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.App.Wpf.Test.Converters;

[TestFixture]
public class NumberConverterTests
{
    private readonly double _tolerance = 0.00000000001;
    [Test]
    public void ConvertAndConvertBack_Default_ValueConvertedAndConvertedBack()
    {
        //Arrange
        var converter = new NumberConverter();
        const double input = 0.05;

        // Act
        var erg = (string)converter.Convert(input, typeof (string), null, CultureInfo.CurrentUICulture);


        //Assert
        Assert.That(erg=="0,05");


        var erg1 = (double) (converter.ConvertBack(erg, typeof(string), null, CultureInfo.CurrentUICulture) ?? 0);

        Assert.That(Math.Abs(erg1 - input) < _tolerance);
    }


    [Test]
    public void ConvertAndConvertBack_Default_BigNumber_ValueConvertedAndConvertedBack()
    {
        //Arrange
        var converter = new NumberConverter();
        const double input = 100000.05;

        // Act
        var erg = (string)converter.Convert(input, typeof(string), null, CultureInfo.CurrentUICulture);


        //Assert
        Assert.That(erg == "100.000,05");


        var erg1 = (double)(converter.ConvertBack(erg, typeof(string), null, CultureInfo.CurrentUICulture) ?? 0);

        Assert.That(Math.Abs(erg1 - input) < _tolerance);
    }

    [Test]
    public void ConvertAndConvertBack_Default_BigNumber_CustomFormat_ValueConvertedAndConvertedBack()
    {
        //Arrange
        var converter = new NumberConverter();
        const double input = 100000.05;

        // Act
        var erg = (string)converter.Convert(input, typeof(string), "N5", CultureInfo.CurrentUICulture);


        //Assert
        Assert.That(erg == "100.000,05000");


        var erg1 = (double)(converter.ConvertBack(erg, typeof(string), null, CultureInfo.CurrentUICulture) ?? 0);

        Assert.That(Math.Abs(erg1 - input) < _tolerance);
    }
}