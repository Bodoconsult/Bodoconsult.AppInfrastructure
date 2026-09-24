// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.DataProtection;

/// <summary>
/// Fake implementation of <see cref="IDataProtectionService"/>
/// </summary>

public class FakeDataProtectionService : IDataProtectionService
{
    /// <summary>
    /// Store a value in a safe manner
    /// </summary>
    /// <param name="key">Key name for the value</param>
    /// <param name="value">Value to store</param>
    public string Protect(string key, string value)
    {
        return value;
    }

    /// <summary>
    /// Load a value stored in a safe manner
    /// </summary>
    /// <param name="key">Key name for the value</param>
    /// <param name="cipherValue">The encrypted value to decrypt</param>
    public string Unprotect(string key, string cipherValue)
    {
        return cipherValue;
    }
}