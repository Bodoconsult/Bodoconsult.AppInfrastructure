// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.IO;
using System.Security.Cryptography;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Windows.DataProtection;

/// <summary>
/// Credential manager for test environments
/// </summary>
public class TestCredentialManager<T> where T : class
{
    private const string KeyName = "Test";

    /// <summary>
    /// The current app of the app
    /// </summary>
    private readonly string? _appPath;

    /// <summary>
    /// The current path in C:\ProgrammData for the data of the app
    /// </summary>
    private readonly string? _dataPath;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appName">App name</param>

    public TestCredentialManager(string appName)
    {
        DataProtectionService = DataProtection.DataProtectionService.CreateInstance(appName, DataProtectionScope.LocalMachine);

        _appPath = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(_appPath);

        _appPath = new FileInfo(_appPath).DirectoryName;
        ArgumentNullException.ThrowIfNull(_appPath);

        _dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName);

        if (!Directory.Exists(_dataPath))
        {
            Directory.CreateDirectory(_dataPath);
        }

        CredentialFileName = Path.Combine(_appPath, "data1.dat");
        CredentialImportFileName = Path.Combine(_appPath, "cred.json");
    }

    /// <summary>
    /// Current data protection service instance to use for protecting secrets
    /// </summary>
    public IDataProtectionService DataProtectionService { get; set; }

    /// <summary>
    /// Filename of the credential storage file
    /// </summary>
    public string CredentialFileName { get; }

    /// <summary>
    /// Filename of the credential storage import file. To be deleted after import
    /// </summary>
    public string CredentialImportFileName { get; }

    /// <summary>
    /// Current credentials or null
    /// </summary>
    public T? Credentials { get; set; }

    /// <summary>
    /// Load the credentials
    /// </summary>
    public void LoadCredentials()
    {
        if (File.Exists(CredentialFileName))
        {
            var crypted = File.ReadAllText(CredentialFileName);

            var json = DataProtectionService.Unprotect(KeyName, crypted);

            Credentials = SimpleJsonHelper.LoadJsonString<T>(json);
        }
        else
        {
            if (!File.Exists(CredentialImportFileName))
            {
                return;
            }

            var json = File.ReadAllText(CredentialImportFileName);

            Credentials = SimpleJsonHelper.LoadJsonString<T>(json);

            SaveCredentials();
        }
    }

    /// <summary>
    /// Save the credntials
    /// </summary>
    public void SaveCredentials()
    {
        var json = SimpleJsonHelper.Serialize(Credentials);

        var crypted = DataProtectionService.Protect(KeyName, json);

        File.WriteAllText(CredentialFileName, crypted);

        FileSystemHelper.DeleteFile(CredentialImportFileName);
    }
}