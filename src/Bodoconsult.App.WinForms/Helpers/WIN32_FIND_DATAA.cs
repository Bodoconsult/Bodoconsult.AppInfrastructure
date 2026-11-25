// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

/**************************************************************************
 *
 * Filename:     ShellShortcut.cs
 * Author:       Mattias Sjögren (mattias@mvps.org)
 *               http://www.msjogren.net/dotnet/
 *
 * Description:  Defines a .NET friendly class, ShellShortcut, for reading
 *               and writing shortcuts.
 *               Define the conditional compilation symbol UNICODE to use
 *               IShellLinkW internally.
 *
 * Public types: class ShellShortcut
 *
 * Dependencies: ShellLinkNative.cs
 *
 * Copyright ©2001-2002, Mattias Sjögren
 *
 **************************************************************************/

using System.Runtime.InteropServices;
// ReSharper disable UnusedMember.Global
#pragma warning disable CS0649

namespace Bodoconsult.App.WinForms.Helpers;

// ReSharper disable once InconsistentNaming
internal struct WIN32_FIND_DATAA
{
    private const int MAX_PATH = 260;
    public int dwFileAttributes;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
    public int nFileSizeHigh;
    public int nFileSizeLow;
    public int dwReserved0;
    public int dwReserved1;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
    public string cFileName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
    public string cAlternateFileName;
}