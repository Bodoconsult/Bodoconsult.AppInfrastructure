// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ABI.Windows.UI;
using Bodoconsult.App.Abstractions.Extensions;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.Wpf.ReactiveUI.Converters;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.ReactiveUI.Test.Converters;

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
internal class InlineConverterMethodsTests
{
    [Test]
    public void FromUiWindowStateToWindowState_Minimized_ReturnsMinimized()
    {
        // Arrange 
        const UiWindowState ws = UiWindowState.Minimized;

        // Act  
        var result = InlineConverterMethods.FromUiWindowStateToWindowState(ws);

        // Assert
        Assert.That(result, Is.EqualTo(WindowState.Minimized));
    }

    [Test]
    public void FromWindowStateToUiWindowState_Minimized_ReturnsMinimized()
    {
        // Arrange 
        const WindowState ws = WindowState.Minimized;

        // Act  
        var result = InlineConverterMethods.FromWindowStateToUiWindowState(ws);

        // Assert
        Assert.That(result, Is.EqualTo(UiWindowState.Minimized));
    }

    [Test]
    public void FromListStringToFlowDocument_Minimized_ReturnsMinimized()
    {
        // Arrange 
        var list = new List<string>
        {
            "Blubb",
            "Blabb"
        };

        // Act  
        var result = InlineConverterMethods.FromListStringToFlowDocument(list);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void FromTypoColorToSolidColorBrush_Blue_ReturnsBlue()
    {
        // Arrange 
        var color = TypoColors.Blue;

        // Act  
        var result = InlineConverterMethods.FromTypoColorToSolidColorBrush(color);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Color, Is.EqualTo(Colors.Blue));
    }

    [Test]
    public void FromSolidColorBrushToTypoColor_Blue_ReturnsBlue()
    {
        // Arrange 
        var brush = new SolidColorBrush(Colors.Blue);

        // Act  
        var result = InlineConverterMethods.FromSolidColorBrushToTypoColor(brush);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(TypoColors.Blue));
    }

    [Test]
    public void FromHtmlColorToSolidColorBrush_Blue_ReturnsBlue()
    {
        // Arrange 
        var color = TypoColors.Blue.ToHtml();

        // Act  
        var result = InlineConverterMethods.FromHtmlColorToSolidColorBrush(color);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Color, Is.EqualTo(Colors.Blue));
    }

    [Test]
    public void FromSolidColorBrushToHtmlColor_Blue_ReturnsBlue()
    {
        // Arrange 
        var brush = new SolidColorBrush(Colors.Blue);

        // Act  
        var result = InlineConverterMethods.FromSolidColorBrushToHtmlColor(brush);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(TypoColors.Blue.ToHtml()));
    }

}