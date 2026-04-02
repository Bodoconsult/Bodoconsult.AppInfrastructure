// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using Avalonia.Controls;
using Bodoconsult.App.Avalonia.ReactiveUI.Converters;
using Bodoconsult.App.ReactiveUI.Ui;
using NUnit.Framework;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.Converters;

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