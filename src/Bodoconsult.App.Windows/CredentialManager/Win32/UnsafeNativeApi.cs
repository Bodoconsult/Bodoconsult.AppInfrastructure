// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// Copyright (c) 2020 Widauer Patrick. All rights reserved.

using System;
using System.Runtime.InteropServices;
using Bodoconsult.App.Windows.CredentialManager.Win32.SafeHandles;
using Bodoconsult.App.Windows.CredentialManager.Win32.Types;

namespace Bodoconsult.App.Windows.CredentialManager.Win32;

internal static unsafe class UnsafeNativeApi
{
    [DllImport ("Advapi32.dll", SetLastError = true)]
    public static extern bool CredWriteW (ref Credentialw credential, int flags);

    [DllImport ("Advapi32.dll", SetLastError = true)]
    public static extern bool CredReadW (
        [MarshalAs (UnmanagedType.LPWStr)] string targetName,
        CredentialType type,
        int flags, // use 0 only
        out CredentialSafeHandle credential);

    [DllImport ("Advapi32.dll")]
    public static extern bool CredDeleteW (
        [MarshalAs (UnmanagedType.LPWStr)] string targetName,
        CredentialType type,
        int flags // use 0 only
    );

    [DllImport ("Advapi32.dll")]
    public static extern void CredFree (CredentialwRaw* buffer);

    [DllImport ("Credui.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
    public static extern int CredUIPromptForWindowsCredentialsW (
        ref CreduiInfo uiInfo,
        int errorMessage,
        ref uint authPackage,
        byte[]? authInBuffer,
        uint authInBufferSize,
        out CredentialCoTaskSafeHandle authOutBuffer,
        out uint authOutBufferSize,
        ref bool save,
        CreduiWindow windowType);

    [DllImport ("Credui.dll", SetLastError = true)]
    public static extern bool CredUnPackAuthenticationBufferW (
        CredPack flags,
        CredentialCoTaskSafeHandle authBuffer,
        uint authBufferSize,
        IntPtr userName,
        ref int userNameCapacity,
        IntPtr domain,
        ref int domainCapacity,
        IntPtr password,
        ref int passwordCapacity);
}