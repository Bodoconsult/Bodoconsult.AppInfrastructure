// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Bodoconsult.Test;

/// <summary>
/// TestHelperHtml class prints to a HTML file
/// </summary>
public class TestDocuHelperHtml : TestDocuHelperBase
{

    private readonly StringBuilder _content = new(100000);

    /// <summary>
    /// Inline CSS-style sheet 
    /// </summary>
    public string Css { get; set; }


    /// <summary>
    /// Inline CSS-style sheet  for printing
    /// </summary>
    public string CssPrint { get; set; }


    private const string DefaultClass = "";

    /// <summary>
    /// Set the defaults for CSS settings
    /// </summary>
    public override void SetDefaults()
    {
        CurrentClass = DefaultClass;
    }

    /// <summary>
    /// default ctor
    /// </summary>
    public TestDocuHelperHtml()
    {
        CurrentClass = DefaultClass;
    }

    /// <summary>
    /// Initialize helper class
    /// </summary>
    public override void HelperInitialize()
    {
        // Delete file if existing
        if (File.Exists(TargetPath))
        {
            File.Delete(TargetPath);
        }

        if (string.IsNullOrEmpty(Css))
        {
            Css = LoadDefaultStyleSheet();
        }

        if (string.IsNullOrEmpty(CssPrint))
        {
            CssPrint = LoadDefaultStyleSheetPrint();
        }


        // Stylesheet for screen
        var cssPath = Path.Combine(DirPath, "layout.css");

        if (File.Exists(cssPath))
        {
            File.Delete(cssPath);
        }

        // Write to css file
        using (var sw = File.AppendText(cssPath))
        {
            sw.Write(Css);
        }

        // Stylesheet for printing
        cssPath = Path.Combine(DirPath, "layoutPrint.css");

        if (File.Exists(cssPath))
        {
            File.Delete(cssPath);
        }

        // Write to css file
        using (var sw = File.AppendText(cssPath))
        {
            sw.Write(CssPrint);
        }

        PrintIt1("<!DOCTYPE html>");
        PrintIt1("<html xml:lang=\"de-DE\" >");
        PrintIt1("<head>");
        PrintIt1("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
        PrintIt1("<title>Automatically created test report</title>");
        PrintIt1("<link rel=\"stylesheet\" href=\"layout.css\" type=\"text/css\"  media=\"screen\" />");
        PrintIt1("<link rel=\"stylesheet\" href=\"layoutPrint.css\" type=\"text/css\"  media=\"print\" />");
        PrintIt1("</head>");

        PrintIt1("<body>");
        PrintIt1("<div id=\"content\">");
    }
    /// <summary>
    /// Finalize helper class
    /// </summary>
    public override void HelperFinalize()
    {
        PrintIt1("</div>");
        PrintIt1("</body>");
        PrintIt1("</html>");

        // Write to file
        using (var sw = File.AppendText(TargetPath))
        {
            sw.Write(_content.ToString());
        }
    }

    /// <summary>
    /// Print a message in the debug window and the output file
    /// </summary>
    /// <param name="inputString"></param>
    /// <param name="array"></param>
    public override void PrintIt(string inputString, params object[] array)
    {
        var pClass = string.IsNullOrEmpty(CurrentClass) ? "<p>" : $"<p class=\"{CurrentClass}\">";
        _content.AppendFormat(pClass+inputString.Replace("\r\n", "<br />")+"</p>", array);
        _content.Append("\r\n");
    }


    private void PrintIt1(string inputString, params object[] array)
    {
        _content.AppendFormat(inputString, array);
        _content.Append("\r\n");
    }


    /// <summary>
    /// Print a header for the output of unit testing
    /// </summary>
    public override void PrintHeader(string message)
    {
        PrintIt1("<h1>{0}</h1>", message);
        PrintIt1("<p>Date: {0}</p>", DateTime.Now);
    }

    /// <summary>
    /// Print divider
    /// </summary>
    public override void PrintDivider()
    {
        PrintIt1("<p>{0}</p>", Divider);

    }

    /// <summary>
    /// Print divider 1
    /// </summary>
    public override void PrintDivider1()
    {
        PrintIt1("<p>{0}</p>", Divider1);

    }


    /// <summary>
    /// Print divider
    /// </summary>
    public override void PrintShortDivider()
    {
        PrintIt1("<p>{0}</p>", ShortDivider);

    }

    /// <summary>
    /// Print an empty row
    /// </summary>
    public override void PrintRow()
    {
        PrintIt1("<p>&nbsp;</p>");
    }


    /// <summary>
    /// Print a message header level 1
    /// </summary>
    /// <param name="message">message to print</param>
    /// <param name="typeName">name of the type</param>
    /// <param name="methodName">name of the calling method</param>
    public override void PrintMethodHeader(string message, string typeName, string methodName)
    {
        //PrintRow();
        //PrintRow();
        //PrintDivider1();
        //PrintDivider();
        PrintIt1("<h3>{0}</h3>", message);
        // get calling method name

        PrintIt1("<p>Unit test class: {0}<br />Unit test method: {1}</p>", typeName, methodName);
        //PrintDivider();
        //PrintRow();

    }

    /// <inheritdoc />
    public override void PrintMethodHeader2(string title)
    {
        //PrintRow();
        //PrintShortDivider();
        PrintIt1("<h4>{0}</h4>", title);
        //PrintRow();
    }


    /// <inheritdoc />
    public override void PrintMethodHeader3(string title)
    {
        //PrintRow();
        //PrintShortDivider();
        PrintIt1("<h5>{0}</h5>", title);
        //PrintRow();
    }


    /// <inheritdoc />
    public override void PrintClassHeader(string message, string className)
    {
        //PrintRow();
        //PrintRow();
        //PrintDivider1();
        //PrintDivider();
        //PrintDivider();
        PrintIt1("<h2>{0}</h2>", message);
        // get calling method name

        PrintIt1("<p>Unit test class: {0}</p>",className);
        //PrintDivider();
        //PrintRow();

    }

    /// <inheritdoc />
    public override void PrintImage(string fileNameWithoutPath)
    {
        var path = Path.Combine(DirPath, fileNameWithoutPath);
        PrintIt1("<p class=\"resultImages\"><img src=\"{0}\" /></p>", path);

    }

    /// <summary>
    /// Load the embedded default stylesheet
    /// </summary>
    /// <returns></returns>
    public string LoadDefaultStyleSheet()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "Bodoconsult.Test.StyleSheet.css";

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream is null) return null;

            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();
                return result;
            }

        }

    }


    /// <summary>
    /// Load the embedded default stylesheet
    /// </summary>
    /// <returns></returns>
    public string LoadDefaultStyleSheetPrint()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "Bodoconsult.Test.StyleSheetPrint.css";

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream is null) return null;

            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();
                return result;
            }

        }

    }


    /// <inheritdoc />
    public override void PrintTable(string[,] data)
    {
        var erg = new StringBuilder();

        erg.AppendLine("<table>\r\n");

        for (var x = 0; x < data.GetLength(0); x++)
        {
            erg.AppendLine("\t<tr>\r\n");

            var tag = (x == 0) ? "th" : "td";

            for (var y = 0; y < data.GetLength(1); y++)
            {
                erg.AppendFormat("\t\t<{1}>{0}</{1}>\r\n", string.IsNullOrEmpty( data[x, y]) ? "&nbsp;": data[x, y], tag);
            }

            erg.AppendLine("\t</tr>\r\n");
        }

        erg.AppendLine("</table>\r\n");

        PrintIt1(erg.ToString());
    }

    /// <inheritdoc />
    public override void PrintLink(string url, string title)
    {
        var msg = string.Format("<a href=\"{1}\">{0}</a>", title, url);
        PrintIt(msg);
    }

}