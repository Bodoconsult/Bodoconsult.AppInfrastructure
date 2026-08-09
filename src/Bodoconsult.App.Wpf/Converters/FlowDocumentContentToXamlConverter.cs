// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Bodoconsult.App.Wpf.Converters;

/// <summary>
/// Converts the content of a FlowDocument to XAML
/// </summary>
[ValueConversion(typeof(string), typeof(FlowDocument))]
public class FlowDocumentContentToXamlConverter : BaseConverter, IValueConverter
{
    #region IValueConverter Members

    /// <summary>
    /// Converts from XAML markup as content to a WPF FlowDocument.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        /* See http://stackoverflow.com/questions/897505/getting-a-flowdocument-from-a-xaml-template-file */


        if (value is null)
        {
            return new FlowDocument();
        }
        var xamlText =
            $"<FlowDocument xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{(string)value}</FlowDocument>";

        var flowDocument = (FlowDocument)XamlReader.Parse(xamlText);

        return flowDocument;
    }

    /// <summary>
    /// Converts the content of a WPF FlowDocument to a XAML markup string.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        /* This converter does not insert returns or indentation into the XAML. If you need to
         * indent the XAML in a text box, see http://www.knowdotnet.com/articles/indentxml.html */

        // Exit if FlowDocument is null
        if (value is null)
        {
            return string.Empty;
        }

        // Get flow document from value passed in
        var flowDocument = (FlowDocument)value;

        // Convert to XAML and return
        var erg =XamlWriter.Save(flowDocument).Replace("</FlowDocument>", string.Empty);

        var i = erg.IndexOf(">", StringComparison.CurrentCultureIgnoreCase);

        erg = erg[(i + 1)..];

        return erg;
    }

    #endregion
}