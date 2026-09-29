// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.DataProtection.Hashing;

// ReSharper disable InconsistentNaming

/// <summary>
/// Enum of the supported hashing algorithms
/// </summary>
public enum HashAlgorithmEnum
{
    /// <summary>
    /// MD5-Verfahren, das einen Hash mit einer Länge von 128 Bit erzeugt
    /// </summary>
    MD5,

    /// <summary>
    /// SHA-Verfahren, das einen Hash mit einer Länge von 160 Bit erzeugt
    /// </summary>
    SHA1,

    /// <summary>
    /// SHA-Verfahren, das einen Hash mit einer Länge von 256 Bit erzeugt
    /// </summary>
    SHA256,

    /// <summary>
    /// SHA-Verfahren, das einen Hash mit einer Länge von 384 Bit erzeugt
    /// </summary>
    SHA384,

    /// <summary>
    /// SHA-Verfahren, das einen Hash mit einer Länge von 512 Bit erzeugt
    /// </summary>
    SHA512,
}