// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Bodoconsult.App.Wpf.Converters;
///// <summary>
///// Converts the content of a XAML string to a <see cref="FlowDocument"/> and back
///// </summary>
//[ValueConversion(typeof(string),typeof(FlowDocument))]
//public class XamlStringFlowDocumentConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//         var strValue = value.ToString();
//        var document = new FlowDocument();
//        var tr = new TextRange(document.ContentStart, document.ContentEnd) {Text = strValue};
//        return document;           
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        var date = (FlowDocument)value;
//        var tr = new TextRange(date.ContentStart, date.ContentEnd);
//        return tr.Text;
//    }
//}

/// <summary>
/// Comvert a FlowDocument to XAML
/// </summary>
[ValueConversion(typeof(string), typeof(FlowDocument))]
public class FlowDocumentToXamlConverter : BaseConverter, IValueConverter
{
    #region IValueConverter Members

    /// <summary>
    /// Converts from XAML markup to a WPF FlowDocument.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        /* See http://stackoverflow.com/questions/897505/getting-a-flowdocument-from-a-xaml-template-file */

        var flowDocument = new FlowDocument();
        if (value == null)
        {
            return flowDocument;
        }

        var xamlText = (string)value;
        flowDocument = (FlowDocument)XamlReader.Parse(xamlText);

        // Set return value
        return flowDocument;
    }

    /// <summary>
    /// Converts from a WPF FlowDocument to a XAML markup string.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        /* This converter does not insert returns or indentation into the XAML. If you need to
         * indent the XAML in a text box, see http://www.knowdotnet.com/articles/indentxml.html */

        // Exit if FlowDocument is null
        if (value == null)
        {
            return string.Empty;
        }

        // Get flow document from value passed in
        var flowDocument = (FlowDocument)value;

        // Convert to XAML and return
        return XamlWriter.Save(flowDocument);
    }

    #endregion
}