// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Runtime.Versioning;
using Bodoconsult.App.Windows.System;

namespace BodoCmsConsole.Business.Helpers;

[SupportedOSPlatform("windows")]
public static class IconHelper
{

    internal static  IconsAsFilesHelper Icons = new();

    public static void AddExtension(FileInfo fri)
    {
        Icons.AddExtension(fri);
    }

    public static void SetPath(string pathHomepage)
    {
        Icons.IconPath = pathHomepage;
    }

    public static void SaveIcons()
    {
        Icons.SaveIcons();
    }
}