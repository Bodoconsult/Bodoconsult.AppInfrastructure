// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.AspNetCore.DataProtection;

// https://duendesoftware.com/blog/20250313-data-protection-aspnetcore-duende-identityserver

// https://andrewlock.net/an-introduction-to-the-data-protection-system-in-asp-net-core/

// https://github.com/dotnet/aspnetcore/blob/main/src/DataProtection/

namespace Bodoconsult.App.DataProtection;

/// <summary>
/// Current implementation of IDataProtectionService
/// </summary>
public class DataProtectionService : IDataProtectionService
{
    private readonly IDataProtectionProvider _dataProtectionProvider;

    /// <summary>
    /// Static factory method for creating a DataProtectionService instance without a DI container
    /// </summary>
    /// <param name="destinationFolderPath">The folder path the encrypted file is stored in</param>
    /// <param name="appName">App name</param>
    /// <returns></returns>
    public static DataProtectionService CreateInstance(string destinationFolderPath, string appName)
    {
        var dirInfo = new DirectoryInfo(destinationFolderPath);

        var provider = BodoDataProtectionProvider.Create(dirInfo, appName);

        var service = new DataProtectionService(destinationFolderPath, provider);

        return service;
    }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="destinationFolderPath">The folder path the encrypted file is stored in</param>
    /// <param name="dataProtectionProvider">Data protection provide to use</param>
    public DataProtectionService(string destinationFolderPath, IDataProtectionProvider dataProtectionProvider)
    {
        DestinationFolderPath = destinationFolderPath;
        _dataProtectionProvider = dataProtectionProvider;
    }

    /// <summary>
    /// The folder path the encrypted file is stored in
    /// </summary>
    public string DestinationFolderPath { get; }

    /// <summary>
    /// Store a value in a safe manner
    /// </summary>
    /// <param name="key">Key name for the value</param>
    /// <param name="value">Value to store</param>
    public string Protect(string key, string value)
    {
        var protector = _dataProtectionProvider.CreateProtector(key);

        // Protect the payload
        var result = protector.Protect(value);

        return result;
    }

    /// <summary>
    /// Load a value stored in a safe manner
    /// </summary>
    /// <param name="key">Key name for the value</param>
    /// <param name="cipherValue">The encrypted value to decrypt</param>
    public string Unprotect(string key, string cipherValue)
    {
        var protector = _dataProtectionProvider.CreateProtector(key);
        var value = protector.Unprotect(cipherValue);
        return value;
    }
}