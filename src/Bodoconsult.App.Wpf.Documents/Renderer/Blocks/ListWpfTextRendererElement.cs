// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Windows;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Documents.Helpers;
using Bodoconsult.Text.Documents;
using ListItem = System.Windows.Documents.ListItem;
using Paragraph = System.Windows.Documents.Paragraph;
using Thickness = System.Windows.Thickness;

namespace Bodoconsult.App.Wpf.Documents.Renderer.Blocks;

/// <summary>
/// WPF rendering element for <see cref="List"/> instances
/// </summary>
public class ListWpfTextRendererElement : WpfTextRendererElementBase
{
    private readonly List _list;

    private readonly TextMarkerStyle _markerStyle;

    /// <summary>
    /// Default ctor
    /// </summary>
    public ListWpfTextRendererElement(List list) : base(list)
    {
        _list = list;
        ClassName = list.StyleName;

        _markerStyle = list.ListStyleType switch
        {
            ListStyleTypeEnum.Disc => TextMarkerStyle.Disc,
            ListStyleTypeEnum.Circle => TextMarkerStyle.Circle,
            ListStyleTypeEnum.Square => TextMarkerStyle.Square,
            ListStyleTypeEnum.Customized => TextMarkerStyle.Disc,
            ListStyleTypeEnum.Decimal => TextMarkerStyle.Decimal,
            ListStyleTypeEnum.DecimalLeadingZero => TextMarkerStyle.Decimal,
            ListStyleTypeEnum.UpperRoman => TextMarkerStyle.UpperRoman,
            ListStyleTypeEnum.LowerRoman => TextMarkerStyle.LowerRoman,
            ListStyleTypeEnum.UpperLatin => TextMarkerStyle.UpperLatin,
            ListStyleTypeEnum.LowerLatin => TextMarkerStyle.LowerLatin,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Render the element
    /// </summary>
    /// <param name="renderer">Current renderer</param>
    public override void RenderIt(WpfTextDocumentRenderer renderer)
    {
        renderer.Dispatcher.Invoke(() =>
        {
            var itemStyle = (ParagraphStyleBase)renderer.Styleset.FindStyle("ListItemStyle");

            var list = new System.Windows.Documents.List
            {
                MarkerStyle = _markerStyle,
                Margin = WpfDocumentRendererHelper.NoMarginThickness,
                MarkerOffset = MeasurementHelper.GetDiuFromPoint(itemStyle.Margins.Left),
            };

            foreach (var item in _list.ChildBlocks)
            {
                var listItem = new ListItem
                {
                    Margin = WpfDocumentRendererHelper.NoMarginThickness
                };

                var p = new Paragraph
                {
                    Margin = new Thickness(0,
                        MeasurementHelper.GetDiuFromPoint(itemStyle.Margins.Top),
                        MeasurementHelper.GetDiuFromPoint(itemStyle.Margins.Right),
                        MeasurementHelper.GetDiuFromPoint(itemStyle.Margins.Bottom))
                };


                var style = (Style)renderer.StyleSet["ListItemStyle"];
                p.Style = style;

                WpfDocumentRendererHelper.RenderBlockInlinesToWpf(renderer, item.ChildInlines, p);

                listItem.Blocks.Add(p);

                list.ListItems.Add(listItem);
            }

            renderer.CurrentSection.Blocks.Add(list);
        });
    }
}