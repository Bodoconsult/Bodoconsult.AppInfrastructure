// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.DataProtection;

/// <summary>
/// Current implementaion of <see cref="IDataProtectionServiceFactory"/> providing a <see cref="DataProtectionService"/> instance
/// </summary>
public class DataProtectionServiceFactory : IDataProtectionServiceFactory
{
    /// <summary>
    /// Create an IDataProtectionService instance
    /// </summary>
    /// <param name="destinationFolderPath">Destination folder path to store the data protection key infrastructure in</param>
    /// <param name="appName">App name</param>
    /// <returns>IDataProtectionService instance</returns>
    public IDataProtectionService CreateInstance(string destinationFolderPath, string appName)
    {
        var dirInfo = new DirectoryInfo(destinationFolderPath);

        var provider = BodoDataProtectionProvider.Create(dirInfo, appName);

        var dataProtectionService = new DataProtectionService(destinationFolderPath, provider);

        return dataProtectionService;
    }
}