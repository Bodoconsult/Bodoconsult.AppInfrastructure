// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using System.Windows.Media;
using Bodoconsult.App.Wpf.Converters;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Test.Converters;

[TestFixture]
public class HtmlColorStringToBrushConverterTests
{
    [Test]
    public void ConvertAndConvertBack_ValidValue_ValueConvertedAndConvertedBack()
    {
        //Arrange
        var converter = new HtmlColorStringToBrushConverter();
        var input = "#D3D3D3";
        var expectedResult = Colors.LightGray;

        // Act
        var erg = (SolidColorBrush)converter.Convert(input, typeof(SolidColorBrush), null, CultureInfo.CurrentUICulture);

        //Assert
        Assert.That(erg, Is.Not.Null);
        Assert.That(erg.Color, Is.EqualTo(expectedResult));


        var erg1 = (string)(converter.ConvertBack(erg, typeof(SolidColorBrush), null, CultureInfo.CurrentUICulture) ?? 0);
        Assert.That(erg1, Is.EqualTo(input));
    }
}