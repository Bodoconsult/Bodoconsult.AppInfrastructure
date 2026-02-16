// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using System.Windows;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.Wpf.ReactiveUI.Converters;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.ReactiveUI.Test.Converters;

[TestFixture]
public class UiWindowStateToWindowStateConverterTests
{

    [Test]
    public void ConvertAndConvertBack_ListWithStrings_ReturnsWindowStateUiWindowState()
    {
        // Arrange 
        var converter = new UiWindowStateToWindowStateConverter();

        var input = UiWindowState.Minimized;

        // Act  
        var result = (WindowState)converter.Convert(input, typeof(WindowState), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(WindowState.Minimized));

        // Act 2
        var result2 = (UiWindowState)converter.ConvertBack(result, typeof(UiWindowState), null, CultureInfo.InvariantCulture);

        // Assert 2
        Assert.That(input== result2);
    }
}