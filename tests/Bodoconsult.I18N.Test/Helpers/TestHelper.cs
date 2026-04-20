// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using System.Reflection;

namespace Bodoconsult.I18N.Test.Helpers;

internal static class TestHelper
{
    static TestHelper()
    {
        CurrentAssembly = typeof(TestHelper).Assembly;

        var s = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(s);

        var fi = new FileInfo(s);
        ArgumentNullException.ThrowIfNull(fi.DirectoryName);
        GetFolderPath = fi.DirectoryName;
    }

    public static Assembly CurrentAssembly { get; }


    public static string GetFolderPath { get; }
}