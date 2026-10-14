// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Bodoconsult.Text.Enums;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Model;

namespace Bodoconsult.Text.Formatter;

/// <summary>
/// Formatting <see cref="IStructuredText"/> to HTML
/// </summary>
public class HtmlTextFormatter : ITextFormatter
{
    #region Internal values

    private int _rowCount;

    private IList<ITextItem> TextItems { get; set; }

    #endregion

    /// <summary>
    /// Default ctor
    /// </summary>
    public HtmlTextFormatter()
    {
        CssTitle = "Title";
        AddTableOfContent = true;
        TocCaption = "Table of content";
        CssToc1 = "Toc1";
        CssToc2 = "Toc2";
        CssToc3 = "Toc3";
        CssToc4 = "Toc4";
        CssTable = "wr_table";
        CssThLeft = "wr_header";
        CssThRight = "wr_header_right";
        CssThCenter = "wr_header_center";
        CssTdLeft = "wr_cell";
        CssTdRight = "wr_cell_right";
        CssTdCenter = "wr_cell_center";
        CssTdLeftAlt = "wr_cell_alt";
        CssTdRightAlt = "wr_cell_right_alt";
        CssTdCenterAlt = "wr_cell_center_alt";


        TextItems = new List<ITextItem>();
        TocLevel = 2;
    }

    /// <summary>
    /// Css class to use for title paragraph. Default: "Title"
    /// </summary>
    public string CssTitle { get; set; }

    /// <summary>
    /// Css class to use for TOC level 1: "Toc1"
    /// </summary>
    public string CssToc1 { get; set; }

    /// <summary>
    /// Css class to use for TOC level 2: "Toc2"
    /// </summary>
    public string CssToc2 { get; set; }

    /// <summary>
    /// Css class to use for TOC level 3: "Toc3"
    /// </summary>
    public string CssToc3 { get; set; }

    /// <summary>
    /// Css class to use for TOC level 4: "Toc4"
    /// </summary>
    public string CssToc4 { get; set; }


    /// <summary>
    /// Css class to use for table tag. Default: "wr_table"
    /// </summary>
    public string CssTable { get; set; }

    /// <summary>
    /// Css class to use for th tag with left adjustment. Default: "wr_header"
    /// </summary>
    public string CssThLeft { get; set; }

    /// <summary>
    /// Css class to use for th tag with right adjustment. Default: "wr_header_right"
    /// </summary>
    public string CssThRight { get; set; }

    /// <summary>
    /// Css class to use for th tag with right adjustment. Default: "wr_headerl_center"
    /// </summary>
    public string CssThCenter { get; set; }


    /// <summary>
    /// Css class to use for td tag with left adjustment. Default: "wr_cell"
    /// </summary>
    public string CssTdLeft { get; set; }

    /// <summary>
    /// Css class to use for td tag with right adjustment. Default: "wr_cell_right"
    /// </summary>
    public string CssTdRight { get; set; }

    /// <summary>
    /// Css class to use for td tag with right adjustment. Default: "wr_cell_center"
    /// </summary>
    public string CssTdCenter { get; set; }


    /// <summary>
    /// Css class to use for td tag with left adjustment. Default: "wr_cell"
    /// </summary>
    public string CssTdLeftAlt { get; set; }

    /// <summary>
    /// Css class to use for td tag with right adjustment. Default: "wr_cell_right"
    /// </summary>
    public string CssTdRightAlt { get; set; }

    /// <summary>
    /// Css class to use for td tag with right adjustment. Default: "wr_cell_center"
    /// </summary>
    public string CssTdCenterAlt { get; set; }


    /// <summary>
    /// Css class for code. Default: null. Use "prettyprint" to use Google prettify for synthax highlighting. See https://github.com/google/code-prettify.
    /// </summary>
    public string CssCode { get; set; }


    /// <summary>
    /// Add a table of content to the document
    /// </summary>
    public bool AddTableOfContent { get; set; }

    /// <summary>
    /// Level of headings included in table of content. Default: 2
    /// </summary>
    public int TocLevel { get; set; }

    /// <summary>
    /// Caption for the table of content. Default: Table of content
    /// </summary>
    public string TocCaption { get; set; }


    /// <summary>
    /// Structured text to format
    /// </summary>
    public IStructuredText StructuredText { get; set; }


