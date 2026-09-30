// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Linq;
using System.Security.Cryptography;

namespace Bodoconsult.App.Windows.DataProtection.Hashing;

/// <summary>
/// Simple class for hashing passwords
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bit 
    private const int KeySize = 32; // 256 bit

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="options">Current hashing options</param>
    public PasswordHasher(HashingOptions options)
    {
        Options = options;
    }

    /// <summary>
    /// Current hashing options
    /// </summary>
    public HashingOptions Options { get; }

    /// <summary>
    /// Calculate a hash value for a password (or another string) 
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public string Hash(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            SaltSize,
            Options.Iterations,
            HashAlgorithmName.SHA512);
        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{Options.Iterations}.{salt}.{key}";
    }

    /// <summary>
    /// Check if a hash value fits to a given password
    /// </summary>
    /// <param name="hash">Hash value to check</param>
    /// <param name="password">Password to compare with given hash value</param>
    /// <returns>A tupel with a value Verified being true if the hask value fits to the password and a NeedsUpgrade value inidicating that the number of ierations has changed and the hash value has to be renewed</returns>
    public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
    {
        var parts = hash.Split('.', 3);

        if (parts.Length != 3)
        {
            throw new FormatException("Unexpected hash format. Should be formatted as `{iterations}.{salt}.{hash}`");
        }

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        var needsUpgrade = iterations != Options.Iterations;

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA512);
        var keyToCheck = algorithm.GetBytes(KeySize);

        var verified = keyToCheck.SequenceEqual(key);

        return (verified, needsUpgrade);
    }
}