// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Extensions;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Renderer.Html;

/// <summary>
/// HTML rendering element for <see cref="Styleset"/> instances
/// </summary>
public class StylesetHtmlTextRendererElement : HtmlTextRendererElementBase
{
    private readonly Styleset _styleset;

    /// <summary>
    /// Default ctor
    /// </summary>
    public StylesetHtmlTextRendererElement(Styleset styleset) : base(styleset)
    {
        _styleset = styleset;
        ClassName = styleset.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {

        var wStyle = (ParagraphStyleBase)renderer.Styleset.FindStyle("WatermarkStyle");

        renderer.Content.AppendLine("<style>");

        renderer.Content.AppendLine("#watermark {");
        renderer.Content.AppendLine("     position: fixed;");
        renderer.Content.AppendLine("     z-index:1;");
        renderer.Content.AppendLine("     top:100px;");
        renderer.Content.AppendLine("     left:100px;");
        renderer.Content.AppendLine("     transform: translate(-100px, -100px);");
        renderer.Content.AppendLine("     align-items: center;");
        renderer.Content.AppendLine("     justify-content: center;");
        renderer.Content.AppendLine("     opacity: 0.2;");
        renderer.Content.AppendLine($"     font-size: {wStyle.FontSize}pt;");
        renderer.Content.AppendLine($"     color: {wStyle.FontColor.ToHtml()}");
        renderer.Content.AppendLine("     background: '#ccc'");
        renderer.Content.AppendLine("     cursor: default;");
        renderer.Content.AppendLine("     user-select: none;");
        renderer.Content.AppendLine("     -webkit-user-select: none;");
        renderer.Content.AppendLine("     -khtml-user-select: none;");
        renderer.Content.AppendLine("     -moz-user-select: none;");
        renderer.Content.AppendLine("     -ms-user-select: none;");
        renderer.Content.AppendLine("     -webkit-transform: rotate(331deg);");
        renderer.Content.AppendLine("     -moz-transform: rotate(331deg);");
        renderer.Content.AppendLine("     -o-transform: rotate(331deg);");
        renderer.Content.AppendLine("     transform: rotate(331deg);");
        renderer.Content.AppendLine("}");


        string fileName;

        var sb = new StringBuilder();

        foreach (var style in _styleset.StyleDictionary.Values)
        {
            var rendererElement = renderer.TextRendererElementFactory.CreateInstance(style);
            rendererElement.RenderIt(renderer);
        }
        renderer.Content.Append(sb);

        renderer.Content.AppendLine("</style>");
        renderer.Content.AppendLine("</head>");

        var md = renderer.Document.DocumentMetaData;

        if (string.IsNullOrEmpty(md.BackgroundImagePath))
        {
            renderer.Content.AppendLine("<body style=\"background-color: #ffffff;\">");
        }
        else
        {
            fileName = renderer.RegisterImage(md.BackgroundImagePath);
            if (string.IsNullOrEmpty(fileName))
            {
                renderer.Content.AppendLine("<body style=\"background-color: #ffffff;\">");
            }
            else
            {
                renderer.Content.AppendLine($"<body style=\"background-image: url('{fileName}');\">");
            }
        }

        var watermark = renderer.Document.DocumentMetaData.WatermarkText;

        if (!string.IsNullOrEmpty(watermark))
        {
            renderer.Content.AppendLine($"<p id=\"watermark\">{watermark}</p>");

        }

        var logoPath = renderer.Document.DocumentMetaData.LogoPath;

        if (string.IsNullOrEmpty(logoPath))
        {
            return;
        }

        fileName = renderer.RegisterImage(logoPath);
        if (!string.IsNullOrEmpty(fileName))
        {
            renderer.Content.AppendLine($"<p style=\"text-align: right;\"><img src=\"{fileName}\" style=\"width: {MeasurementHelper.GetPxFromCm(renderer.Document.DocumentMetaData.LogoWidth)}px;\"></p>");
        }

    }
}