    /// <inheritdoc />
    /// <summary>
    /// Template to place the structered text. Must contain placeholder {0} for the structured text
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    /// Move header levels in HTML to new level: if 0 nothing happens. If 1 then H1 migrates to H2, H2 to H3 and so on. If 2 then H1 migrates to H3, H2 to H4 and so on.
    /// Use this feature is you want to add the created HTML in an external HTML base, where H1 is used already.
    /// 
    /// </summary>
    public int MoveHeaderLevel { get; set; }
    
    /// <inheritdoc />
    public string Title { get; set; }

    /// <inheritdoc />
    public string DateString { get; set; }

    /// <inheritdoc />
    public string GetFormattedText()
    {
        var title = string.IsNullOrEmpty(Title) ? "Missing title" : Title;

        var dateString = string.IsNullOrEmpty(DateString) ? string.Empty : DateString;

        _rowCount = -1;


        if (AddTableOfContent)
        {
            AddTocToHtm();
        }
        else
        {
            TextItems = StructuredText.TextItems.ToList();
        }

        return string.IsNullOrEmpty(Template) ? GetBody(TextItems) :
            Template.Replace("{0}", GetBody(TextItems)).Replace("{1}", WebUtility.HtmlEncode(title)).Replace("{2}", WebUtility.HtmlEncode(dateString));
    }


    private void AddTocToHtm()
    {

        TextItems.Add(new TextItem
        {
            Content = TocCaption,
            LogicalType = TextItemType.H1
        });

        foreach (var ti in StructuredText.TextItems)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (ti.LogicalType)
            {
                case TextItemType.H1:
                    if (TocLevel > 0) { _rowCount++; }
                    break;
                case TextItemType.H2:
                    if (TocLevel > 1) _rowCount++;
                    break;
                case TextItemType.H3:
                    if (TocLevel > 2) _rowCount++;
                    break;
                case TextItemType.H4:
                    if (TocLevel > 3) _rowCount++;
                    break;
            }
        }

        _rowCount++;

        var row = -1;

        foreach (var ti in StructuredText.TextItems)
        {
            row++;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (ti.LogicalType)
            {
                case TextItemType.H1:
                    if (TocLevel > 0)
                    {
                        //Debug.Print("<p class=\"{2}\"><a href=\"#H{1}\">{0}</a></p>\r\n",
                        //    ti.Content, _rowCount,
                        //    CssToc1);
                        TextItems.Add(new TextItem
                        {
                            Content = string.Format("<p class=\"{2}\"><a href=\"#H{1}\">{0}</a></p>\r\n",
                                WebUtility.HtmlEncode(ti.Content), row + _rowCount + 1,
                                CssToc1),
                            LogicalType = TextItemType.Plain
                        });

                    }

                    break;
                case TextItemType.H2:
                    if (TocLevel > 1) TextItems.Add(new TextItem
                    {
                        Content = string.Format("<p class=\"{2}\"><a href=\"#H{1}\">{0}</a></p>\r\n",
                            WebUtility.HtmlEncode(ti.Content), row + _rowCount + 1,
                            CssToc2),
                        LogicalType = TextItemType.Plain
                    });
                    break;
                case TextItemType.H3:
                    if (TocLevel > 2) TextItems.Add(new TextItem
                    {
                        Content = string.Format("<p class=\"{2}\"><a href=\"#H{1}\">{0}</a></p>\r\n",
                            WebUtility.HtmlEncode(ti.Content), row + _rowCount + 1,
                            CssToc3),
                        LogicalType = TextItemType.Plain
                    });
                    break;
                case TextItemType.H4:
                    if (TocLevel > 3) TextItems.Add(new TextItem
                    {
                        Content = string.Format("<p class=\"{2}\"><a href=\"#H{1}\">{0}</a></p>\r\n",
                            WebUtility.HtmlEncode(ti.Content), row + _rowCount + 1,
                            CssToc4),
                        LogicalType = TextItemType.Plain
                    });
                    break;
            }
        }

