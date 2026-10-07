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
    private readonly HashAlgorithm _hashObject = SHA256.Create();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="algorithm">Angabe des zu verwendenden Hash-Algorithmus</param>
    public Hasher(HashAlgorithmEnum algorithm = HashAlgorithmEnum.SHA256)
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
    /// Create a random key
    /// </summary>
    /// <returns>Random key as string</returns>
    public static string CreateRandomKey()
    {
        var salt = new byte[256];

        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return Convert.ToBase64String(salt);
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
                return Convert.ToBase64String(kha.Key);
            }

            throw new NotSupportedException("The current hashing algorithm does not support keys");
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
                kha.Key = Convert.FromBase64String(value);
            }
            else
            {
                throw new NotSupportedException("The current hashing algorithm does not support keys");
            }
        }
    }

    private static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        var sOutput = new StringBuilder();
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
    public byte[] ComputeHashAsArray(Stream inputStream)
    {
        inputStream.Position = 0;
        return _hashObject.ComputeHash(inputStream);
    }

    /// <summary>
    /// Compute a string with the hash in hex format
    /// </summary>
    /// <param name="inputStream">Input stream</param>
    /// <returns>Hash as string with hex values</returns>
    public string ComputeHash(Stream inputStream)
    {
        inputStream.Position = 0;
        return ByteArrayToString(_hashObject.ComputeHash(inputStream));
    }

    /// <summary>
    /// Compute a string with the hash in hex format
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Hash as string with hex values</returns>

    public string ComputeHash(string input)
    {
        var bytes = Encoding.Unicode.GetBytes(input);
        return ByteArrayToString(_hashObject.ComputeHash(bytes));
    }

    /// <summary>
    /// Hashes a value and compares with another hashed value
    /// </summary>
    /// <param name="pureValue">Pure value like a password</param>
    /// <param name="hashedValue">Hashed value to compare with</param>
    /// <returns>True if the hash value is correct for the given pureValue</returns>
    public bool ValidateHash(string pureValue, string hashedValue)
    {
        try
        {
            var calcHash = ComputeHash(pureValue);
            return calcHash.Equals(hashedValue, StringComparison.Ordinal);
        }
        catch
        {
            return false;
        }
    }


    /// <summary>
    /// Hashes a value and compares with another hashed value
    /// </summary>
    /// <param name="pureInputStream">Pure stream with data to hash</param>
    /// <param name="hashedValue">Hashed value to compare with</param>
    /// <returns>True if the hash value is correct for the given pureInputStream</returns>
    public bool ValidateHash(Stream pureInputStream, string hashedValue)
    {
        try
        {
            var calcHash = ComputeHash(pureInputStream);
            return calcHash.Equals(hashedValue, StringComparison.Ordinal);
        }
        catch
        {
            return false;
        }
    }
}