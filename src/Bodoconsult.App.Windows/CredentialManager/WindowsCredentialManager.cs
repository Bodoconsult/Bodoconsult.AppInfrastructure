// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Extensions;
using Bodoconsult.App.Windows.CredentialManager.Win32;
using Bodoconsult.App.Windows.CredentialManager.Win32.Blobs;
using Bodoconsult.App.Windows.CredentialManager.Win32.SafeHandles;
using Bodoconsult.App.Windows.CredentialManager.Win32.Types;
using CredentialType = Bodoconsult.App.Abstractions.Interfaces.CredentialType;

namespace Bodoconsult.App.Windows.CredentialManager;

/// <summary>
/// Current impl of <see cref="ICredentialManager"/> on current Win32 systems to handle generic credentials
/// </summary>
public class WindowsCredentialManager : ICredentialManager
{
    /// <summary>
    /// Load credential by unique target name
    /// </summary>
    /// <param name="targetName"></param>
    /// <returns>Credential</returns>
    public ICredentials Load(string targetName)
    {
        return LoadInternal(targetName, CredentialType.Generic);
    }

    private unsafe ICredentials LoadInternal(string targetName, CredentialType type)
    {

        WindowsCredentials credential;

        CredentialSafeHandle handle = null;
        try
        {
            credential = new WindowsCredentials(targetName, type);

            if (!UnsafeNativeApi.CredReadW(targetName, type.ConvertToApiEnum(), 0, out handle))
            {
                throw new Win32Exception();
            }

            if (handle is null)
            {
                throw new Win32Exception();
            }

            var credentialW = handle.AsCredentialW();

            credential.Comment = Marshal.PtrToStringUni(credentialW->Comment);
            credential.LastModified = credentialW->LastWritten.ToDateTime();
            credential.Persistence = credentialW->Persist.ConvertToConsumerEnum();
            credential.TargetAlias = Marshal.PtrToStringUni(credentialW->TargetAlias);
            credential.Attributes = DeserializeAttributes(credentialW->Attributes, credentialW->AttributeCount);

            Deserialize(credentialW, credential);
        }
        catch
        {
            credential = null;
        }
        finally
        {
            handle?.Dispose();

        }

        return credential;
    }

    /// <summary>
    /// Save a credential
    /// </summary>
    /// <param name="credential">Credential to save</param>
    public void Save(ICredentials credential)
    {
        if (credential is not WindowsCredentials wCredential)
        {
            return;
        }

        var freeObjects = new List<IntPtr>();
        SecureBlob blob = null;
        try
        {
            Credentialw credentialW = default;
            credentialW.Flags = CredentialFlags.None;
            credentialW.Type = wCredential.Type.ConvertToApiEnum();
            credentialW.TargetName = wCredential.TargetName;
            credentialW.Comment = wCredential.Comment;
            credentialW.LastWritten = default; // LastWritten is ignored on write
            credentialW.BlobSize = 0;
            credentialW.Blob = null;
            credentialW.Persist = wCredential.Persistence.ConvertToApiEnum();
            credentialW.Attributes = SerializeAttributes(wCredential, freeObjects, out var attributeCount);
            credentialW.AttributeCount = attributeCount;
            credentialW.TargetAlias = wCredential.TargetAlias;
            credentialW.UserName = null;

            Serialize(ref credentialW, wCredential);

            blob = credentialW.Blob;
            credentialW.BlobSize = blob?.Size ?? 0;

            Debug.Assert(credentialW.Type != default);
            Debug.Assert(credentialW.TargetName != null);

            if (!UnsafeNativeApi.CredWriteW(ref credentialW, 0))
            {
                throw new Win32Exception();
            }
        }
        finally
        {
            blob?.Dispose();
            foreach (var freeObject in freeObjects)
            {
                Marshal.FreeHGlobal(freeObject);
            }
        }
    }

    /// <summary>
    /// Delete credentials
    /// </summary>
    /// <param name="targetName">UUnique target name</param>
    /// <returns>true if the credential was deleted else false</returns>
    public bool Delete(string targetName)
    {
        return UnsafeNativeApi.CredDeleteW(targetName, CredentialType.Generic.ConvertToApiEnum(), default);
    }

    internal static void Serialize(ref Credentialw credentialW, WindowsCredentials wCredential)
    {
        var password = wCredential.Password;

        credentialW.UserName = wCredential.UserName;
        credentialW.Blob = password is null
            ? null
            : new RawStringSecureBlob(password);
    }

    internal static unsafe void Deserialize(CredentialwRaw* credentialW, WindowsCredentials credential)
    {
        credential.UserName = Marshal.PtrToStringUni(credentialW->UserName);
        credential.Password = Win32Utility.UniStringToSecureString(credentialW->Blob, credentialW->BlobSize);
    }

    internal static unsafe IntPtr SerializeAttributes(WindowsCredentials wCredential, List<IntPtr> freeObjects, out int count)
    {
        count = wCredential.Attributes.Count;
        if (count == 0)
        {
            return IntPtr.Zero;
        }

        var attributeIntPtr = Marshal.AllocHGlobal(sizeof(CredentialAttributew) * count);
        freeObjects.Add(attributeIntPtr);

        var ptr = (CredentialAttributew*)attributeIntPtr;
        for (var i = 0; i < count; i++)
        {
            var attribute = wCredential.Attributes[i];

            var keyword = Marshal.StringToHGlobalUni(attribute.Keyword);
            if (keyword != IntPtr.Zero)
            {
                freeObjects.Add(keyword);
            }

            var value = Win32Utility.StringToHGlobalUniWithoutTerminator(attribute.Value, out var valueSize);
            if (value != IntPtr.Zero)
            {
                freeObjects.Add(value);
            }

            if (ptr is null)
            {
                continue;
            }
            ptr[i].Keyword = keyword;
            ptr[i].ValueSize = valueSize;
            ptr[i].Value = value;
        }

        return attributeIntPtr;
    }

    private static unsafe List<CredentialAttribute> DeserializeAttributes(IntPtr attributes, int count)
    {
        var result = new List<CredentialAttribute>();

        var ptr = (CredentialAttributew*)attributes;
        if (ptr is null)
        {
            return result;
        }

        for (var i = 0; i < count; i++)
        {
            var keyword = Marshal.PtrToStringUni(ptr[i].Keyword);
            var value = Win32Utility.UniStringToString(ptr[i].Value, ptr[i].ValueSize) ?? string.Empty;

            result.Add(new CredentialAttribute(keyword, value));
        }

        return result;
    }
}