        foreach (var ti in StructuredText.TextItems)
        {
            TextItems.Add(ti);
        }
    }

    private string GetBody(IList<ITextItem> body)
    {
        var erg = new StringBuilder();
        var count = body.Count - 1;

        var codeClass = string.IsNullOrEmpty(CssCode) ? string.Empty : $" class=\"{CssCode}\"";

        var row = -1;

        // Add a title
        if (!string.IsNullOrEmpty(Title))
        {
            erg.AppendFormat("<p class=\"{1}\">{0}</p>", WebUtility.HtmlEncode(Title), CssTitle);
        }

        if (!string.IsNullOrEmpty(DateString))
        {
            erg.Append($"<p>{WebUtility.HtmlEncode(DateString)}</p>");
        }

        // Add rest of text
        foreach (var ti in body)
        {

            row++;

            var className = string.IsNullOrEmpty(ti.ClassName) ? string.Empty : $" class=\"{ti.ClassName}\"";

            var content = HtmlHelper.GetContentAsHtml(ti.Content);

            string id;

            switch (ti.LogicalType)
            {
                case TextItemType.H1:

                    id = AddTableOfContent && TocLevel > 0 ? $"id=\"H{row}\"" : string.Empty;
                    erg.AppendFormat("<h{2:0} {1}{3}>{0}</h{2:0}>\r\n", content, id, MoveHeaderLevel + 1, className);
                    break;
                case TextItemType.H2:
                    id = AddTableOfContent && TocLevel > 1 ? $"id=\"H{row}\"" : string.Empty;
                    erg.AppendFormat("<h{2:0} {1}{3}>{0}</h{2:0}>\r\n", content, id, MoveHeaderLevel + 2, className);
                    break;
                case TextItemType.H3:
                    id = AddTableOfContent && TocLevel > 2 ? $"id=\"H{row}\"" : string.Empty;
                    erg.AppendFormat("<h{2:0} {1}{3}>{0}</h{2:0}>\r\n", content, id, MoveHeaderLevel + 3, className);
                    break;
                case TextItemType.H4:
                    id = AddTableOfContent && TocLevel > 3 ? $"id=\"H{row}\"" : string.Empty;
                    erg.AppendFormat("<h{2:0} {1}{3}>{0}</h{2:0}>\r\n", content, id, MoveHeaderLevel + 4, className);
                    break;
                case TextItemType.ListItem:
                    if (row == 0) // 1. Zeile: starte UL vorher
                    {
                        erg.Append("<ul>\r\n");
                    }
                    else
                    {
                        var beforeItem = body[row - 1];
                        if (beforeItem.LogicalType != TextItemType.ListItem)
                        {
                            erg.Append("<ul>\r\n");
                        }
                    }

                    erg.AppendFormat("<li{1}>{0}</li>\r\n", content, className);

                    if (row == count) // Letzte Zeile: beende UL nachher
                    {
                        erg.Append("</ul>\r\n");
                    }
                    else
                    {
                        var nextItem = body[row + 1];
                        if (nextItem.LogicalType != TextItemType.ListItem)
                        {
                            erg.Append("</ul>\r\n");
                        }
                    }
                    break;

                case TextItemType.DefinitionListLine:

                    var dl = (DefinitionListTextItem)ti;

                    if (row == 0) // 1. Zeile: starte DL vorher
                    {
                        erg.Append("<dl>\r\n");
                    }
                    else
                    {
                        var beforeItem = body[row - 1];
                        if (beforeItem.LogicalType != TextItemType.DefinitionListLine)
                        {
                            erg.Append("<dl>\r\n");
                        }
                    }

                    var content2 = HtmlHelper.GetContentAsHtmlParagraph(dl.Content2);

                    erg.Append($"<dt>{content}</dt><dd>{content2}</dd>\r\n");

                    if (row == count) // Letzte Zeile: beende DL nachher
                    {
                        erg.Append("</dl>\r\n");
                    }
                    else
                    {
                        var nextItem = body[row + 1];
                        if (nextItem.LogicalType != TextItemType.DefinitionListLine)
                        {
                            erg.Append("</dl>\r\n");
                        }
                    }

                    break;
                case TextItemType.Xml:
                    if (string.IsNullOrEmpty(ti.Content)) continue;

                    var c = BeautifyXml(ti.Content);

                    // c = "Hallo";

                    erg.Append($"<div class=\"code\"><code{className}>{c}</code></div>\r\n");
                    break;
                case TextItemType.Code:

                    var localCodeClass = string.IsNullOrEmpty(className) ? $" {codeClass}" : className;

                    erg.Append($"<div class=\"code\"><code{localCodeClass}>{content.Replace("??pa??", "<p>")
                        .Replace("??pe??", "</p>")
                        .Replace("</p>\r\n", "</p>")}</code></div>\r\n");
                    break;
                case TextItemType.Plain:
                    erg.Append(ti.Content);
                    break;
                case TextItemType.Table:
                    erg.Append(GetTable((TableTextItem)ti));
                    break;
                default:
                    content = HtmlHelper.GetContentAsHtmlParagraph(ti.Content, className);
                    erg.Append($"{content}\r\n");
                    break;
            }
        }

        return erg.ToString();
    }

    private StringBuilder GetTable(TableTextItem ti)
    {
        var data = new DataTable();

        using (var stream = new MemoryStream())
        {
            var writer = new StreamWriter(stream);
            writer.Write(ti.DataTableXml);
            writer.Flush();
            stream.Position = 0;
            data.ReadXml(stream);
        }

        var erg = new StringBuilder();
        string css;

        erg.Append($"<table class=\"{CssTable}\">\r\n");

        if (!string.IsNullOrEmpty(ti.Content))
        {
            erg.Append($"<caption>{ti.Content}</caption>\r\n");
        }

        erg.Append("<tr>\r\n");
        foreach (DataColumn col in data.Columns)
        {

            string s;

            css = CssThLeft;

            var dt = col.DataType.ToString().ToLower().Replace("system.", string.Empty);

            switch (dt)
            {
                case "boolean":
                    css = CssThCenter;
                    s = col.ColumnName;
                    break;
                case "char":
                    s = col.ColumnName;
                    break;
                //case "sbyte":

                //    break;
                case "decimal":
                case "double":
                case "single":
                    css = CssThRight;
                    s = col.ColumnName;
                    break;
                case "datetime":
                case "timespan":
                    css = CssThCenter;
                    s = col.ColumnName;
                    break;

                case "byte":
                case "int16":
                case "int32":
                case "int64":
                case "uint16":
                case "uint32":
                case "uint64":
                    css = CssThRight;
                    s = col.ColumnName;
                    break;
                default:
                    s = col.ColumnName;
                    break;
            }

            erg.AppendFormat("<th class=\"{1}\">{0}</th>\r\n", s, css);
        }
        erg.Append("</tr>\r\n");

        var alternateRow = false;

        foreach (DataRow row in data.Rows)
        {

            erg.Append("<tr>\r\n");

            foreach (DataColumn col in data.Columns)
            {

                string s;
                css = alternateRow ? CssTdLeftAlt : CssTdLeft;

                var dt = col.DataType.ToString().ToLower().Replace("system.", string.Empty);

                switch (dt)
                {
                    case "boolean":
                        css = alternateRow ? CssTdCenterAlt : CssTdCenter;
                        s = row[col.ColumnName].ToString();
                        break;
                    case "char":
                        s = col.ColumnName;
                        break;
                    //case "sbyte":

                    //    break;
                    case "decimal":
                    case "double":
                    case "single":
                        css = alternateRow ? CssTdRightAlt : CssTdRight;
                        var nValue = Convert.ToDecimal(row[col.ColumnName]);
                        s = $"{nValue:#,##0.00}";
                        break;
                    case "datetime":
                        //case "timespan":
                        css = alternateRow ? CssTdCenterAlt : CssTdCenter;
                        var dValue = Convert.ToDateTime(row[col.ColumnName]);
                        s = $"{dValue:d}";
                        break;

                    case "byte":
                    case "int16":
                    case "int32":
                    case "int64":
                    case "uint16":
                    case "uint32":
                    case "uint64":
                        css = alternateRow ? CssTdRightAlt : CssTdRight;
                        var iValue = Convert.ToInt64(row[col.ColumnName]);
                        s = $"{iValue:#,##0}";
                        break;
                    default:

                        s = row[col.ColumnName].ToString();
                        break;
                }

                erg.AppendFormat("<td class=\"{1}\">{0}</td>\r\n", s, css);
            }

            erg.Append("</tr>\r\n");
            alternateRow = !alternateRow;
        }

        erg.Append("</table>\r\n");
        return erg;
    }

    private static string BeautifyXml(string xml)
    {
        try
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t\t",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (var writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            var html = WebUtility.HtmlEncode(sb.ToString()).Replace("\r\n", "<br />").Replace("\t", "&nbsp;&nbsp;&nbsp;").Replace("{", "{{").Replace("}", "}}");

            return html;
        }
        catch
        {
            return WebUtility.HtmlEncode(xml);
        }
    }
}