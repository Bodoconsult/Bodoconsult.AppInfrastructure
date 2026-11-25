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
using System.Text;

namespace Bodoconsult.App.WinForms.Helpers;

[Guid("000214EE-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IShellLinkA : IPersistFile
{
    void GetPath([MarshalAs(UnmanagedType.LPStr), Out] StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATAA pfd, SLGP_FLAGS fFlags);

    void GetIDList(out IntPtr ppidl);

    void SetIDList(IntPtr pidl);

    void GetDescription([MarshalAs(UnmanagedType.LPStr), Out] StringBuilder pszName, int cchMaxName);

    void SetDescription([MarshalAs(UnmanagedType.LPStr)] string pszName);

    void GetWorkingDirectory([MarshalAs(UnmanagedType.LPStr), Out] StringBuilder pszDir, int cchMaxPath);

    void SetWorkingDirectory([MarshalAs(UnmanagedType.LPStr)] string pszDir);

    void GetArguments([MarshalAs(UnmanagedType.LPStr), Out] StringBuilder pszArgs, int cchMaxPath);

    void SetArguments([MarshalAs(UnmanagedType.LPStr)] string pszArgs);

    void GetHotkey(out short pwHotkey);

    void SetHotkey(short wHotkey);

    void GetShowCmd(out int piShowCmd);

    void SetShowCmd(int iShowCmd);

    void GetIconLocation([MarshalAs(UnmanagedType.LPStr), Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

    void SetIconLocation([MarshalAs(UnmanagedType.LPStr)] string pszIconPath, int iIcon);

    void SetRelativePath([MarshalAs(UnmanagedType.LPStr)] string pszPathRel, int dwReserved);

    void Resolve(IntPtr hwnd, SLR_FLAGS fFlags);

    void SetPath([MarshalAs(UnmanagedType.LPStr)] string pszFile);
}