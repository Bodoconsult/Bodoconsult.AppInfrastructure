// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.DataProtection.FileProtection;

/// <summary>
/// Simple <see cref="IFileProtectionService"/> implementation not intended to protect secret data with a high security level
/// </summary>
public class SimpleFileProtectionService : IFileProtectionService
{
    /// <summary>
    /// Defines the XOR value to use
    /// </summary>
    public int XorValue { get; set; } = 0x41;
    
    /// <summary>
    /// Protect unprotected data
    /// </summary>
    /// <param name="data">Unprotected data</param>
    /// <returns>Protected data</returns>
    public byte[] Protect(byte[] data)
    {

        var copy = new byte[data.Length];
        data.AsSpan().CopyTo(copy);

        for (var i = 0; i < copy.Length; i++)
        {
            copy[i] = (byte)(copy[i] ^ XorValue);
        }

        return copy;
    }

    /// <summary>
    /// Unprotect protected data
    /// </summary>
    /// <param name="data">Protected data</param>
    /// <returns>Unprotected</returns>
    public byte[] Unprotect(byte[] data)
    {
        var copy = new byte[data.Length];
        data.AsSpan().CopyTo(copy);

        for (var i = 0; i < copy.Length; i++)
        {
            copy[i] = (byte)(copy[i] ^ XorValue);
        }

        return copy;
    }
}