// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.Office.Test.Helpers;

public static class FileHelper
{

    public static string TempPath { get; set; } = Path.GetTempPath();

    public static void StartExcel(string path)
    {
        FileSystemHelper.RunInDebugMode(path);
    }
}