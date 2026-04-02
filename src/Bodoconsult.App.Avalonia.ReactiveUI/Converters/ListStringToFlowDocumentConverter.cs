//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using System.Globalization;

//namespace Bodoconsult.App.Avalonia.ReactiveUI.Converters;

///// <summary>
///// Converts an IList&lt;string&gt; to a <see cref="FlowDocument"/>. One-way-converter!
///// </summary>
//[ValueConversion(typeof(IList<string>), typeof(FlowDocument))]
//public class ListStringToFlowDocumentConverter : BaseConverter, IValueConverter
//{
//    private readonly SolidColorBrush _brush = new(Colors.LightSteelBlue);
//    private readonly SolidColorBrush _brush1 = new(Colors.White);
//    private readonly Thickness _margin = new(0, 0, 0, 0);
//    private readonly Thickness _padding = new(0, 10, 0, 10);

//    #region IValueConverter Members

//    /// <summary>
//    /// Converts from IList string as content to a Avalonia FlowDocument.
//    /// </summary>
//    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
//    {
//        if (value is not IList<string> data)
//        {
//            return new FlowDocument();
//        }

//        var doc = new FlowDocument
//        {
//            FontFamily = SystemFonts.StatusFontFamily,
//            FontSize = 14,
//            PageWidth = 1000,
//            ColumnWidth = 1000,
//            IsOptimalParagraphEnabled = true,
//            IsHyphenationEnabled = true
//        };

//        var isActive = false;

//        for (var index = data.Count - 1; index >= 0; index--)
//        {
//            var message = data[index];
//            var myParagraph = new Paragraph
//            {
//                Margin = _margin,
//                Padding = _padding,
//                Background = isActive ? _brush : _brush1
//            };

//            isActive = !isActive;

//            myParagraph.Inlines.Add(message);
//            doc.Blocks.Add(myParagraph);
//        }

//        return doc;
//    }

//    /// <summary>
//    /// Converts the content of a Avalonia FlowDocument to a XAML markup string.
//    /// </summary>
//    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
//    {
//        throw new NotSupportedException();
//    }

//    #endregion
//}