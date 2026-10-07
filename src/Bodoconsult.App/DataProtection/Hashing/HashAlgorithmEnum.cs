// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.DataProtection.Hashing;

// ReSharper disable InconsistentNaming

/// <summary>
/// Enum of the supported hashing algorithms
/// </summary>
public enum HashAlgorithmEnum
{
    /// <summary>
    /// MD5-algorithm create a hash with the length of 128 bit
    /// </summary>
    MD5,

    /// <summary>
    /// (Keyless) SHA-algorithm create a hash with the length of 160 bit 
    /// </summary>
    SHA1,

    /// <summary>
    /// (Keyless) SHA-algorithm create a hash with the length of 256 bit
    /// </summary>
    SHA256,

    /// <summary>
    /// (Keyless) SHA-algorithm create a hash with the length of 384 bit
    /// </summary>
    SHA384,

    /// <summary>
    /// (Keyless) SHA-algorithm create a hash with the length of 512 bit
    /// </summary>
    SHA512,
}