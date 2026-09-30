// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Security.Cryptography;
using System.Text;

// ReSharper disable InconsistentNaming

namespace Bodoconsult.App.DataProtection.Hashing;

/// <summary>
/// Implements multiple hashing mechanisms
/// </summary>
public class Hasher
{
    private readonly HashAlgorithm _hashObject = SHA1.Create();
    private readonly Encoding _encoding = Encoding.GetEncoding("ISO-8859-1");

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="algorithm">Angabe des zu verwendenden Hash-Algorithmus</param>
    public Hasher(HashAlgorithmEnum algorithm)
    {
        switch (algorithm)
        {
            case HashAlgorithmEnum.MD5:
                _hashObject = MD5.Create();
                break;
            case HashAlgorithmEnum.SHA1:
                _hashObject = SHA1.Create();
                break;
            case HashAlgorithmEnum.SHA256:
                _hashObject = SHA256.Create();
                break;
            case HashAlgorithmEnum.SHA384:
                _hashObject = SHA384.Create();
                break;
            case HashAlgorithmEnum.SHA512:
                _hashObject = SHA512.Create();
                break;
        }
    }

    /// <summary>
    /// The key used for hashing
    /// </summary>
    public string Key
    {
        get
        {
            // Key required ?
            if (_hashObject is KeyedHashAlgorithm kha)
            {
                // Key to string
                return _encoding.GetString(kha.Key);
            }

            throw new NotSupportedException("Der aktuell verwendete Hash-Algorithmus unterstützt keine Schlüssel");
        }

        set
        {
            // Auf Unicode-Zeichen größer 0x00FF überprüfen
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] > 255)
                {
                    throw new CryptographicException(
                        $"The given keys contains atleast one char bigger then 0x00FF (255): {value[i]} ({(int)value[i]}). Only 8-bit unicode is supported");
                }
            }

            // Überprüfen, ob der Algorithmus einen Schlüssel erlaubt,
            // und Speichern des Schlüssel
            if (_hashObject is KeyedHashAlgorithm kha)
            // Schlüssel in einen String umwandeln und zurückgeben
            {
                kha.Key = _encoding.GetBytes(value);
            }
            else
            {
                throw new NotSupportedException("The current hash algorithm does not support keys");
            }
        }
    }

    private static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        var sOutput = new StringBuilder(arrInput.Length);
        for (i = 0; i < arrInput.Length - 1; i++)
        {
            sOutput.Append(arrInput[i].ToString("X2"));
        }
        return sOutput.ToString();
    }

    /// <summary>
    /// Compute the hash value as byte array
    /// </summary>
    /// <param name="inputStream">Input stream</param>
    /// <returns>Hash as byte array</returns>
    public byte[] ComputeHash(Stream inputStream)
    {
        return _hashObject.ComputeHash(inputStream);
    }

    /// <summary>
    /// Compute a string with the hash in hex format
    /// </summary>
    /// <param name="inputStream">Input stream</param>
    /// <returns>Hash as string with hex values</returns>
    public string ComputeHashHex(Stream inputStream)
    {
        return ByteArrayToString(_hashObject.ComputeHash(inputStream));
    }
}