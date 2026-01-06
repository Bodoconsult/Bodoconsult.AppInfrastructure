// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Linq;
using System.Text;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Rtf.Blocks;

/// <summary>
/// Rtf rendering element for <see cref="DocumentMetaData"/> instances
/// </summary>
public class DocumentMetaDataRtfTextRendererElement : RtfTextRendererElementBase
{
    private readonly DocumentMetaData _documentMetaData;

    /// <summary>
    /// Default ctor
    /// </summary>
    public DocumentMetaDataRtfTextRendererElement(DocumentMetaData documentMetaData) : base(documentMetaData)
    {
        _documentMetaData = documentMetaData;
        ClassName = documentMetaData.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {
        var sb = new StringBuilder();

        

        FontsParsing(renderer.Styleset, sb);
        ColorParsing(renderer.Styleset, sb);


        sb.AppendLine("{\\info ");

        var date = DateTime.Now;
        sb.AppendLine($@"{{\creatim\yr{date.Year}\mo{date.Month}\dy{date.Day}\hr{date.Hour}\min{date.Minute}}}");
        sb.AppendLine("{\\edmins0}");
        sb.AppendLine("{\\nofpages1}");
        sb.AppendLine("{\\nofwords0}");
        sb.AppendLine("{\\nofchars0}");

        if (!string.IsNullOrEmpty(_documentMetaData.Title))
        {
            sb.AppendLine($"{{\\title {_documentMetaData.Title}}}");
        }

        if (!string.IsNullOrEmpty(_documentMetaData.Description))
        {
            sb.AppendLine($"{{\\comment {_documentMetaData.Description}}}");
        }

        if (!string.IsNullOrEmpty( _documentMetaData.Keywords))
        {
            sb.AppendLine($"{{\\keywords {_documentMetaData.Keywords}}}");
        }

        if (!string.IsNullOrEmpty(_documentMetaData.Authors))
        {
            sb.AppendLine($"{{\\author {_documentMetaData.Authors}}}");
        }

        if (!string.IsNullOrEmpty(_documentMetaData.Company))
        {
            sb.AppendLine($"{{\\company {_documentMetaData.Company}}}");
        }

        sb.AppendLine("}");


        // Basic page settings
        var style = (DocumentStyle)renderer.Styleset.FindStyle("DocumentStyle");

        sb.AppendLine(style.PaperFormat.Size.Height < style.PaperFormat.Size.Width ? "\\landscape" : "\\portrait");

        sb.AppendLine(
            $@"\paperw{MeasurementHelper.GetTwipsFromCm((float)style.PaperFormat.Size.Width)}\paperh{MeasurementHelper.GetTwipsFromCm((float)style.PaperFormat.Size.Height)}\margl{MeasurementHelper.GetTwipsFromCm((float)style.Margins.Left)}\margr{MeasurementHelper.GetTwipsFromCm((float)style.Margins.Right)}\margt{MeasurementHelper.GetTwipsFromCm((float)style.Margins.Top)}\margb{MeasurementHelper.GetTwipsFromCm((float)style.Margins.Bottom)} ");


        // Now add all to content
        renderer.Content.Append(sb);
    }

    private void ColorParsing(Styleset styleset, StringBuilder sb)
    {
        var baseType = typeof(ParagraphStyleBase);

        foreach (var style in styleset.StyleDictionary.Values.Where(x => baseType.IsAssignableFrom(x.GetType())))
        {
            if (style is not ParagraphStyleBase paragraphStyle)
            {
                continue;
            }

            CheckColor(styleset, paragraphStyle.FontColor);

            if (paragraphStyle.BorderBrush == null)
            {
                continue;
            }
            CheckColor(styleset, paragraphStyle.BorderBrush.TypoColor);
            
        }

        var tableStyle = (TableStyle)styleset.FindStyle("TableStyle");

        CheckColor(styleset, tableStyle.TableHeaderBackgroundColor);
        CheckColor(styleset, tableStyle.TableBackColor);
        CheckColor(styleset, tableStyle.TableAlternateBackColor);
        CheckColor(styleset, tableStyle.TableBorderColor);

        sb.AppendLine("{\\colortbl;");

        foreach (var color in styleset.Colors)
        {
            sb.AppendLine($@"\red{color.R}\green{color.G}\blue{color.B};");
        }

        sb.AppendLine("}");
    }

    private static void CheckColor(Styleset styleset, TypoColor colorTable)
    {
        if (!styleset.Colors.Exists(x => x.R == colorTable.R &&
                                         x.G == colorTable.G &&
                                         x.B == colorTable.B &&
                                         x.A == colorTable.A))
        {
            styleset.Colors.Add(colorTable);
        }
    }

    private void FontsParsing(Styleset styleset, StringBuilder sb)
    {

        var baseType = typeof(ParagraphStyleBase);

        foreach (var style in styleset.StyleDictionary.Values.Where(x => baseType.IsAssignableFrom(x.GetType())))
        {
            if (style is not ParagraphStyleBase paragraphStyle)
            {
                continue;
            }

            if (styleset.Fonts.Exists(x => x == paragraphStyle.FontName))
            {
                continue;
            }

            styleset.Fonts.Add(paragraphStyle.FontName);
        }

        sb.AppendLine("{\\fonttbl");

        for (var i = 0; i < styleset.Fonts.Count; i++)
        {
            sb.AppendLine($"{{\\f{i} {styleset.Fonts[i]};}}");
        }

        sb.AppendLine("}");
    }
}