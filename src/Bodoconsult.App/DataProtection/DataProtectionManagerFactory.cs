// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.DataProtection;

/// <summary>
/// Factory creating a singleton DataProtectionManager instance
/// </summary>
public class DataProtectionManagerFactory : IDataProtectionManagerFactory
{
    private readonly string _appName = "Default";

    private readonly IDataProtectionServiceFactory _dataProtectionServiceFactory;

    private readonly IFileProtectionService _fileProtectionService;

    private IDataProtectionManager? _dataProtectionManager;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataProtectionServiceFactory">´Current factory for IDataProtectionService instances</param>
    /// <param name="fileProtectionService">Current IFileProtectionService instance</param>
    public DataProtectionManagerFactory(IDataProtectionServiceFactory dataProtectionServiceFactory, IFileProtectionService fileProtectionService)
    {
        _dataProtectionServiceFactory = dataProtectionServiceFactory;
        _fileProtectionService = fileProtectionService;
    }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataProtectionServiceFactory">´Current factory for IDataProtectionService instances</param>
    /// <param name="fileProtectionService">Current IFileProtectionService instance</param>
    /// <param name="appName">Current app name</param>
    public DataProtectionManagerFactory(IDataProtectionServiceFactory dataProtectionServiceFactory, IFileProtectionService fileProtectionService, string appName)
    {
        _dataProtectionServiceFactory = dataProtectionServiceFactory;
        _fileProtectionService = fileProtectionService;
        _appName = appName;
    }


    /// <summary>
    /// Create an IDataProtectionManager instance
    /// </summary>
    /// <param name="destinationFilePath">File path to store the secrets storage</param>
    /// <returns>IDataProtectionManager instance</returns>
    public IDataProtectionManager CreateInstance(string destinationFilePath)
    {
        if (_dataProtectionManager != null)
        {
            return _dataProtectionManager;
        }

        var dir = new DirectoryInfo(destinationFilePath);

        var dataService = _dataProtectionServiceFactory.CreateInstance(dir.FullName, _appName);

        _dataProtectionManager = new DataProtectionManager(dataService, _fileProtectionService, destinationFilePath);
        return _dataProtectionManager;
    }
}
