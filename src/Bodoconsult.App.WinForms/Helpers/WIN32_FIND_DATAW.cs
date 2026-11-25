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
using System.Runtime.InteropServices.ComTypes;

namespace Bodoconsult.App.WinForms.Helpers;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct WIN32_FIND_DATAW
{
    public int dwFileAttributes;
    public FILETIME ftCreationTime;
    public FILETIME ftLastAccessTime;
    public FILETIME ftLastWriteTime;
    public int nFileSizeHigh;
    public int nFileSizeLow;
    public int dwReserved0;
    public int dwReserved1;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
    public string cFileName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
    public string cAlternateFileName;
    private const int MAX_PATH = 260;
}