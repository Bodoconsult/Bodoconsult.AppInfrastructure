// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.DataProtection.FileProtection;

/// <summary>
/// Fake implementation of <see cref="IFileProtectionService"/>
/// </summary>
public class FakeFileProtectionService : IFileProtectionService
{
    /// <summary>
    /// Protect unprotected data
    /// </summary>
    /// <param name="data">Unprotected data</param>
    /// <returns>Protected data</returns>
    public byte[] Protect(byte[] data)
    {
        return data;
    }

    /// <summary>
    /// Unprotect protected data
    /// </summary>
    /// <param name="data">Protected data</param>
    /// <returns>Unprotected</returns>
    public byte[] Unprotect(byte[] data)
    {
        return data;
    }
}