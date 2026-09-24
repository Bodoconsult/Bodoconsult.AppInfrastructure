// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Runtime.InteropServices;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection.FileProtection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Bodoconsult.App.DataProtection.DependencyInjection;

/// <summary>
/// Adding all DI container service to implement data protection with a simple file protection
/// </summary>
public class SimpleDataProtectionDiContainerServiceProvider : IDiContainerServiceProvider
{
    private readonly string _destinationFolderPath;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="destinationFolderPath">Folder path to store the key storage infrastructure in</param>
    public SimpleDataProtectionDiContainerServiceProvider(string destinationFolderPath)
    {
        if (string.IsNullOrEmpty(destinationFolderPath))
        {
            throw new ArgumentException("Folder path for key infrastructure may not be null");
        }

        _destinationFolderPath = destinationFolderPath;
    }


    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        var builder = diContainer.ServiceCollection.AddDataProtection();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            builder.ProtectKeysWithDpapi();
        }

        if (string.IsNullOrEmpty(_destinationFolderPath))
        {
            throw new ArgumentException("Folder path for key infrastructure may not be null");
        }
        builder.PersistKeysToFileSystem(new DirectoryInfo(_destinationFolderPath));
        diContainer.AddSingleton<IFileProtectionService, SimpleFileProtectionService>();
        diContainer.AddSingleton<IDataProtectionServiceFactory, DataProtectionServiceFactory>();
        diContainer.AddSingleton<IDataProtectionManagerFactory, DataProtectionManagerFactory>();
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references
    /// </summary>
    /// <param name="diContainer"></param>
    public void LateBindObjects(DiContainer diContainer)
    {
        // Do nothing
    }
}