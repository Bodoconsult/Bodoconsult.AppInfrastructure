// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Diagnostics;
using System.Text;
using Bodoconsult.App.Abstractions.Helpers;

namespace Bodoconsult.Text.Renderer.Rtf.Blocks;

/// <summary>
/// Rtf rendering element for <see cref="Document"/> instances
/// </summary>
public class DocumentRtfTextRendererElement : RtfTextRendererElementBase
{
    private readonly Document _document;

    /// <summary>
    /// Default ctor
    /// </summary>
    public DocumentRtfTextRendererElement(Document document) : base(document)
    {
        _document = document;
        ClassName = document.StyleName;
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(ITextDocumentRenderer renderer)
    {

        var md = renderer.Document.DocumentMetaData;

        renderer.Content.AppendLine(@"{\rtf1\ansi\deff0");


        //var imagePath = md.BackgroundImagePath;
        //if (!string.IsNullOrEmpty(imagePath))
        //{

        //    var width = MeasurementHelper.GetPxFromCm(renderer.PageStyleBase.PaperFormat.Size.Width);
        //    var height = MeasurementHelper.GetPxFromCm(renderer.PageStyleBase.PaperFormat.Size.Height);

        //    var sb = new StringBuilder();

        //    sb.Append($"{{\\*\\background\\shp\\shpleft0\\shptop0\\shpbottom{height}\\shpright{width}\\shpfhdr1\\shpbxpage{{");



        //    var image = new Image
        //    {
        //        Uri = imagePath
        //    };

        //    // Add the image
        //    var bytes = ImageHelper.GetBytes(image.Uri);

        //    var path = image.Uri.ToLowerInvariant();

        //    sb.Append(@"{{\*\shppict\pict");

        //    if (path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
        //    {
        //        sb.Append("\\jpegblip");
        //    }
        //    else if (path.EndsWith(".png"))
        //    {
        //        sb.Append("\\pngblip");
        //    }
        //    else
        //    {
        //        throw new NotSupportedException("Unsupported image format. Use JPEG or PNG images!");
        //    }

        //    sb.Append($@"\picw{width}\pich{height}\picwgoal{width}\pichgoal{height}\bin{{");

        //    var str = BitConverter.ToString(bytes, 0).Replace("-", string.Empty);
        //    sb.Append(str);
        //    sb.Append("}}");


        //    sb.AppendLine("}}}");

        //    Debug.Print((sb.ToString()));
        //    renderer.Content.Append(sb);
        //}



        DocumentRendererHelper.RenderBlockChildsToRtf(renderer, _document.ChildBlocks);
        renderer.Content.AppendLine("}");

        // Some general fixes to be applied
        renderer.Content.Replace("\\par\r\n}\\cell", "}\\cell");
        renderer.Content.Replace("\\par}\\cell", "}\\cell");

    }
}