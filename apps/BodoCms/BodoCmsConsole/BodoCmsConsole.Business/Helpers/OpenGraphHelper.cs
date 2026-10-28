// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace BodoCmsConsole.Business.Helpers;

//public static class OpenGraphHelper
//{

//    public const string OpenGraphFileName = @"opengraph.png";

//    public static ImageWriterData Header { get; set; } = new ImageWriterData
//    {
//        FontName = "Arial",
//        FontSize = 16,
//        X = 20,
//        Y = 580,
//        Width = 1650,
//        Height = 70,
//        Text = "",
//        FontStyle = 1
//    };

//    public static ImageWriterData Body { get; set; } = new ImageWriterData
//    {
//        FontName = "Arial",
//        FontSize = 10,
//        X = 20,
//        Y = 660,
//        Width = 1650,
//        Height = 230,
//        Text = ""
//    };


//    public static string OpenGraphImage { get; set; }


//    public static ImageWriter OpenGraphImageWriter { get; } = new ImageWriter();


//    public static void Save(string title, string description, string fileName)
//    {
//        Header.Text = title;
//        Body.Text = description;

//        OpenGraphImageWriter.TargetFile = fileName;
//        OpenGraphImageWriter.NewImage();
//        OpenGraphImageWriter.WriteText(Header);
//        OpenGraphImageWriter.WriteText(Body);
//        OpenGraphImageWriter.Save();
//    }


//    public static string CreateOpenGraphData(string template, string title, string description, string openGraphImageUrl, string url)
//    {
//        // Open Graph data
//        var s = template;
//        var i = title.Length;
//        if (i > 95) i = 95;

//        s = s.Replace("??Title??", title.Substring(0, i));

//        i = description.Length;
//        if (i > 297) i = 297;

//        s = s.Replace("??Description??", description.Substring(0, i))
//            .Replace("??Image??", openGraphImageUrl)
//            .Replace("??Url??", url);
//        return s;
//    }
//}