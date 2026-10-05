// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Runtime.InteropServices;

namespace Bodoconsult.App.Windows.Helpers;

/// <summary>
/// OS helper class
/// </summary>
public static class OsHelper
{
    /// <summary>
    /// Is the current OS a server version?
    /// </summary>
    /// <returns>true if server version</returns>
    public static bool IsWindowsServer()
    {
        return IsOS(OsAnyserver);
    }

    const int OsAnyserver = 29;

    [DllImport("shlwapi.dll", SetLastError = true, EntryPoint = "#437")]
    private static extern bool IsOS(int os);
}