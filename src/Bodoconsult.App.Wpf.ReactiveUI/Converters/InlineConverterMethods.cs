// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.Wpf.Extensions;
using Bodoconsult.App.Wpf.ReactiveUI.Extensions;

namespace Bodoconsult.App.Wpf.ReactiveUI.Converters;

/// <summary>
/// Static class for predefined inline converter methods for ReactiveUI bindings
/// </summary>
public static class InlineConverterMethods
{
    private static readonly SolidColorBrush Brush = new(Colors.LightSteelBlue);
    private static readonly SolidColorBrush Brush1 = new(Colors.White);
    private static readonly Thickness Margin = new(0, 0, 0, 0);
    private static readonly Thickness Padding = new(0, 10, 0, 10);

    /// <summary>
    /// Convert from <see cref="UiWindowState"/> to <see cref="WindowState"/>
    /// </summary>
    /// <param name="uiWindowState">Current <see cref="UiWindowState"/>  instance</param>
    /// <returns>Returns <see cref="WindowState"/> instance</returns>
    public static WindowState FromUiWindowStateToWindowState(UiWindowState uiWindowState)
    {
        return uiWindowState.ToWindowState();
    }

    /// <summary>
    /// Convert from <see cref="WindowState"/> to <see cref="UiWindowState"/>
    /// </summary>
    /// <param name="windowState">Current <see cref="WindowState"/>  instance</param>
    /// <returns>Returns <see cref="UiWindowState"/> instance</returns>
    public static UiWindowState FromWindowStateToUiWindowState(WindowState windowState)
    {
        return windowState.ToUiWindowState();
    }

    /// <summary>
    /// Converts from IList string as content to a WPF FlowDocument.
    /// </summary>
    public static FlowDocument FromListStringToFlowDocument(IList<string> list)
    {
        var doc = new FlowDocument
        {
            FontFamily = SystemFonts.StatusFontFamily,
            FontSize = 14,
            PageWidth = 1000,
            ColumnWidth = 1000,
            IsOptimalParagraphEnabled = true,
            IsHyphenationEnabled = true
        };

        var isActive = false;

        for (var index = list.Count - 1; index >= 0; index--)
        {
            var message = list[index];
            var myParagraph = new Paragraph
            {
                Margin = Margin,
                Padding = Padding,
                Background = isActive ? Brush : Brush1
            };

            isActive = !isActive;

            myParagraph.Inlines.Add(message);
            doc.Blocks.Add(myParagraph);
        }

        return doc;
    }

    /// <summary>
    /// Convert a <see cref="TypoColor"/> to a <see cref="SolidColorBrush"/>
    /// </summary>
    /// <param name="color">Current <see cref="TypoColor"/></param>
    /// <returns><see cref="SolidColorBrush"/> instance</returns>
    public static SolidColorBrush FromTypoColorToSolidColorBrush(TypoColor color)
    {
        return new SolidColorBrush(color.ToColor());
    }

    /// <summary>
    /// Convert a <see cref="SolidColorBrush"/> to a <see cref="TypoColor"/>
    /// </summary>
    /// <param name="solidColorBrush">Current <see cref="SolidColorBrush"/> instance</param>
    /// <returns><see cref="TypoColor"/> instance</returns>
    public static TypoColor FromSolidColorBrushToTypoColor(SolidColorBrush solidColorBrush)
    {
        return solidColorBrush.Color.ToTypoColor();
    }

    /// <summary>
    /// Convert a <see cref="TypoColor"/> to a <see cref="SolidColorBrush"/>
    /// </summary>
    /// <param name="htmlColor">HTML color string like #FFFFFFFF</param>
    /// <returns><see cref="SolidColorBrush"/> instance</returns>
    public static SolidColorBrush FromHtmlColorToSolidColorBrush(string htmlColor)
    {
        var o = ColorConverter.ConvertFromString(htmlColor);

        if (o == null)
        {
            throw new ArgumentNullException(nameof(o));
        }

        var color = (Color)o;

        var brush = new SolidColorBrush(color);
        return brush;
    }

    /// <summary>
    /// Convert a <see cref="SolidColorBrush"/> to a <see cref="TypoColor"/>
    /// </summary>
    /// <param name="solidColorBrush">Current <see cref="SolidColorBrush"/> instance</param>
    /// <returns>HTML color string like #FFFFFFFF</returns>
    public static string FromSolidColorBrushToHtmlColor(SolidColorBrush solidColorBrush)
    {
        return solidColorBrush.Color.ToHtml();
    }
}