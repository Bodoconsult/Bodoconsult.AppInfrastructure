// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection.FileProtection;
using Bodoconsult.App.Helpers;
using System.Text;
using System.Text.Json;

namespace Bodoconsult.App.DataProtection.ConsoleTools;

/// <summary>
/// Manage <see cref="BasicCredentials"/> for an app
/// </summary>
public class BasicCredentialsManager
{
    private IDataProtectionManager _dpm = new DoNothingDataProtectionManager();
    private string? _folderPath;
    private string _transferTargetPath = string.Empty;

    /// <summary>
    /// Default ctor
    /// </summary>
    public BasicCredentialsManager()
    {
        TransferTargetPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        ReadStringDelegate = ReadFromConsole; 
    }

    private static string ReadFromConsole(string message)
    {
        Console.WriteLine($"{message}:");
        var s = PasswordHandler.ReadPassword();
        return s;
    }
    
    /// <summary>
    /// Name of the credential set
    /// </summary>
    public string Name { get; set; } = "Credentials";

    /// <summary>
    /// Target folder path for the transfer file
    /// </summary>
    public string TransferTargetPath
    {
        get => _transferTargetPath;
        set
        {
            _transferTargetPath = value;
            TransferFileName = Path.Combine(_transferTargetPath, $"{AppName}Transfer.json");
        }
    }

    /// <summary>
    /// Full filename of the transfer file
    /// </summary>
    public string TransferFileName { get; private set; } = string.Empty;

    /// <summary>
    /// Current folder path to store the credentials in
    /// </summary>
    public string? FolderPath
    {
        get => _folderPath;
        set
        {
            _folderPath = value;

            ArgumentNullException.ThrowIfNull(_folderPath);

            FilePath = Path.Combine(_folderPath, $"appData.{Extension}");
        }
    }

    /// <summary>
    /// App name. Must not contain invalid chars in file names
    /// </summary>
    public string AppName { get; set; } = "Default";

    ///// <summary>
    ///// Key 1
    ///// </summary>
    //public string Key { get; set; } = "Key1";

    ///// <summary>
    ///// Key 1
    ///// </summary>
    //public string Key2 { get; set; } = "Key2";

    ///// <summary>
    ///// Key 1
    ///// </summary>
    //public string Key3 { get; set; } = "Key3";

    /// <summary>
    /// Extension to use for stroage file. Default: .dat
    /// </summary>
    public string Extension { get; set; } = "dat";

    /// <summary>
    /// Filepath of the secrets file
    /// </summary>
    public string? FilePath { get; private set; }

    /// <summary>
    /// Current credentials
    /// </summary>
    public BasicCredentials? Credentials { get; set; }

    /// <summary>
    /// Delegate to read a string input from console, UI, etc.. Default is reading from console. If needed implement your own <see cref="ReadStringDelegate "/> method
    /// </summary>
    /// <returns>Read string input</returns>
    public ReadStringDelegate ReadStringDelegate { get; set; }

    /// <summary>
    /// Initialize the data protection after settings props like <see cref="AppName"/> etc.
    /// </summary>
    public void Init()
    {
        ArgumentNullException.ThrowIfNull(FolderPath);
        ArgumentNullException.ThrowIfNull(FilePath);

        // Basic setup of data protection
        Credentials = new BasicCredentials
        {
            Name = Name
        };

        var instance = DataProtectionService.CreateInstance(FolderPath, AppName);

        var fs = new SimpleFileProtectionService();
        _dpm = new DataProtectionManager(instance, fs, FilePath);
        _dpm.ReadStringDelegate = ReadStringDelegate;
        
        // Load the values from an existing file
        _dpm.LoadValues();

        // Load the values into the credentials entity
        _dpm.Unprotect(Credentials);
    }

    /// <summary>
    /// Load the values the first time from console, UI, etc.. Overrides existing secrets file.
    /// </summary>
    public void AskForInitialLoadValues()
    {
        _dpm.AskForInitialLoadValues();
    }

    /// <summary>
    /// Protect the credentials
    /// </summary>
    public void Protect()
    {
        ArgumentNullException.ThrowIfNull(Credentials);

        var cr = new BasicCredentials
        {
            Name = Credentials.Name,
            Password = Credentials.Password,
            Url = Credentials.Url,
            Username = Credentials.Username
        };

        _dpm.Protect(cr);
    }

    /// <summary>
    /// Unprotect the credentials
    /// </summary>
    public void Unprotect()
    {
        ArgumentNullException.ThrowIfNull(Credentials);
        _dpm.Unprotect(Credentials);
    }

    /// <summary>
    /// Save the credentials into a transfer file stored in Path.Combine(TransferTargetPath, $"{AppName}Transfer.json")
    /// </summary>
    public void SaveAsTransferFile()
    {
        ArgumentNullException.ThrowIfNull(Credentials);

        var json = JsonSerializer.Serialize(Credentials);

        File.WriteAllText(TransferFileName, json, Encoding.UTF8);
    }

    /// <summary>
    /// Load the credentials from a transfer file stored in Path.Combine(TransferTargetPath, $"{AppName}Transfer.json"). After reading the transfer file successfully it is deleted
    /// </summary>
    public void LoadFromTransferFile()
    {
        ArgumentNullException.ThrowIfNull(Credentials);

        var json = File.ReadAllText(TransferFileName, Encoding.UTF8);

        Credentials = JsonSerializer.Deserialize<BasicCredentials>(json);

        if (File.Exists(TransferFileName))
        {
            File.Delete(TransferFileName);
        }
    }
}