// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using Bodoconsult.App.Wpf.Documents.Test.Helpers;
using Bodoconsult.App.Wpf.Documents.Utilities;

namespace Bodoconsult.App.Wpf.Documents.Test;

public static class FlowDocHelper
{
    public const string MassText =
        "Lorem ipsum dolor ≥ = &gt; &#61; sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";



    public static string MassTextWithImages =
        $"Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. <Image src=\"{TestHelper.TestChartImage}\" />At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

    public static string MassTextWithImages1 =
        $"Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. <Image src=\"{TestHelper.TestDistributionImage}\" />At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. <Image src=\"{TestHelper.TestChartImage}\" /> At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

    public static string MassTextWithImages2 =
        $"<P>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.</P><Image src=\"{TestHelper.TestChartImage}\" /><P>At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.</P><Image src=\"{TestHelper.TestChartImage}\" title=\"Testbild\" height=\"100\" /><P>At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.</P>";



    public const string MassTextTags =
        "<Glyphs UnicodeString= 'Wide load!' Indices= ',150;,100;,100;,100;,100;,100;,100;,100;,100;' FontUri= 'file://c:/windows/fonts/times.ttf' Fill = 'Black' FontRenderingEmSize = '40' />Lorem <Run FontStyle='italic'>ipsum</Run> dolor sit amet, <Run FontWeight='bold'>consetetur sadipscing elitr</Run>, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo <Run FontWeight='bold'>duo dolores et ea rebum</Run>. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

    public const string MassTextShort =
        "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. Liiquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";


    public const string Disclaimer =
        "<P>Die in diesem Papier gemachten Angaben dienen der Unterrichtung und sind keine Aufforderung zum Kauf oder Verkauf von Wertpapieren.</P> <P>Sie ersetzt in keinem Fall eine anlegerspezifische Beratung. Diese Broschüre gibt ausschließlich die Meinung des Autors wieder.</P> <P>Autor, Herausgeber sowie zitierte Quellen haften nicht für etwaige Verluste, die aufgrund der Umsetzung ihrer Gedan-ken und Ideen entstehen.</P> <P>Alle Marken und Warenzeichen sind Eigentum ihrer jeweiligen Eigentümer.</P>";

    public static string ImageTag = $"<Image src=\"{TestHelper.TestChartImage}\" />";

    public static string ImageTagWithTitle = $"<Image src=\"{TestHelper.TestChartImage}\" title=\"Bildunterschrift\"/>";

    public const string Header1 = "Headline Level 1";

    public const string Header2 = "Headline Level 2";

    public const string Header3 = "Headline Level 3";


    public static string[,] GetTableData(int rows, int columns)
    {
        var erg = new string[rows, columns];

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                erg[row, col] = row == 0 ? $"Header {col}" : $"Content R{row}C{col}";
            }

        }

        return erg;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IList<string> GetListData()
    {
        var erg = new List<string>();
        for (var i = 1; i < 2; i++)
        {
            erg.Add($"ListItem {i}{MassTextShort}");
        }

        return erg;
    }


    public static string GetInlineSamples()
    {
        var erg =
            $"Das ist normaler Text.{WpfDocumentTextUtility.LineBreak()}{WpfDocumentTextUtility.Bold("Das ist ein fetter Text.")}{WpfDocumentTextUtility.LineBreak()}{WpfDocumentTextUtility.Italic("Das ist kursiver Text.")}{WpfDocumentTextUtility.LineBreak()}{WpfDocumentTextUtility.Subscript("Das ist tiefgestellter Text.")}{WpfDocumentTextUtility.LineBreak()}{WpfDocumentTextUtility.Superscript("Das ist hochgestellter Text.")}{WpfDocumentTextUtility.LineBreak()}{WpfDocumentTextUtility.ColoredText("Das ist roter Text.", "Red")}{WpfDocumentTextUtility.LineBreak()}{WpfDocumentTextUtility.BackgroundedText("Das ist Text auf gelben Hintergrund.", "Yellow")}{WpfDocumentTextUtility.LineBreak()}{WpfDocumentTextUtility.BackgroundedText(WpfDocumentTextUtility.ColoredText("Das ist roter Text auf gelben Hintergrund.", "Red"), "Yellow")}";

        return erg;
    }


    public static string[,] GetTableDataNumeric(int rows, int columns)
    {
        var erg = new string[rows, columns];

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                string content;

                switch (col)
                {
                    case 1:
                        content = $"{row},{col}";
                        break;

                    case 2:
                        content = $"{row},{col}%";
                        break;

                    //case 3:

                    //    break;
                    default:
                        content = $"Content R{row}C{col}";
                        break;

                }


                erg[row, col] = row == 0 ? $"Header {col}" : content;
            }

        }

        return erg;
    }

}