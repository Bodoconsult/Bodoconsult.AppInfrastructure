// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Documents;

namespace Bodoconsult.Text.Renderer.Pdf.Blocks;

/// <summary>
/// PDF rendering element for <see cref="List"/> instances
/// </summary>
public class ListPdfTextRendererElement : PdfTextRendererElementBase
{
    private readonly List _list;

    /// <summary>
    /// Default ctor
    /// </summary>
    public ListPdfTextRendererElement(List list) : base(list)
    {
        _list = list;
        ClassName = list.StyleName;

        LocalCss = list.ListStyleType switch
        {
            ListStyleTypeEnum.Disc => "list-style-type: disc",
            ListStyleTypeEnum.Circle => "list-style-type: circle",
            ListStyleTypeEnum.Square => "list-style-type: square",
            ListStyleTypeEnum.Customized => $"list-style-type: '{_list.ListStyleTypeChar}'",
            ListStyleTypeEnum.Decimal => "list-style-type: decimal",
            ListStyleTypeEnum.DecimalLeadingZero => "list-style-type: decimal-leading-zero",
            ListStyleTypeEnum.UpperRoman => "list-style-type: upper-roman",
            ListStyleTypeEnum.LowerRoman => "list-style-type: lower-roman",
            ListStyleTypeEnum.UpperLatin => "list-style-type: upper-latin",
            ListStyleTypeEnum.LowerLatin => "list-style-type: lower-latin",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}