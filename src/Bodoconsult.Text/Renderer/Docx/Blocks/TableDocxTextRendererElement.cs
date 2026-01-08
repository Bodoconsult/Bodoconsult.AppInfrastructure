// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Office;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using Bodoconsult.App.Abstractions.Interfaces;
using Table = Bodoconsult.Text.Documents.Table;

namespace Bodoconsult.Text.Renderer.Docx.Blocks;

/// <summary>
/// Docx rendering element for <see cref="Documents.Table"/> instances
/// </summary>
public class TableDocxTextRendererElement : DocxTextRendererElementBase
{
    private readonly Table _table;

    /// <summary>
    /// Default ctor
    /// </summary>
    public TableDocxTextRendererElement(Table table) : base(table)
    {
        _table = table;
        ClassName = "TableStyle";
    }


    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(DocxTextDocumentRenderer renderer)
    {

        var style = renderer.Styleset.FindStyle(_table.StyleName);

        List<OpenXmlElement> runs;
        var rows = new List<DocxTableRow>();

        foreach (var row in _table.Rows)
        {

            var dataRow = new DocxTableRow();

            foreach (var cell in row.Cells)
            {
                var dataCell = new DocxTableCell();

                runs = [];
                DocxDocumentRendererHelper.RenderBlockInlinesToRunsForDocx(renderer, cell.ChildInlines, runs);
                dataCell.Items.AddRange(runs);

                dataCell.StyleId = GetStyleName(cell);

                dataRow.Cells.Add(dataCell);
            }

            rows.Add(dataRow);

        }

        renderer.DocxDocument.AddTable(rows, (ITypoTableStyle)style, _table.TagName);

        // Legend
        var childs = new List<Inline>();
        if (!string.IsNullOrEmpty(_table.CurrentPrefix))
        {
            childs.Add(new Span(_table.CurrentPrefix));
        }
        childs.AddRange(_table.ChildInlines);
        runs = [];
        DocxDocumentRendererHelper.RenderBlockInlinesToRunsForDocx(renderer, childs, runs);
        renderer.DocxDocument.AddParagraph(runs, "TableLegend");


        //DocxDocumentRendererHelper.RenderBlockInlinesToStringForDocx(renderer, childs, sb);
        //var legend = sb.ToString();

        //foreach (var column in _table.Columns)
        //{
        //    var col = new DocxColumn(column.Name);

        //    switch (DocumentRendererHelper.GetAlignment(column.DataType))
        //    {
        //        case "Center":
        //            col.TextAlignment = DocxTextAlignment.Center;
        //            break;
        //        case "Right":
        //            col.TextAlignment = DocxTextAlignment.Right;
        //            break;
        //        default:
        //            col.TextAlignment = DocxTextAlignment.Left;
        //            break;
        //    }

        //    col.MaxLength = column.MaxLength;

        //    dt.Columns.Add(col);
        //}


        //foreach (var dataRow in _table.Rows)
        //{
        //    var row = new DocxRow();


        //    foreach (var cell in dataRow.Cells)
        //    {
        //        sb.Clear();
        //        DocxDocumentRendererHelper.RenderBlockInlinesToStringForDocx(renderer, cell.ChildInlines, sb);

        //        var DocxCell = new DocxCell(sb.ToString());
        //        row.Cells.Add(DocxCell);
        //    }

        //    dt.Rows.Add(row);
        //}

        //renderer.DocxDocument.AddTable(dt, legend, _table.TagName);
    }

    private string GetStyleName(Documents.Cell cell)
    {
        if (cell.Column.DataType == typeof(double))
        {
            return "CellRight";
        }
        if (cell.Column.DataType == typeof(float))
        {
            return "CellRight";
        }
        if (cell.Column.DataType == typeof(short))
        {
            return "CellRight";
        }
        if (cell.Column.DataType == typeof(int))
        {
            return "CellRight";
        }
        if (cell.Column.DataType == typeof(long))
        {
            return "CellRight";
        }
        if (cell.Column.DataType == typeof(Int128))
        {
            return "CellRight";
        }
        if (cell.Column.DataType == typeof(byte))
        {
            return "CellRight";
        }
        if (cell.Column.DataType == typeof(bool))
        {
            return "CellCenter";
        }
        if (cell.Column.DataType == typeof(DateTime))
        {
            return "CellCenter";
        }

        // ToDo: Other alignments
        return "CellLeft";
    }
}