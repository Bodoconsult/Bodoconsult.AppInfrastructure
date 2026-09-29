// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.DataProtection.FileProtection;

/// <summary>
/// <see cref="IFileProtectionService"/> implementation writing data unprotected
/// </summary>
public class NoFileProtectionService : IFileProtectionService
{
    /// <summary>
    /// Protect unprotected data
    /// </summary>
    /// <param name="data">Unprotected data</param>
    /// <returns>Protected data</returns>
    public byte[] Protect(byte[] data)
    {
        var copy = new byte[data.Length];
        data.AsSpan().CopyTo(copy);
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
        return copy;
    }
}