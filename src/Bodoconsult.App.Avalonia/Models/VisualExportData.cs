// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Avalonia;
using Bodoconsult.App.Avalonia.Helpers;

namespace Bodoconsult.App.Avalonia.Models;

/// <summary>
/// Data needed for exporting a chart as image file
/// </summary>
//    [AddINotifyPropertyChangedInterface]
public class VisualExportData
{

    /// <summary>
    ///  default ctor
    /// </summary>
    public VisualExportData()
    {
        ImageFormat = AvaloniaHelper.ImageFormat.Png;
        Width = 1024;
        Height = 768;
    }

    /// <summary>
    /// Visual to export as file
    /// </summary>
    public Visual? Visual { get; set; }

    /// <summary>
    /// Image format for the chart export
    /// </summary>
    public AvaloniaHelper.ImageFormat ImageFormat { get; set; }

    /// <summary>
    /// path to save the exported chart
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Width in pixels of the exported chart. Default: 1024px
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Height in pixels of the exported chart. Default: 768px
    /// </summary>
    public int Height { get; set; }
}