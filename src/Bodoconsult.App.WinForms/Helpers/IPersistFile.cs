// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.
// 
// /**************************************************************************
// *
// * Filename:     ShellShortcut.cs
// * Author:       Mattias Sjögren (mattias@mvps.org)
// *               http://www.msjogren.net/dotnet/
// *
// * Description:  Defines a .NET friendly class, ShellShortcut, for reading
// *               and writing shortcuts.
// *               Define the conditional compilation symbol UNICODE to use
// *               IShellLinkW internally.
// *
// * Public types: class ShellShortcut
// *
// * Dependencies: ShellLinkNative.cs
// *
// * Copyright ©2001-2002, Mattias Sjögren
// *
// **************************************************************************/

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Bodoconsult.App.WinForms.Helpers;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("0000010B-0000-0000-C000-000000000046")]
[ComImport]
internal interface IPersistFile
{
    void GetClassID(out Guid pClassId);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int IsDirty();

    void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);

    void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

    void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

    void GetCurFile(out IntPtr ppszFileName);
}