// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using NotImplementedException = System.NotImplementedException;

namespace Bodoconsult.App.Windows.DataProtection;

/// <summary>
/// Service to protect data in a secure way depending on user account or machine account
/// </summary>
public class DataProtectionService : IDataProtectionService
{
    /// <summary>
    /// Current protection scope
    /// </summary>
    public DataProtectionScope CurrentDataProtectionScope { get; set; } = DataProtectionScope.CurrentUser;

    /// <summary>
    /// Byte array containing entropy data to be used for encryption.
    /// </summary>
    public byte[] EntropyBytes { get; set; } = [0, 1, 2, 3, 4, 1, 2, 3, 4, 9];

    /// <summary>
    /// Encrypt the data using the current scope. The result can be decrypted only by the same scope
    /// </summary>
    /// <param name="data">Data to encrypt</param>
    /// <returns>Encrypted byte array</returns>
    public byte[] Protect(byte[] data)
    {
        try
        {
            return ProtectedData.Protect(data, EntropyBytes, CurrentDataProtectionScope);
        }
        catch (CryptographicException e)
        {
            Debug.Print("Data was not encrypted. An error occurred.");
            Debug.Print(e.ToString());
            throw;
        }
    }

    /// <summary>
    /// Encrypt a string using the current scope. The result can be decrypted only by the same scope
    /// </summary>
    /// <param name="secret">Data to encrypt</param>
    /// <returns>Encrypted byte array</returns>
    public byte[] ProtectString(string secret)
    {
        var bytes = Encoding.Unicode.GetBytes(secret);
        var result = Protect(bytes);
        return result;
    }

    /// <summary>
    /// Decrypt data encrypted by the same scope
    /// </summary>
    /// <param name="data">Byte array with encrypted data</param>
    /// <returns>Byte array with the decrypted data</returns>
    public byte[] Unprotect(byte[] data)
    {
        try
        {
            Debug.Print(ArrayHelper.GetStringFromArrayCsharpStyle(data, false));

            //Decrypt the data using the current scope
            return ProtectedData.Unprotect(data, EntropyBytes, CurrentDataProtectionScope);
        }
        catch (CryptographicException e)
        {
            Debug.Print("Data was not decrypted. An error occurred.");
            Debug.Print(e.ToString());
            return [];
        }
    }

    /// <summary>
    /// Decrypt a string encrypted by the same scope
    /// </summary>
    /// <param name="data">Byte array with encrypted string data</param>
    /// <returns>String with the decrypted data</returns>
    public string UnprotectString(byte[] data)
    {
        var result = Unprotect(data);
        return Encoding.Unicode.GetString(result);
    }

    /// <summary>
    /// Store a value in a safe manner
    /// </summary>
    /// <param name="key">Key name for the value</param>
    /// <param name="value">Value to store</param>
    public string Protect(string key, string value)
    {
        var result = ProtectString(value);

        Debug.Print(ArrayHelper.GetStringFromArrayCsharpStyle(result,  false));

        return ArrayHelper.GetStringFromArrayCsharpStyle(result, false);
    }

    /// <summary>
    /// Load a value stored in a safe manner
    /// </summary>
    /// <param name="key">Key name for the value</param>
    /// <param name="cipherValue">The encrypted value to decrypt</param>
    public string Unprotect(string key, string cipherValue)
    {
        var bytes = ArrayHelper.GetBytes(cipherValue);
        var result = Unprotect(bytes);
        return Encoding.Unicode.GetString( result);
    }

    /// <summary>
    /// Create an instance of <see cref="DataProtectionService"/>
    /// </summary>
    /// <param name="appName">Current app name</param>
    /// <returns><see cref="DataProtectionService"/> instance</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static IDataProtectionService CreateInstance(string appName)
    {
        var s = new DataProtectionService
        {
            EntropyBytes = Encoding.Unicode.GetBytes(appName)
        };
        return s;
    }
}