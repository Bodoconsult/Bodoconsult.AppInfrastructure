// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// Copyright (c) 2020 Widauer Patrick. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Bodoconsult.App.Windows.CredentialManager.Win32.Types;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct Credentialw
{
    public Credentialw()
    {
        
    }
    public CredentialFlags Flags { get; set; }= CredentialFlags.None;
    public CredentialType Type = CredentialType.Generic;
    [MarshalAs (UnmanagedType.LPWStr)] public string TargetName = string.Empty;
    [MarshalAs (UnmanagedType.LPWStr)] public string? Comment;
    public FILETIME? LastWritten;
    public int BlobSize;
    public SecureBlob? Blob;
    public CredentialPersist Persist = CredentialPersist.Session;
    public int AttributeCount;
    public IntPtr Attributes;
    [MarshalAs (UnmanagedType.LPWStr)] public string? TargetAlias;
    [MarshalAs (UnmanagedType.LPWStr)] public string? UserName;
}