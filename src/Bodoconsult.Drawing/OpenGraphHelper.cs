// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Runtime.Versioning;

namespace Bodoconsult.Drawing;

/// <summary>
/// Creates OpenGraph images based on a template image
/// </summary>
[SupportedOSPlatform("windows")]
public static class OpenGraphHelper
{

    /// <summary>
    /// Filename of the OpenGraph image
    /// </summary>
    public const string OpenGraphFileName = @"opengraph.png";

    /// <summary>
    /// Header
    /// </summary>
    public static ImageWriterData Header { get; set; } = new()
    {
        FontName = "Aptos",
        FontSize = 16,
        X = 20,
        Y = 580,
        Width = 1650,
        Height = 70,
        Text = "",
        FontStyle = 1
    };

    /// <summary>
    /// Body
    /// </summary>
    public static ImageWriterData Body { get; set; } = new()
    {
        FontName = "Aptos",
        FontSize = 10,
        X = 20,
        Y = 660,
        Width = 1650,
        Height = 230,
        Text = ""
    };

    /// <summary>
    /// Source file
    /// </summary>
    public static string SourceFile
    {
        get => OpenGraphImageWriter.SourceFile;
        set => OpenGraphImageWriter.SourceFile = value;
    }


    private static ImageWriter OpenGraphImageWriter { get; } = new();

    /// <summary>
    /// Save the image as 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="fileName"></param>
    public static void Save(string title, string description, string fileName)
    {
        Header.Text = title;
        Body.Text = description;

        OpenGraphImageWriter.TargetFile = fileName;
        OpenGraphImageWriter.NewImage();
        OpenGraphImageWriter.WriteText(Header);
        OpenGraphImageWriter.WriteText(Body);
        OpenGraphImageWriter.Save();
    }
}