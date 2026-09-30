// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Windows.DataProtection.Hashing;

/// <summary>
/// Interface for password hasher implementations
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Calculate a hash value for a password (or another string) 
    /// </summary>
    /// <param name="password">Password to hash</param>
    /// <returns>Hash value for the password</returns>
    string Hash(string password);

    /// <summary>
    /// Check if a hash value fits to a given password
    /// </summary>
    /// <param name="hash">Hash value to check</param>
    /// <param name="password">Password to compare with given hash value</param>
    /// <returns>A tupel with a value Verified being true if the hask value fits to the password and a NeedsUpgrade value inidicating that the number of ierations has changed and the hash value has to be renewed</returns>
    (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
}