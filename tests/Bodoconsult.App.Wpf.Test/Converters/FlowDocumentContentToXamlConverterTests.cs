// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Threading;
using System.Windows.Documents;
using Bodoconsult.App.Wpf.Converters;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.Test.Converters;

[TestFixture]
public class FlowDocumentContentToXamlConverterTests
{

    [Test]
    public void ConvertAndConvertBack_ValidXaml_ValueConvertedAndConvertedBack()
    {
        //Arrange
        //const string xaml = "<Paragraph>Lorem <Run FontStyle='italic'>ipsum</Run> dolor sit amet, <Run FontWeight='bold'>consetetur sadipscing elitr</Run>, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo <Run FontWeight='bold'>duo dolores et ea rebum</Run>. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.</Paragraph>";
        const string xaml = "<Paragraph>Lorem ipsum dolor sit amet, consetetur sadipscing elitr</Paragraph>";

        var conv = new FlowDocumentContentToXamlConverter();

        // Act
        var doc = (FlowDocument)conv.Convert(xaml, typeof(FlowDocument), null, Thread.CurrentThread.CurrentUICulture);

        //Assert
        Assert.That(doc != null);

        var erg = (string)conv.ConvertBack(doc, typeof(string), null, Thread.CurrentThread.CurrentUICulture);

        Assert.That(erg, Is.EqualTo(xaml));
    }
}