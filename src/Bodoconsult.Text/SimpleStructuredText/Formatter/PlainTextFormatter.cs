// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Bodoconsult.Text.Enums;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Model;

namespace Bodoconsult.Text.Formatter;

/// <summary>
/// Formatting <see cref="IStructuredText"/> to plain text
/// </summary>
public class PlainTextFormatter : ITextFormatter
{
    /// <summary>
    /// Structured text to format
    /// </summary>
    public IStructuredText StructuredText { get; set; }


    /// <summary>
    /// Template to place the structered text. Must contain placeholder {0} for the structured text
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    /// Title for document
    /// </summary>
    public string Title { get; set; }

    /// <inheritdoc />
    public string DateString { get; set; }


    /// <inheritdoc />
    public string GetFormattedText()
    {
        return string.IsNullOrEmpty(Template) ? GetBody(StructuredText.TextItems) : Template.Replace("{0}", GetBody(StructuredText.TextItems));
    }


    private string GetBody(IEnumerable<ITextItem> body)
    {
        var erg = new StringBuilder();

        // Add a title
        if (!string.IsNullOrEmpty(Title))
        {
            erg.AppendFormat("\r\n\r\n\r\n******************************\r\n" +
                             "******************************\r\n{0}\r\n" +
                             "******************************\r\n", Title);
        }

        if (!string.IsNullOrEmpty(DateString))
        {
            erg.AppendFormat("\r\n{0}\r\n", WebUtility.HtmlEncode(DateString));
        }

        // Add rest of text
        foreach (var ti in body)
        {

            var content = ti.Content.Replace("??br??", "\r\n");

            switch (ti.LogicalType)
            {
                case TextItemType.H1:
                    erg.AppendFormat("\r\n\r\n\r\n***************\r\n{0}\r\n***************\r\n", content);

                    break;
                case TextItemType.H2:
                    erg.AppendFormat("\r\n\r\n************\r\n{0}\r\n************\r\n", content);
                    break;
                case TextItemType.H3:
                    erg.AppendFormat("\r\n\r\n******\r\n{0}", content);
                    break;
                case TextItemType.H4:
                    erg.AppendFormat("\r\n\r\n***\r\n{0}", content);
                    break;
                case TextItemType.ListItem:
                    erg.AppendFormat("  - {0}", content);
                    break;
                case TextItemType.DefinitionListLine:

                    var dl = (DefinitionListTextItem)ti;

                    erg.AppendFormat("\r\n{0} {1}", dl.Content.PadRight(25), dl.Content2?.Replace("<para>", string.Empty).Replace("</para>", string.Empty));

                    break;
                case TextItemType.Xml:
                    erg.Append(BeautifyXml(content));
                    break;
                case TextItemType.Code:
                    erg.AppendFormat("\r\n{0}\r\n", GetCode(content));
                    break;
                case TextItemType.Table:
                    erg.Append(GetTable((TableTextItem)ti));
                    break;
                default:
                    erg.Append($"\r\n{content.Replace("??empty??", "\r\n")}");
                    break;
            }
        }

        erg.Append($"\r\n{DateString}");

        return erg.ToString();
    }

    private StringBuilder GetTable(TableTextItem ti)
    {

        var erg = new StringBuilder();

        var data = new DataTable();

        using (var stream = new MemoryStream())
        {
            var writer = new StreamWriter(stream);
            writer.Write(ti.DataTableXml);
            writer.Flush();
            stream.Position = 0;
            data.ReadXml(stream);
        }

        foreach (DataColumn col in data.Columns)
        {

            switch (col.DataType.ToString().ToLower())
            {


                case "boolean":
                    erg.Append(col.ColumnName);
                    break;
                case "char":
                    var ml = col.MaxLength;

                    erg.Append(col.ColumnName.PadRight(ml));
                    break;
                //case "sbyte":

                //    break;
                case "decimal":
                case "double":
                case "single":
                    erg.Append(col.ColumnName.PadLeft(15));
                    break;
                case "datetime":
                case "timespan":
                    erg.Append(col.ColumnName.PadLeft(15));
                    break;

                case "byte":
                case "int16":
                case "int32":
                case "int64":
                case "uint16":
                case "uint32":
                case "uint64":
                    erg.Append(col.ColumnName.PadLeft(10));
                    break;
                default:

                    erg.Append(col.ColumnName.PadRight(15));
                    break;
            }


        }
        erg.Append("\r\n");

        foreach (DataRow row in data.Rows)
        {

            foreach (DataColumn col in data.Columns)
            {
                switch (col.DataType.ToString().ToLower())
                {
                    case "boolean":
                        erg.Append(row[col.ColumnName]);
                        break;
                    case "char":
                        var ml = col.MaxLength;

                        erg.Append(col.ColumnName.PadRight(ml));
                        break;
                    //case "sbyte":

                    //    break;
                    case "decimal":
                    case "double":
                    case "single":
                        var nValue = Convert.ToDecimal(row[col.ColumnName]);
                        erg.Append($"{nValue:#,##0.00}".PadLeft(15));
                        break;
                    case "datetime":
                        //case "timespan":
                        var dValue = Convert.ToDateTime(row[col.ColumnName]);
                        erg.Append($"{dValue:d}".PadLeft(15));
                        break;

                    case "byte":
                    case "int16":
                    case "int32":
                    case "int64":
                    case "uint16":
                    case "uint32":
                    case "uint64":
                        var iValue = Convert.ToInt64(row[col.ColumnName]);
                        erg.Append($"{iValue:#,##0}".PadLeft(10));
                        break;
                    default:

                        erg.Append(row[col.ColumnName].ToString().PadRight(15));
                        break;
                }
            }

            erg.Append("\r\n");
        }

        return erg;
    }

    /// <summary>
    /// Get formatted code
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private static string GetCode(string content)
    {
        var erg = new StringBuilder();

        var data = content.Split(["\r\n"], StringSplitOptions.None);
        foreach (var row in data)
        {
            erg.AppendFormat("\r\n    {0}", row);
        }

        return erg.ToString();
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
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (var writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }
        catch
        {
            return xml;
        }
    }
}