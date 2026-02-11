// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.Wpf.ReactiveUI.Converters;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.ReactiveUI.Test.Extensions;

[TestFixture]
public class ListStringToFlowDocumentConverterTests
{

    [Test]
    public void Convert_ListWithStrings_ReturnsFlowDocument()
    {
        // Arrange 
        var converter = new ListStringToFlowDocumentConverter();

        var input = new List<string>
        {
            "Blubb",
            "blabb"
        };

        // Act  
        var result = (FlowDocument)converter.Convert(input, typeof(FlowDocument), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Blocks.Count, Is.Not.EqualTo(0));

    }
}

[TestFixture]
public class UiWindowStateToWindowStateConverterTests
{

    [Test]
    public void ConvertAndConvertBack_ListWithStrings_ReturnsFlowDocument()
    {
        // Arrange 
        var converter = new UiWindowStateToWindowStateConverter();

        var input = UiWindowState.Minimized;

        // Act  
        var result = (WindowState)converter.Convert(input, typeof(WindowState), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.Not.Null);

        // Act 2
        var result2 = (UiWindowState)converter.ConvertBack(result, typeof(UiWindowState), null, CultureInfo.InvariantCulture);

        // Assert 2
        Assert.That(input== result2);
    }
}